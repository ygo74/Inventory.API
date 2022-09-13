using Inventory.Domain.Base.Repository;
using Inventory.Infrastructure.Base.Database;
using Inventory.Servers.Domain.Models;
using Inventory.Servers.Infrastructure.EntityConfiguration;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OperatingSystem = Inventory.Servers.Domain.Models.OperatingSystem;

namespace Inventory.Servers.Infrastructure
{
    public class ServerDbContext : ApplicationDbContext, IUnitOfWork
    {

        //public ServerDbContext() : base() { }

        public ServerDbContext(DbContextOptions<ServerDbContext> options) : base(options) { }

        // Configuration tables
        public DbSet<Server> Servers { get; set; }
        //public DbSet<OperatingSystem> OperatingSystems { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new OperatingSystemTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ServerEntityTypeConfiguration());
        }

    }
}
