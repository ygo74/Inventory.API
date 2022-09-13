using Inventory.Configuration.Domain.Models;
using Inventory.Configuration.Infrastructure.EntityConfiguration;
using Inventory.Domain.Base.Repository;
using Inventory.Infrastructure.Base.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Configuration.Infrastructure
{
    public class ConfigurationDbContext : ApplicationDbContext, IUnitOfWork
    {
        public ConfigurationDbContext(DbContextOptions<ConfigurationDbContext> options) : base(options) { }

        // Configuration tables
        public DbSet<Datacenter> Datacenters { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DatacenterEntityTypeConfiguration());
        }

    }
}
