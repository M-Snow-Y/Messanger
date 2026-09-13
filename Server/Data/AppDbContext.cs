using Messanger.Shared.Module;
using Microsoft.EntityFrameworkCore;

namespace Messanger.Server.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Chat> Chats { get; set; } = null!;
        public DbSet<Message> Messages { get; set; } = null!;

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    // Явно указываем имена таблиц для SQLite
    modelBuilder.Entity<User>().ToTable("Users");
    modelBuilder.Entity<Chat>().ToTable("Chats");
    modelBuilder.Entity<Message>().ToTable("Messages");

    // Настраиваем связь "Многие ко многим": У чата много пользователей, у пользователя много чатов
    modelBuilder.Entity<Chat>()
        .HasMany(c => c.Users)
        .WithMany(u => u.Chats);
}
    }
}