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
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber);
        if (existingUser != null) return Ok(existingUser);

        var newUser = new User
        {
            PhoneNumber = request.PhoneNumber,
            UserName = request.UserName,
            CreatedAt = DateTime.Now
        };

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();
        return Ok(newUser);
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
}