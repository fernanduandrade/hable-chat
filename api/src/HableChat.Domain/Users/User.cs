using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace HableChat.Domain.Users;

[Table("AspNetUsers", Schema = "identity")]
public class User : IdentityUser
{
    public string? Name {get; set;}
    public string? LastName {get; set;}
}
