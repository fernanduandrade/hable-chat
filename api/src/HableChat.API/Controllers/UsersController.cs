using HableChat.API.Controllers.Base;
using HableChat.Infra.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace HableChat.API.Controllers;

public class UsersController : BaseController
{
    [HttpGet("{id}")]
    public async Task<IActionResult> ById(string id, [FromServices] AppIdentityContext appIdentityContext)
    {
        var user = await appIdentityContext.Users.FirstOrDefaultAsync(x => x.Id == id);
        return Ok(user);
    }
}
