using System.Collections.Generic;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Data;

public class AuctionDbContext : IdentityDbContext<IdentityUser>
{
    public AuctionDbContext(DbContextOptions options) : base(options) { }


    public DbSet<Event> Events { get; set; } = null!;
    public DbSet<Artist> Artists { get; set; } = null!;
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<EventArtist> EventArtists { get; set; }

public DbSet<api.Models.Category> Category { get; set; } = default!;

}