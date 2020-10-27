using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Infrastructure.EntityConfiguration;
using Inventory.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

namespace Inventory.Infrastructure
{
    public class InventoryDbContext : DbContext
    {
        public InventoryDbContext() : base() { }

        public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options) { }

        public DbSet<Domain.Models.OperatingSystem> OperatingSystems { get; set; }

        public DbSet<Server> Servers { get; set; }
        public DbSet<Group>  Groups { get; set; }
        public DbSet<ServerGroup> ServerGroups { get; set; }

        //public DbSet<Variable> Variables { get; set; }
        //public DbSet<NumericVariable> NumericVariables { get; set; }
        //public DbSet<StringVariable> StringVariables { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ServerEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new GroupEntityTypeConfiguration());

            modelBuilder.Entity<ServerGroup>().HasKey(sg => new { sg.ServerId, sg.GroupId });
            modelBuilder.Entity<ServerGroup>()
                        .HasOne(sg => sg.Server)
                        .WithMany(s => s.ServerGroups)
                        .HasForeignKey(sg => sg.ServerId);

            modelBuilder.Entity<ServerGroup>()
                        .HasOne(sg => sg.Group)
                        .WithMany(g => g.ServerGroups)
                        .HasForeignKey(sg => sg.GroupId);


            //modelBuilder.Entity<Variable>()
            //    .Ignore(v => v.RawValue);

        }

        //        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //        {
        //#if DEBUG
        //            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "MyDb.db" };
        //            var connectionString = connectionStringBuilder.ToString();
        //            var connection = new SqliteConnection(connectionString);

        //            optionsBuilder.UseSqlite(connection);
        //#endif
        //        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("host=postgres_image;port=32774;database=blogdb;username=bloguser;password=bloguser");
            }
        }
    }
}
