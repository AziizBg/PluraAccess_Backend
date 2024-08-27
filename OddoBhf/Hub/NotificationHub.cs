using Microsoft.AspNetCore.SignalR;
using OddoBhf.Interfaces;
using OddoBhf.Migrations;

namespace OddoBhf.Hub
{
    public class NotificationHub:Hub<INotificationHub>
    {
        private readonly IUserService _userService;
        private readonly IQueueService _queueService;
        private readonly ILicenceService _licenceService;
        public NotificationHub(IUserService userService, IQueueService queueService, ILicenceService licenceService)
        {
            _userService = userService;
            _queueService = queueService;
            _licenceService = licenceService;
        }
        public override async Task OnConnectedAsync()
        {
            // Retrieve userId from the query string
            var userIdString = Context.GetHttpContext().Request.Query["userId"];

            // Try to parse the userId to an integer
            if (int.TryParse(userIdString, out int userId))
            {
                // Fetch the user using the parsed userId
                var user = _userService.GetUserById(userId);

                if (user != null)
                {
                    user.ConnectionId = Context.ConnectionId;
                    _userService.UpdateUser(userId, user);
                }
                else
                {
                    // Handle the case where the user is not found
                    Console.WriteLine($"User with ID {userId} not found.");
                }
            }
            else
            {
                // Handle the case where userId could not be parsed to an integer
                Console.WriteLine("Invalid userId provided.");
            }

            await base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var user = _userService.GetUserByConnectionId(Context.ConnectionId);
            if (user != null)
            {
                user.ConnectionId = null;
                user.BookedLicenceId = null;
                _userService.UpdateUser(user.Id, user);

                var licence = _licenceService.GetLicenceBookedByUserId(user.Id);
                if (licence != null)
                {
                    var nextUser = _queueService.GetFirst();
                    if (nextUser != null)
                    {
                        licence.BookedByUserId = nextUser.User.Id;
                        licence.BookedUntil = DateTime.Now.AddMinutes(1);
                        nextUser.User.BookedLicenceId = licence.Id;
                        _userService.UpdateUser(nextUser.User.Id, nextUser.User);
                    }
                    else
                    {
                        licence.BookedByUserId = null;
                    }
                    _licenceService.UpdateLicence(licence);
                }
            }
            
            return base.OnDisconnectedAsync(exception);
        }
    }
}
