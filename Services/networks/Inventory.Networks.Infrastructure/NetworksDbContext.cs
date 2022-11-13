using Inventory.Domain.Base.Repository;
using Inventory.Infrastructure.Base.Database;
using Inventory.Networks.Domain.Models;
using Inventory.Networks.Infrastructure.EntityConfiguration;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Networks.Infrastructure
{
    public class NetworksDbContext : ApplicationDbContext, IUnitOfWork
    {
        public NetworksDbContext(DbContextOptions<NetworksDbContext> options) : base(options) { }

        // Configuration tables
        public DbSet<Plugin> Plugins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PluginEntityTypeConfiguration());
        }
    }

}
