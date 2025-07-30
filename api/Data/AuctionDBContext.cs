    using System.Collections.Generic;
    using api.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    namespace api.Data;

    public class AuctionDbContext : IdentityDbContext<IdentityUser>
    {
        public AuctionDbContext(DbContextOptions<AuctionDbContext> options) : base(options) { }


        public DbSet<Event> Events { get; set; } = null!;
        public DbSet<Artist> Artists { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderItem> OrderItems { get; set; } = null!;
        public DbSet<Review> Reviews { get; set; } = null!;
        public DbSet<EventArtist> EventArtists { get; set; } = null!;
        public DbSet<api.Models.Category> Category { get; set; } = default!;

    }