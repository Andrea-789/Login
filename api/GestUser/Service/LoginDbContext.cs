using Microsoft.EntityFrameworkCore;
using GestUser.Models;

namespace GestUser.Services
{
  public class LoginDbContext : DbContext
  {
    public LoginDbContext(DbContextOptions<LoginDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Users> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Users>()
          .HasKey(a => new { a.Id });
      
    }
  }
}
