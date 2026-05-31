using Microsoft.AspNetCore.Mvc;
using Messanger.Shared.Module;
using Shared.Interfaces;

namespace Massanger.Server.Controllers;

[ApiController]
[Route("api/[controller]")]


public class MessagesController : ControllerBase
{
    private readonly IMessanger _messangesServices;
    public MessagesController(IMessanger messanger)
    {
        _messangesServices = messanger;
    }
    [HttpGet("group/{userId}")]
    public async Task<ActionResult<List<Chat>>> GetGroup(int userId)
    {
        var _getGroup = await _messangesServices.GetAllMessageGroupAsync(userId);
        return Ok(_getGroup);
    }
    [HttpGet("people/{userId}")]
    public async Task<ActionResult<List<Chat>>> GetPersonalChat(int userId) 
    {
        var _getPersonalChat = await _messangesServices.GetAllMessagePeopleAsync(userId);
        return Ok(_getPersonalChat);
    }
    [HttpGet("chat/{chatId}")]
    public async Task<ActionResult<List<Message>>> GetChatMessage(int chatId)
    {
        var _message = await _messangesServices.GetChatMessageAsync(chatId);
        return Ok(_message);
    }

    [HttpGet("send")]
    public async Task<ActionResult<Message>> SendMessage([FromBody] Message request)
    {
        var _sendMessage = await _messangesServices.SendMessageAsync(request);
        return Ok(_sendMessage);
    }
}