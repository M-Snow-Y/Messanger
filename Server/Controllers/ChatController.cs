using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Messanger.Shared.Module;
using Messanger.Server.Data;
using System.Runtime.InteropServices.Marshalling;
using System.Data.Common;
using System.Reflection.Metadata;

namespace Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly AppDbContext _context;
    public ChatController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<Chat>>> GetUserChats(int userId)
    {
        var chats = await _context.Chats.ToListAsync();
        return Ok(chats);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Chat>> GetChat(int id)
    {
        var chat = await _context.Chats.FindAsync(id);
        if (chat == null)
        {
            return NotFound("Чат не найден");
        }
        return Ok(chat);
    }

    [HttpPost("create")]
    public async Task<ActionResult<Chat>> CreateChat([FromBody] Chat chat)
    {
        if (string.IsNullOrWhiteSpace(chat.Name))
        {
            return BadRequest("Название чата не может быть пустым");
        }

        _context.Chats.Add(chat);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetChat), new { id = chat.Id }, chat);
    }

    
    [HttpPost("private")]

    public async Task<ActionResult<Chat>> GetOrCreatePrivateChat( [FromBody] PrivateChatRequest request )
    {
        if(request.CurrentUserId == request.TargetUserId) return BadRequest("Ты не можешь сам себе отправить это");
        
        var currentUser = await _context.Users.FindAsync(request.CurrentUserId);
        var targetUser = await _context.Users.FindAsync(request.TargetUserId);
        
        if (currentUser == null || targetUser == null)
        {
            return NotFound("Один из пользователей не найден в базе");
        }
        
        var existingChat  = await _context.Chats.Include(
            c => c.Users
            ).FirstOrDefaultAsync
            (c => !c.IsGroup &&
                c.Users.Any(
                    u => u.Id == request.CurrentUserId
                ) && 
                c.Users.Any(
                    u => u.Id == request.TargetUserId
                )
            );
        if(existingChat  != null) return Ok(existingChat );

        var newChat = new Chat
        {
          Name = "Диалог",
          IsGroup = false,
          Users = new List<User>{currentUser, targetUser},
          CreatedAt = DateTime.Now  
        };
        _context.Chats.Add(newChat);

        await _context.SaveChangesAsync();
        return Ok(newChat);
    }
    [HttpGet("search")]
    public async Task<ActionResult<Chat>> GetSearchChatWithText()
    {
        
    } 
}