using HableChat.API.Controllers.Base;
using HableChat.Application.Channels.Create;
using HableChat.Application.Channels.GetByGuildId;
using HableChat.Application.Guilds.Create;
using HableChat.Application.Guilds.Delete;
using HableChat.Application.Guilds.Get;
using HableChat.Application.Guilds.GetById;
using Microsoft.AspNetCore.Mvc;

namespace HableChat.API.Controllers;

public class GuildsController : BaseController
{

    [HttpGet("{id}")]
    public async Task<IActionResult> ById([FromRoute] GetGuildByIdQuery query)
    {
        var result = await Mediator.Send(query);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> All()
    {
        var query = new GetGuildsQuery();
        var result = await Mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateGuildCommand command)
    {
        var result = await Mediator.Send(command);

        return new ObjectResult(result) { StatusCode = StatusCodes.Status201Created };
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] DeleteGuildCommand command)
    {
        await Mediator.Send(command);
        return NoContent();
    }

    [HttpPost("{id}/channels")]
    public async Task<IActionResult> CreateChannel([FromRoute] long id, [FromBody] CreateChannelDto request)
    {
        var command = new CreateChannelCommand(request.Name, id);
        var result = await Mediator.Send(command);

        return new ObjectResult(result) { StatusCode = StatusCodes.Status201Created };
    }

    [HttpGet("{id}/channels")]
    public async Task<IActionResult> GetChannelsByGuildId([FromRoute] GetChannelsByGuildIdQuery query)
    {
        var result = await Mediator.Send(query); 
        return Ok(result);
    }
    
}
