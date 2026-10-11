using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Messanger.Server.Data;
using Messanger.Shared.Module;

namespace Messanger.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessagesController : ControllerBase
{
    private readonly AppDbContext _db;

    public MessagesController(AppDbContext db)
    {
        _db = db;
    }

    // 1. Получить сообщения для конкретного чата
    [HttpGet("chat/{chatId}")]
    public async Task<IActionResult> GetMessagesByChat(int chatId)
    {
        var messages = await _db.Messages
            .Where(m => m.ChatId == chatId)
            .OrderBy(m => m.CreatedAt)
            .Select(m => new Message
            {
                Id = m.Id,
                ChatId = m.ChatId,
                SenderId = m.SenderId,
                // Автоматически подтягиваем имя отправителя из таблицы Users!
                SenderName = m.SenderName != null ? m.Sender.Name : "Неизвестный",
                Text = m.Text,
                Content = m.Content, 
                TypeMessage = m.TypeMessage, 
                CreatedAt = m.CreatedAt
            })
            .ToListAsync();

        return Ok(messages);
    }

    [HttpPost]
    public async Task<IActionResult> SendMessage([FromBody] Message request)
    {
        if (request.ChatId <= 0 || request.SenderId <= 0)
            return BadRequest("Неверный ChatId или SenderId.");

        var message = new Message
        {
            ChatId = request.ChatId,
            SenderId = request.SenderId,
            Text = request.Text,
            Content = request.Content,
            TypeMessage = request.TypeMessage,
            CreatedAt = DateTime.UtcNow
        };

        _db.Messages.Add(message);
        await _db.SaveChangesAsync();

        var sender = await _db.Users.FindAsync(message.SenderId);
        message.SenderName = sender?.Name ?? "Пользователь";

        return Ok(message);
    }
}