using Microsoft.EntityFrameworkCore;
using RefugiAnimals.Models;

namespace RefugiAnimals.Data;

public class RefugiContext : DbContext
{
    public DbSet<Animal> Animals { get; set; }

    public DbSet<Xip> Xips { get; set; }
    
    public DbSet<Adoptant> Adoptants { get; set; }

    protected override void OnConfiguring(
        DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder
                .UseSqlite("Data Source=RefugiAnimals.db")
                .UseLazyLoadingProxies();
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Animal>()
            .HasOne(a => a.Xip)
            .WithOne(x => x.Animal)
            .HasForeignKey<Animal>(a => a.XipID)
            .IsRequired();

        modelBuilder.Entity<Animal>()
            .Property(a => a.Especie)
            .HasConversion<string>();
        
        modelBuilder.Entity<Xip>()
            .HasIndex(x => x.Codi);
        
        modelBuilder.Entity<Animal>()
            .HasOne(a => a.Adoptant)
            .WithMany(ad => ad.Animals)
            .HasForeignKey(a => a.AdoptantID)
            .IsRequired(false);
    }
    
    
}   