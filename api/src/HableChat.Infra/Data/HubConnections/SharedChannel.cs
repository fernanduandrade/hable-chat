using System.Collections.Concurrent;

namespace HableChat.Infra.Data.HubConnections;

public class SharedChannel
{
    private readonly ConcurrentDictionary<string, UserConnection> _connections = new();
    public ConcurrentDictionary<string, UserConnection> connections => _connections;
}


public class UserConnection
{
    public string? UserName { get; set; }
    public string? UserId {get; set; }
    public string? ChannelId { get; set; }
    public  string? Content { get; set; } 
}