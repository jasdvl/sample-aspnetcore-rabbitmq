namespace PubLib.FrontDesk.Server.Messaging;

using Microsoft.AspNetCore.SignalR;

public class MessageHub : Hub
{
    public async Task JoinGroup(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }
}
