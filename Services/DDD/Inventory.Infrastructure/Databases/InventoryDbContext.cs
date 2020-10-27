using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Infrastructure.Databases.EntityConfiguration;
using Inventory.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Databases
{
    public class InventoryDbContext : DbContext
    {
        public InventoryDbContext() : base() { }

        public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options) { }

        // COnfiguration tables
        public DbSet<Domain.Models.OperatingSystem> OperatingSystems { get; set; }
        public DbSet<Domain.Models.Environment> Environments { get; set; }


        // Inventory variables
        public DbSet<Server> Servers { get; set; }
        public DbSet<Group>  Groups { get; set; }
        //public DbSet<ServerGroup> ServerGroups { get; set; }
        //public DbSet<ServerEnvironment> ServerEnvironments { get; set; }


        //public DbSet<BaseDisk>    ServerDisks { get; set; }
        public DbSet<WindowsDisk> WindowsDisks { get; set; }
        public DbSet<LinuxDisk>   LinuxDisks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.ApplyConfiguration(new ServerEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new GroupEntityTypeConfiguration());

            //Many to Many server groups relation
            modelBuilder.Entity<ServerGroup>().HasKey(sg => new { sg.ServerId, sg.GroupId });
            modelBuilder.Entity<ServerGroup>()
                        .HasOne(sg => sg.Server)
                        .WithMany(s => s.ServerGroups)
                        .HasForeignKey(sg => sg.ServerId);

            modelBuilder.Entity<ServerGroup>()
                        .HasOne(sg => sg.Group)
                        .WithMany(g => g.ServerGroups)
                        .HasForeignKey(sg => sg.GroupId);

            //Many to Many server Environments relation
            modelBuilder.Entity<ServerEnvironment>().HasKey(se => new { se.ServerId, se.EnvironmentId });
            modelBuilder.Entity<ServerEnvironment>()
                        .HasOne(se => se.Server)
                        .WithMany(e => e.ServerEnvironments)
                        .HasForeignKey(se => se.ServerId);

            modelBuilder.Entity<ServerEnvironment>()
                        .HasOne(se => se.Environment)
                        .WithMany(e => e.ServerEnvironments)
                        .HasForeignKey(se => se.EnvironmentId);

            modelBuilder.Entity<WindowsDisk>().HasBaseType<BaseDisk>();
            modelBuilder.Entity<LinuxDisk>().HasBaseType<BaseDisk>();

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("host=localhost;port=55432;database=blogdb;username=bloguser;password=bloguser");
            }
        }

    }
}
