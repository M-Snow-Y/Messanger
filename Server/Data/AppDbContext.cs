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

    modelBuilder.Entity<User>().ToTable("Users");
    modelBuilder.Entity<Chat>().ToTable("Chats");
    modelBuilder.Entity<Message>().ToTable("Messages");

    modelBuilder.Entity<Chat>()
        .HasMany(c => c.Users)
        .WithMany(u => u.Chats);
}
    }
}