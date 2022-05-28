using Microsoft.EntityFrameworkCore;
using GestUser.Models;

namespace GestUser.Services
{
  public class LoginDbContext : DbContext
  {
    public LoginDbContext(DbContextOptions<LoginDbContext> options)
        : base(options)
    {
      //Database.Migrate();
    }

    public virtual DbSet<Users> Users { get; set; }
    //public virtual DbSet<Profili> Profili { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Users>()
          .HasKey(a => new { a.Id });

      /*
      modelBuilder.Entity<Profili>()
          .Property(a => a.Id)
          .ValueGeneratedOnAdd();

      modelBuilder.Entity<Profili>()
          .HasOne<Utenti>(s => s.Utente)
          .WithMany(g => g.Profili)
          .HasForeignKey(g => g.CodFidelity);
      */


    }
  }
}
