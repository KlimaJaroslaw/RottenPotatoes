using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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
    }
}
