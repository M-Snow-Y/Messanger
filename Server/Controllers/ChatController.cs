using Messanger.Server.Data;
using Messanger.Shared.Module;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class ChatsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ChatsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserChats(int userId)
    {
        var chats = await _context.Chats.ToListAsync();
        return Ok(chats);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateChat([FromBody] Chat request)
    {
        var newChat = new Chat
        {
            Name = request.Name,
            CreatedAt = DateTime.Now
        };

        _context.Chats.Add(newChat);
        await _context.SaveChangesAsync();

        return Ok(newChat);
    }
}