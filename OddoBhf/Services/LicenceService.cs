
using Microsoft.AspNetCore.Mvc;
using OddoBhf.Dto;
using OddoBhf.Interfaces;
using OddoBhf.Models;
using System.Text.Json;
using System.Text;
using System.Net.Http;

namespace OddoBhf.Services
{
    public class LicenceService:ILicenceService
    {
        private readonly ILicenceRepository _licenceRepository;
        private readonly ISessionService _sessionService;
        private readonly IUserRepository _userRepository;
        private readonly HttpClient _httpClient;

        public LicenceService(ILicenceRepository licenceRepository, ISessionService sessionService, IUserRepository userRepository, HttpClient httpClient) {
            _licenceRepository = licenceRepository;
            _sessionService = sessionService;
            _userRepository = userRepository;
            _httpClient = httpClient;
        }
        public Licence GetLicenceById(int id)
        {
            return _licenceRepository.GetLicenceById(id);
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
        }
        public void DeleteLicence(int id)
        {
            _licenceRepository.DeleteLicence(id);
        }

        public async Task<Licence> TakeLicence(int id, OpenPluralsightDto dto)
        {
            var licence = _licenceRepository.GetLicenceById(id);
            if (licence == null || licence.CurrentSession != null)
            {
                return null;
            }

            try
            {
                var url = dto.NgorkUrl != null ? dto.NgorkUrl + "/get_cookie" : "http://127.0.0.1:5000/get_cookie";
                var email = "sami.belhadj@oddo-bhf.com";
                var password = "7cB3MP.6y9.Z?c?"; // Remplacez par le vrai mot de passe
                var user = _userRepository.GetUserById(dto.UserId);
                if (user == null)
                {
                    throw new Exception("User not provided");
                }

                var startTime = DateTime.Now;
                var endTime = DateTime.Now.AddHours(2);
                var payload = new { email, password, formattedEndTime = endTime, licenceId = licence.Id };
                var jsonPayload = JsonSerializer.Serialize(payload);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Local server failed to start the browser");
                }

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
                _licenceRepository.UpdateLicence(licence);

                return licence;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}");
            }
        }

        public async Task<Session> ReturnLicence(int id, ReturnLicenceDto dto)
        {
            var licence = _licenceRepository.GetLicenceById(id);
            if (licence == null || licence.CurrentSession == null)
            {
                return null;
            }

            var currentSession = _sessionService.GetSessionById(licence.CurrentSession.Id);

            try
            {
                if (dto.isBrowserClosed==false)
                {
                    var response = await _httpClient.GetAsync("http://127.0.0.1:5000/close");

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception("Failed to close the browser session");
                    }
                }

                currentSession.EndTime = DateTime.Now;
                currentSession.Licence = licence;

                _sessionService.UpdateSession(currentSession);

                licence.CurrentSession = null;
                _licenceRepository.UpdateLicence(licence);

                return currentSession;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}");
            }
        }
    }
}
