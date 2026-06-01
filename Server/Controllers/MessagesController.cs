using Messanger.Server.Data;
using Messanger.Shared.Module;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class MessagesController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _env;

    public MessagesController(AppDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    [HttpGet("chat/{chatId}")]
    public async Task<IActionResult> GetChatMessages(int chatId)
    {
        var messages = await _context.Messages
            .Where(m => m.ChatId == chatId)
            .OrderBy(m => m.CreatedAt)
            .ToListAsync();

        return Ok(messages);
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody] Message request)
    {
        request.CreatedAt = DateTime.Now;
        _context.Messages.Add(request);
        await _context.SaveChangesAsync();
        return Ok(request);
    }

    [HttpPost("upload-photo")]
    public async Task<IActionResult> UploadPhoto(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Файл не выбран");

        var uploadsFolder = Path.Combine(_env.WebRootPath ?? _env.ContentRootPath, "wwwroot", "uploads");
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var fileUrl = $"/uploads/{uniqueFileName}";
        return Ok(new { url = fileUrl });
    }
}