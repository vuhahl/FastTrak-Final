using FastTrak.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FastTrak.Api.Data;

public class FastTrakDbContext : DbContext
{
    public FastTrakDbContext(DbContextOptions<FastTrakDbContext> options) : base(options)
    {
    }

    public DbSet<Restaurant> Restaurants => Set<Restaurant>();
    public DbSet<MenuItem> MenuItems => Set<MenuItem>();
    public DbSet<CustomOption> CustomOptions => Set<CustomOption>();
    public DbSet<MenuItemOption> MenuItemOptions => Set<MenuItemOption>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Restaurant -> MenuItems (one-to-many)
        modelBuilder.Entity<MenuItem>()
            .HasOne(mi => mi.Restaurant)
            .WithMany(r => r.MenuItems)
            .HasForeignKey(mi => mi.RestaurantId);

        // MenuItem <-> CustomOption (many-to-many via MenuItemOption)
        modelBuilder.Entity<MenuItemOption>()
            .HasOne(mio => mio.MenuItem)
            .WithMany(mi => mi.MenuItemOptions)
            .HasForeignKey(mio => mio.MenuItemId);

        modelBuilder.Entity<MenuItemOption>()
            .HasOne(mio => mio.CustomOption)
            .WithMany(co => co.MenuItemOptions)
            .HasForeignKey(mio => mio.CustomOptionId);

        // Indexes for faster lookups
        modelBuilder.Entity<MenuItem>()
            .HasIndex(mi => mi.RestaurantId);

        modelBuilder.Entity<MenuItem>()
            .HasIndex(mi => mi.Category);

        modelBuilder.Entity<CustomOption>()
            .HasIndex(co => co.Category);

        modelBuilder.Entity<MenuItemOption>()
            .HasIndex(mio => mio.MenuItemId);

        modelBuilder.Entity<MenuItemOption>()
            .HasIndex(mio => mio.CustomOptionId);
    }
}
