
using HableChat.Domain.Users;
using HableChat.Infra.Data.HubConnections;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace HableChat.Infra.Hubs;


public class ChannelHub(SharedChannel sharedChannel, UserManager<User> userNamager) : Hub
{

    public async Task JoinChannel(UserConnection connection)
    {
        await Clients.All.SendAsync("ReceiveMesssage", connection.UserName);
    }

    public async Task JoinSpecificChannel(UserConnection connection)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, connection.ChannelId);
        sharedChannel.connections[Context.ConnectionId] = connection;
        await Clients
            .Group(connection.ChannelId.ToString())
            .SendAsync("JoinSpecificChannel", connection.UserName);
    }
    public async Task SendMessage(string message)
    {
        if(sharedChannel.connections.TryGetValue(Context.ConnectionId, out UserConnection connection))
        {
            var user = await userNamager.FindByIdAsync(connection.UserId!);
            await Clients.Group(connection.ChannelId).SendAsync("RecieveSpecificMessage", connection.UserName, message, user);    
        }
        
    }
}