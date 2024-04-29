using Microsoft.AspNetCore.SignalR;

namespace Task7.Hubs
{
    public class VideoChatHub : Hub
    {
        private static Dictionary<string, string> _users = new Dictionary<string, string>();

        public async Task JoinRoom(string roomId, string userId)
        {
            _users[Context.ConnectionId] = userId;
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            await Clients.Group(roomId).SendAsync("JoinRoom", userId);
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            Clients.All.SendAsync("LeaveRoom", _users[Context.ConnectionId]);
            _users.Remove(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
