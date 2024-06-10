using System.Security.Claims;
using HableChat.Application.Common.Interfaces;

namespace HableChat.API.Services;

public class UserManager(IHttpContextAccessor contextAccessor) : IUserManager
{
    public string GetId()
    {
        string userId = contextAccessor.HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
        return userId;
    }

    public string GetEmail()
    {
        string email = contextAccessor.HttpContext.User.Claims.First(x => x.Type == ClaimTypes.Email).Value;
        return email;
    }
}
