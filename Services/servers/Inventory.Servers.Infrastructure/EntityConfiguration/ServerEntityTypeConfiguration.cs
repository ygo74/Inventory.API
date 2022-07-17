using Inventory.Servers.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Servers.Infrastructure.EntityConfiguration
{
    public class ServerEntityTypeConfiguration : IEntityTypeConfiguration<Server>
    {
        public void Configure(EntityTypeBuilder<Server> builder)
        {
            builder.UseXminAsConcurrencyToken();

            builder.ToTable("Server");
            builder.HasKey(server => server.Id);
            builder.Property(s => s.Id).UseHiLo("server_ids");

            builder.HasIndex(server => server.Hostname).IsUnique();

            builder.Ignore(e => e.DomainEvents);

        }
    }
}
