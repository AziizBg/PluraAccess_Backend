
using Microsoft.AspNetCore.Mvc;
using OddoBhf.Dto;
using OddoBhf.Interfaces;
using OddoBhf.Models;
using System.Text.Json;
using System.Text;
using System.Net.Http;
using Microsoft.AspNetCore.SignalR;
using OddoBhf.Hub;

namespace OddoBhf.Services
{
    public class LicenceService:ILicenceService
    {
        private readonly ILicenceRepository _licenceRepository;
        private readonly ISessionService _sessionService;
        private readonly IQueueService _queueService;
        private readonly IUserService _userService;
        private readonly HttpClient _httpClient;
        private readonly IHubContext<NotificationHub, INotificationHub> _notification;


        public LicenceService(ILicenceRepository licenceRepository, ISessionService sessionService,IQueueService queueService, IUserService userService, HttpClient httpClient, IHubContext<NotificationHub, INotificationHub> hubContext) {
            _licenceRepository = licenceRepository;
            _sessionService = sessionService;
            _queueService = queueService;
            _userService = userService;
            _httpClient = httpClient;
            _notification= hubContext;
        }
        public Licence GetLicenceById(int id)
        {
            return _licenceRepository.GetLicenceById(id);
        }
        public Licence GetLicenceBookedByUserId(int id)
        {
            return _licenceRepository.GetLicenceBookedByUserId(id);
        }

        public ICollection<GetLicenceDto> GetLicences()
        {
            return _licenceRepository.GetAllLicences();
        }
        public void CreateLicence([FromBody] Licence licence)
        {
             _licenceRepository.AddLicence(licence);
        }

        public void UpdateLicence( Licence licence)
        {
            _licenceRepository.UpdateLicence(licence);
            _notification.Clients.All.SendMessage(new Notification
            {
                Message = "Licence " + licence.Id + " Updated"
            });
        }
        public void DeleteLicence(int id)
        {
            _licenceRepository.DeleteLicence(id);
        }

        public async Task<Licence> TakeLicence(int id, OpenPluralsightDto dto)
        {
            //check if the licence exists and has no current session only if fromQueue = true
            var licence = _licenceRepository.GetLicenceById(id);
            if (licence == null || licence.CurrentSession != null && dto.FromQueue==false)
            {
                return null;
            }

            //check if the user is given and exists
            var user = _userService.GetUserById(dto.UserId);
            if (user == null)
            {
                throw new Exception("User not provided");
            }

            try
            {
                var url = dto.NgorkUrl != null ? dto.NgorkUrl + "/get_cookie" : "http://127.0.0.1:5000/get_cookie";
                var email = "sami.belhadj@oddo-bhf.com";
                var password = "7cB3MP.6y9.Z?c?";

                //licence being requested to disable other requests for it 
                licence.BookedByUserId = user.Id;
                _licenceRepository.UpdateLicence(licence);
                user.BookedLicenceId = id;
                _userService.UpdateUser(user.Id, user);

                // send licence requested notification to disable taking this licence from everyone
                var excludedConnectionIds = new List<string> { user.ConnectionId };
                await _notification.Clients.AllExcept(excludedConnectionIds).SendMessage(new Notification
                {
                    Title = "Licence Requested",
                    Message = user.Name + " has requested licence number " + licence.Id,
                    UserId = user.Id,
                });
                
                //send request to the flask server to open pluralsight
                var payload = new { email, password, formattedEndTime = DateTime.Now.AddHours(2), licenceId = licence.Id };
                var jsonPayload = JsonSerializer.Serialize(payload);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, content);

                //create the actual session if the flask resquest is succeeded
                var startTime = DateTime.Now;
                var endTime = DateTime.Now.AddHours(2);
                var session = new Session
                {
                    StartTime = startTime,
                    EndTime = endTime,
                    Licence = licence,
                    User = user,
                    UserNotes = ""
                };
                _sessionService.AddSession(session);

                licence.CurrentSession = session;
                licence.BookedByUserId = null;
                licence.BookedUntil= null;
                _licenceRepository.UpdateLicence(licence);

                user.BookedLicenceId = null;
                _userService.UpdateUser(user.Id, user);

                //send licence taken notification 
                await _notification.Clients.All.SendMessage(new Notification
                {
                    CreatedAt = DateTime.Now,
                    Title = "Licence Taken",
                    Message = user.Name + " has taken licence number " + licence.Id,
                    UserId =user.Id,
                });

                //remove the user from the queue if he was waiting
                if (dto.FromQueue==true) _queueService.RemoveUser(user.Id);

                return licence;
            }
            catch (Exception ex)
            {
                //delete the created session
                var session = licence.CurrentSession;
                _sessionService.DeleteSession(session.Id);
                
                //liberate the licence
                licence.BookedByUserId = null;
                _licenceRepository.UpdateLicence(licence);

                user.BookedLicenceId = null;
                _userService.UpdateUser(user.Id, user);

                //send notification
                await _notification.Clients.All.SendMessage(new Notification
                {
                    Title = "Licence Request Failed",
                    Message = "Request for 'licence number " + licence.Id + " failed",
                });
                throw new Exception($"An error occurred: {ex.Message}");
            }
        }

