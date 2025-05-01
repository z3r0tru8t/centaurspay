using Microsoft.EntityFrameworkCore;
using SecureApiDemo.Models;

namespace SecureApiDemo.Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) {}

    public DbSet<User> Users { get; set; }

    // Optional: seeding data
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Username = "admin", Password = "1234", Role = "Admin" },
            new User { Id = 2, Username = "user", Password = "1234", Role = "User" }
        );
    }
}
