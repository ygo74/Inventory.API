using Inventory.Domain.Models;
using Inventory.Domain.Models.ManagedEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Infrastructure.Databases.EntityConfiguration
{
    public class ServerEntityTypeConfiguration : IEntityTypeConfiguration<Server>
    {
        public void Configure(EntityTypeBuilder<Server> builder)
        {

            builder.UseXminAsConcurrencyToken();

            builder.ToTable("Server");
            builder.HasKey(server => server.ServerId);
            builder.Property(s => s.ServerId).UseHiLo();

            builder.HasIndex(server => server.HostName).IsUnique();

            builder.Property(s => s.CPU).IsRequired();
            builder.Property(s => s.RAM).IsRequired();
            builder.Property(s => s.Subnet).IsRequired();

            //Os Familly definition
            builder.HasOne(s => s.OperatingSystem)
                   .WithMany(os => os.Servers)
                   .HasForeignKey(s => s.OperatingSystemId);


            // Location
            builder.HasOne(s => s.Location)
                .WithMany(l => l.Servers)
                .HasForeignKey(s => s.LocationId);

            // Ignore server Properties
            //Variables
            //builder.Ignore(s => s.Variables);
            //builder.Property<String>("_variables");
            //builder.OwnsMany(s => s.Variables);

        }
    }
}
