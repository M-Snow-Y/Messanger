using Microsoft.AspNetCore.Mvc;

namespace Messanger.Server.Controllers;

[Route("api/[controller]")]
[ApiController]

public class UploadController : ControllerBase
{
    private readonly IWebHostEnvironment _env;

    public UploadController(IWebHostEnvironment env)
    {
        _env = env;
    }

    [HttpPost]
    public async Task<IActionResult> Upload([FromForm] IFormFile file)
    {
        if(file == null||file.Length == 0)
            return BadRequest("Файл не найден/не передан");
        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(),"uploads");
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }
        var extension = Path.GetExtension(file.FileName);
        var uniqueFileName = $"{Guid.NewGuid():N}{extension}";
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var relativeUrl = $"/uploads/{uniqueFileName}";
        return Ok(new { url = relativeUrl });
    }
}