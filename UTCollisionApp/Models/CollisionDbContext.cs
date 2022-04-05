using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UTCollisionApp.Models
{
    public class CollisionDbContext : DbContext
    {
        public CollisionDbContext(DbContextOptions<CollisionDbContext> options) : base(options)
        {
        }

        public DbSet<Crash> Crashes { get; set; }
        public DbSet<Factor> Factors { get; set; }
        public DbSet<Location> Locations { get; set; }
    }
}
