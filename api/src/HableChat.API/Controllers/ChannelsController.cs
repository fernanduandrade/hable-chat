using HableChat.API.Controllers.Base;
using HableChat.Application.Channels.Create;
using HableChat.Application.Channels.GetById;
using HableChat.Application.Common.Interfaces;
using HableChat.Application.Messages.Create;
using HableChat.Application.Messages.Delete;
using HableChat.Application.Messages.GetChannelMesssages;
using HableChat.Infra.Data.HubConnections;
using HableChat.Infra.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace HableChat.API.Controllers;

public class ChannelsController : BaseController
{
    private readonly IHubContext<ChannelHub> _hubContext;
    private readonly SharedChannel _sharedChannel;

    public ChannelsController(IHubContext<ChannelHub> hubContext, SharedChannel sharedChannel)
        => (_hubContext, _sharedChannel) = (hubContext, sharedChannel);

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] GetChannelByIdQuery query)
    {
        var result = await Mediator.Send(query); 
        return Ok(result);
    }

    [HttpPost("{id}/messages")]
    public async Task<IActionResult> CreateMessage([FromRoute] long id,
        [FromBody] CreateMessageDto dto,
        [FromServices] IUserManager userManager,
        [FromServices] IHttpContextAccessor contextAccessor)
    {
        var userId = userManager.GetId();
        var command = new CreateMessageCommand(dto.Content, userId, id);
        var userEmail = userManager.GetEmail();

        var result = await Mediator.Send(command); 
        return new ObjectResult(result) { StatusCode = StatusCodes.Status201Created };
    }

    [HttpDelete("{id}/messages/{messageId}")]
    public async Task<IActionResult> DeleteMessage([FromRoute] long id, [FromRoute] long messageId)
    {
        var command = new DeleteMessageCommand(id, messageId);

        await Mediator.Send(command); 
        return NoContent();
    }

    [HttpGet("{id}/messages")]
    public async Task<IActionResult> ChannelMessages([FromRoute] long id, [FromQuery] int limit = 99999)
    {
        
        var query = new GetChannelMessagesQuery(id, limit);

        var result = await Mediator.Send(query); 
        return Ok(result);
    }

    
}
