using Microsoft.AspNetCore.SignalR;
using OddoBhf.Interfaces;

namespace OddoBhf.Hub
{
    public class NotificationHub:Hub<INotificationHub>
    {
        private readonly IUserService _userService;
        public NotificationHub(IUserService userService)
        {
            _userService = userService;
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
    }
}
