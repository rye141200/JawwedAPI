using JawwedAPI.Core.Domain.Entities;
using JawwedAPI.Core.ServiceInterfaces.SeedInterfaces;
using JawwedAPI.Infrastructure.DataSeeding.JsonBindedClasses;
using Microsoft.EntityFrameworkCore;

namespace JawwedAPI.Infrastructure.DbContexts;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Verse> Verses { get; set; }
    public DbSet<Chapter> Chapters { get; set; }
    public DbSet<Line> Lines { get; set; }
    public DbSet<Bookmark> Bookmarks { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<Mofasir> Mofasirs { get; set; }
    public DbSet<Tafsir> Tafsirs { get; set; }
    public DbSet<Zekr> Azkar { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Verse>()
            .Property(v => v.VerseID) // Replace 'Id' with the actual primary key property name for Verse
            .ValueGeneratedNever(); // Disables identity for this primary key

        modelBuilder
            .Entity<Chapter>()
            .Property(c => c.ChapterID) // Replace 'Id' with the actual primary key property name for Chapter
            .ValueGeneratedNever(); // Disables identity for this primary key

        modelBuilder
            .Entity<Zekr>()
            .Property(c => c.ZekrID) // Replace 'Id' with the actual primary key property name for Zekr
            .ValueGeneratedNever(); // Disables identity for this primary key

        modelBuilder.Entity<ApplicationUser>().HasIndex(user => user.Email).IsUnique();

        modelBuilder
            .Entity<ApplicationUser>()
            .HasData(
                [
                    new() { Email = "thecityhunterhd@gmail.com", UserName = "Ahmad Mahfouz" },
                    new()
                    {
                        Email = "ahmad.mhfz1412@gmail.com",
                        UserName = "Ahmad Mahfouz",
                        UserRole = ApplicationRoles.Premium,
                    },
                ]
            );
        /* modelBuilder.Entity<Verse>()
        .HasOne<Line>()
        .WithMany(l => l.Verses)
        .HasForeignKey("LineID")
        .OnDelete(DeleteBehavior.ClientSetNull); */
    }
}
