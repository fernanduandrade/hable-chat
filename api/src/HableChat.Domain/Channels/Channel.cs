using System.ComponentModel.DataAnnotations.Schema;
using HableChat.Domain.Guilds;
using SharedKernel;

namespace HableChat.Domain.Channels;

[Table("channels", Schema = "public")]
public class Channel : Entity, IAggregateRoot
{
    [Column("name")]
    public required string Name {get; set;}

    public virtual Guild Guild {get; set;}

    [Column("guild_id")]
    public long GuildId {get; set;} 

    public static Channel Create(string name, long guildId)
    {
        Channel channel = new()
        {
            Name = name,
            GuildId = guildId
        };

        return channel;
    }
}