        public async Task<Session> ReturnLicence(int id, ReturnLicenceDto dto)
        {
            //check if licence exists and has a currentSession
            var licence = _licenceRepository.GetLicenceById(id);
            if (licence == null || licence.CurrentSession == null)
            {
                return null;
            }

            var currentSession = _sessionService.GetSessionById(licence.CurrentSession.Id);

            try
            {
                //if browser is not closed yet
                if (dto.isBrowserClosed == false)
                {
                    var response = await _httpClient.GetAsync("http://127.0.0.1:5000/close");

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception("Failed to close the browser session");
                    }
                }
                //end session
                currentSession.EndTime = DateTime.Now;
                currentSession.Licence = licence;
                _sessionService.UpdateSession(currentSession);

                // send notification to the first in the queue if its not empty
                var queue = _queueService.GetFirst();
                if (queue != null)
                {
                    await _notification.Clients.Client(queue.User.ConnectionId).SendMessage(new Notification
                    {
                        CreatedAt = DateTime.Now,
                        Title = "First in queue",
                        Message = "Click to take the licence",
                        UserId = queue.User.Id,
                        LicenceId = id
                    });
                    licence.BookedUntil = DateTime.Now.AddMinutes(1);
                    licence.BookedByUserId = queue.User.Id;

                    queue.User.BookedLicenceId = id;
                    _userService.UpdateUser(queue.User.Id, queue.User);
                }
                
                licence.CurrentSession = null;
                _licenceRepository.UpdateLicence(licence);
                
                await _notification.Clients.All.SendMessage(new Notification
                {
                    CreatedAt = DateTime.Now,
                    Title = "Licence Returned",
                    Message = currentSession?.User?.Name + " has returned licence number " + licence.Id,
                    UserId = currentSession?.User?.Id,
                });

                return currentSession;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}");
            }
        }
        
        public async Task<Licence> CancelRequestLicence(int id)
        {
            //check if licence exists
            var licence = _licenceRepository.GetLicenceById(id);
            if (licence == null || licence.BookedByUserId==null)
            {
                return null;
            }
            var userId = (int) licence.BookedByUserId;
            var user = _userService.GetUserById(userId);

            user.BookedLicenceId = null;
            _userService.UpdateUser(user.Id, user);

            //remove from queue:
            _queueService.RemoveUser(userId);

            // send notification to the first in the queue if its not empty
            var queue = _queueService.GetFirst();
            if (queue != null)
            {
                await _notification.Clients.Client(queue.User.ConnectionId).SendMessage(new Notification
                {
                    CreatedAt = DateTime.Now,
                    Title = "First in queue",
                    Message = "You can now take a licence before the timer ends!",
                    UserId = queue.User.Id,
                    LicenceId = id
                });
                licence.BookedByUserId = queue.User.Id;
                licence.BookedUntil = DateTime.Now.AddMinutes(1);
                _licenceRepository.UpdateLicence(licence);

                queue.User.BookedLicenceId = id;
                _userService.UpdateUser(queue.User.Id, queue.User);
            }
            else
            {
                licence.BookedByUserId = null;
                _licenceRepository.UpdateLicence(licence);
                await _notification.Clients.All.SendMessage(new Notification
                {
                    CreatedAt = DateTime.Now,
                    Title = "Licence Available",
                    Message = "Licence no longer booked",
                    UserId = userId
                });
            }
            return licence;
        }
    }
}
