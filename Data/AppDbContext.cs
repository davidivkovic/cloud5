using Microsoft.EntityFrameworkCore;

using Cloud5.Model;

namespace Cloud5.Data;

public class AppDbContext : DbContext
{
    public DbSet<Player> Players { get; set; } = default!;
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Player>()
            .HasKey(p => p.Name);
        modelBuilder.Entity<Player>()
            .Property(p => p.Name)
            .HasMaxLength(100);
    }
}