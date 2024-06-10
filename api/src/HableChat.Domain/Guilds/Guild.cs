using System.ComponentModel.DataAnnotations.Schema;
using HableChat.Domain.Users;
using SharedKernel;

namespace HableChat.Domain.Guilds;

[Table("guilds", Schema = "public")]
public class Guild : Entity, IAggregateRoot
{
    [Column("name")]
    public string Name {get; set;}
    public virtual User User {get; set;}

    [Column("user_id")]
    public string UserId {get; set;}

    public static Guild Create(string userId, string name)
    {
        Guild guild = new()
        {
            Name = name,
            UserId = userId,
        };

        return guild;
    }
}
