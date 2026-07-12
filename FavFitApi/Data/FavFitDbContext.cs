using Microsoft.EntityFrameworkCore;
using FavFitApi.Models;

namespace FavFitApi.Data;

public partial class FavFitdbContext : DbContext
{
    public FavFitdbContext()
    {
    }

    public FavFitdbContext(DbContextOptions<FavFitdbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users {get; set;}
    public DbSet<Activity> Activities {get; set;}
    public DbSet<RefreshToken> RefreshTokens {get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
        .Property(o => o.CreatedAt)
        .HasDefaultValueSql("timezone('utc', now())");

        modelBuilder.Entity<Activity>()
        .Property(o => o.CreatedAt)
        .HasDefaultValueSql("timezone('utc', now())");
    }
}
