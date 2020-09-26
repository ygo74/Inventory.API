using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Infrastructure.EntityConfiguration;
using Inventory.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure
{
    public class InventoryDbContext : DbContext
    {
        public InventoryDbContext() : base() { }

        public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options) { }

        public DbSet<Server> Servers { get; set; }
        public DbSet<Group>  Groups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ServerEntityTypeConfiguration());

        }

    }
}
