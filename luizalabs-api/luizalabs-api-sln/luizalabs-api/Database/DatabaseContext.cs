using Microsoft.EntityFrameworkCore;

using LuizaLabsApi.Models;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions options)
        : base(options) { }

    public DbSet<User> User { get; set; }
    public DbSet<UserVerification> UserVerification { get; set; }
}
