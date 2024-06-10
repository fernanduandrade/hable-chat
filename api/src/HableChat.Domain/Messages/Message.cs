using System.ComponentModel.DataAnnotations.Schema;
using HableChat.Domain.Channels;
using HableChat.Domain.Users;
using SharedKernel;

namespace HableChat.Domain.Messages;

[Table("messages", Schema = "public")]
public class Message : Entity, IAggregateRoot
{
    [Column("content")]
    public required string Content {get; set;}

    public virtual User User {get; set;}

    [Column("user_id")]
    public required string UserId {get; set;}

    public virtual Channel Channel {get; set;}

    [Column("channel_id")]
    public long ChannelId {get; set;}


    public static Message Create(string userId, long channelId, string content)
    {
        Message message = new ()
        {
            UserId = userId,
            Content = content,
            ChannelId = channelId
        };

        return message;
    }

}
