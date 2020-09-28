﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.API.Infrastructure.EntityConfiguration;
using Inventory.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory.API.Infrastructure
{
    public class InventoryContext : DbContext
    {
        public InventoryContext() : base() { }

        public InventoryContext(DbContextOptions<InventoryContext> options) : base(options) { }

        public DbSet<Server> Servers { get; set; }
        public DbSet<Group>  Groups { get; set; }
        public DbSet<ServerGroup> ServerGroups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ServerEntityTypeConfiguration());

            modelBuilder.Entity<ServerGroup>().HasKey(sg => new { sg.ServerId, sg.GroupId });
        }

        

    }
}
