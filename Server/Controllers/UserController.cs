using Messanger.Server.Data;
using Messanger.Shared.Module;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Messanger.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // === 1. РЕГИСТРАЦИЯ ===
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(RegisterRequest request)
        {
           
            if (request.UserName.Length > 20)
                return BadRequest("Никнейм не должен превышать 20 символов.");

            
            bool phoneExists = await _context.Users.AnyAsync(u => u.PhoneNumber == request.PhoneNumber);
            if (phoneExists)
                return BadRequest("Пользователь с таким номером телефона уже зарегистрирован.");

            
            bool usernameExists = await _context.Users.AnyAsync(u => u.UserName == request.UserName);
            if (usernameExists)
                return BadRequest("Этот никнейм уже занят. Придумайте другой.");

            // Если всё ок, создаем нового пользователя
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

        // === 2. ПОИСК ПОЛЬЗОВАТЕЛЯ (Проверка при вводе) ===
        [HttpGet("search")]
        public async Task<ActionResult<List<User>>> SearchUsers(string query, int currentUserId)
        {
            if (string.IsNullOrWhiteSpace(query))
                return Ok(new List<User>());
            var foundUsers = await _context.Users.Where(u => u.UserName.Contains(query) && u.Id != currentUserId).Take(10).ToListAsync();

            return Ok(foundUsers);
        }

        // === 3. ВХОД (Для проверки, существует ли юзер при авторизации) ===
        [HttpGet("login")]
        public async Task<ActionResult<User>> Login(string phoneNumber)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);

            if (user == null)
                return NotFound("Пользователь не найден. Пройдите регистрацию.");

            return Ok(user);
        }
    }
}