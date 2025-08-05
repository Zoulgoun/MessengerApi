using MessengerApi.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    
    public DbSet<Message> Messages { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<LoginViewModel> Logging { get; set; }

    public DbSet<RegistrationViewModel> Registrations { get; set; }
}
