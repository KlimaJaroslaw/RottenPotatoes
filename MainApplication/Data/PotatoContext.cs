using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RottenPotatoes.Models;

namespace Data
{
    public class PotatoContext : DbContext
    {
        public PotatoContext (DbContextOptions<PotatoContext> options)
            : base(options)
        {
        }

        public DbSet<Movie> Movie { get; set; } = default!;

        public DbSet<Watchlist> Watchlist { get; set; } = default!;

        public DbSet<User> Users { get; set; } = default!;
        public DbSet<Permission> Permissions { get; set; } = default!;
        public DbSet<Review> Reviews { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().HasIndex(u => u.Login_Hash).IsUnique();
            modelBuilder.Entity<Permission>().HasIndex(p=>p.Description).IsUnique();
        }
    }
}
