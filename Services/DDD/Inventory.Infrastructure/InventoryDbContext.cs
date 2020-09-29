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

            modelBuilder.Entity<ServerGroup>().HasKey(sg => new { sg.ServerId, sg.GroupId });
            modelBuilder.Entity<ServerGroup>()
                        .HasOne(sg => sg.Server)
                        .WithMany(s => s.ServerGroups)
                        .HasForeignKey(sg => sg.ServerId);

            modelBuilder.Entity<ServerGroup>()
                        .HasOne(sg => sg.Group)
                        .WithMany(g => g.ServerGroups)
                        .HasForeignKey(sg => sg.GroupId);


        }

    }
}
