using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Infrastructure.Databases.EntityConfiguration;
using Inventory.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Inventory.Domain.Repositories.Interfaces;
using System.Data;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Inventory.Infrastructure.Databases
{
    public class InventoryDbContext : DbContext, IUnitOfWork
    {
        public InventoryDbContext() : base() { }

        public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options) { }

        #region Transactions
        private IDbContextTransaction _currentTransaction;
        public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;

        public bool HasActiveTransaction => _currentTransaction != null;
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (_currentTransaction != null) return null;

            _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

            return _currentTransaction;
        }

        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

            try
            {
                await SaveChangesAsync();
                transaction.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        #endregion


        #region IUnitOfWork

        #endregion

        // COnfiguration tables
        public DbSet<Domain.Models.OperatingSystem> OperatingSystems { get; set; }
        public DbSet<Domain.Models.Environment> Environments { get; set; }
        public DbSet<Location> Locations { get; set; }


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
                //optionsBuilder.UseNpgsql("host=localhost;port=55432;database=blogdb;username=bloguser;password=bloguser");
            }
        }

    }

    public class InventoryDbContextDesignFactory : IDesignTimeDbContextFactory<InventoryDbContext>
    {
        public InventoryDbContext CreateDbContext(string[] args)
        {
            // Get environment
            string environment = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");


            // Build config
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Inventory.API"))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            // Get connection string
            var optionsBuilder = new DbContextOptionsBuilder<InventoryDbContext>();
            var connectionString = config.GetConnectionString("InventoryDatabase");
            optionsBuilder.UseNpgsql(connectionString);

            return new InventoryDbContext(optionsBuilder.Options);
        }
    }

}
