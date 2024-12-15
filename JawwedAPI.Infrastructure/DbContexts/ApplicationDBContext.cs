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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Verse>()
            .Property(v => v.VerseID) // Replace 'Id' with the actual primary key property name for Verse
            .ValueGeneratedNever(); // Disables identity for this primary key

        modelBuilder.Entity<Chapter>()
            .Property(c => c.ChapterID) // Replace 'Id' with the actual primary key property name for Chapter
            .ValueGeneratedNever(); // Disables identity for this primary key
    }
}
