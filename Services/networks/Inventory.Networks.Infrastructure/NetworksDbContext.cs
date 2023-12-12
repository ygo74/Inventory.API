using Inventory.Common.Domain.Repository;
using Inventory.Common.Infrastructure.Database;
using Inventory.Networks.Domain.Models;
using Inventory.Networks.Infrastructure.EntityConfiguration;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Networks.Infrastructure
{
    public class NetworksDbContext : ApplicationDbContext, IUnitOfWork
    {
        public NetworksDbContext(DbContextOptions<NetworksDbContext> options) : base(options) { }

        // Configuration tables
        public DbSet<NetworkPlugin> Plugins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PluginEntityTypeConfiguration());
        }
    }

}
