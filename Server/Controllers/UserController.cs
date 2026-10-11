using Messanger.Server.Data;
using Messanger.Shared.Module;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("register")]
    public async Task<IActionResult> AuthUser([FromBody] User request)
    {

        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == request.UserName);
        if (existingUser != null) return Ok(existingUser);

        var newUser = new User
        {
            UserName = request.UserName,
            Name = request.Name,
            CreatedAt = DateTime.UtcNow,
            PasswordHash = request.PasswordHash
        };

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();
        return Ok(newUser);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] User request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.PasswordHash))
        {
            return BadRequest("Введите юзер и пароль");
        }
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == request.UserName);
        if (user == null || user.PasswordHash != request.PasswordHash)
        {
            return NotFound("Пользователь с таким номером не найден или не верный пароль");
        }
        return Ok(user);
    }


    [HttpPut("update")]
    public async Task<IActionResult> UpdateProfile([FromBody] User updateRequest)
    {
        var user = await _context.Users.FindAsync(updateRequest.Id);
        if (user == null) return NotFound("Пользователь не найден");

        if (!string.IsNullOrEmpty(updateRequest.UserName))
        {
            user.UserName = updateRequest.UserName;
        }

        await _context.SaveChangesAsync();
        return Ok(user);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchUser([FromQuery] int currentId, string query)
    {
        if(string.IsNullOrWhiteSpace(query)) return Ok(new List<User>());
        string cleanQuery = query.Trim().ToLower();
        var existingUser = await _context.Users.AsNoTracking()
        .Where(u => u.Id != currentId && 
        (u.UserName.ToLower().Contains(cleanQuery) || u.Name.ToLower().Contains(cleanQuery)))
        .Take(15).Select(u => 
        new User{
            Id = u.Id,
            Name = u.Name,
            UserName = u.UserName
        }
        ).ToListAsync();
        return Ok(existingUser);
    }
}