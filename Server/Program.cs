using Microsoft.EntityFrameworkCore;
using Messanger.Server.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=messanger.db"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();

app.UseStaticFiles();

// Создаем папку uploads в корне проекта, если её нет
var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
if (!Directory.Exists(uploadsPath))
{
    Directory.CreateDirectory(uploadsPath);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(uploadsPath),
    RequestPath = "/uploads"
});


app.MapControllers();
// АВТОМАТИЧЕСКОЕ СОЗДАНИЕ ТАБЛИЦ В БАЗЕ ДАННЫХ
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    //db.Database.EnsureDeleted(); // 1. Удаляем пустой битый файл базы
    db.Database.EnsureCreated(); // 2. Создаём заново со всеми таблицами Users, Chats, Messages!
}
app.Run("http://0.0.0.0:5050");