using Inventory.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Infrastructure.EntityConfiguration
{
    public class ServerEntityTypeConfiguration : IEntityTypeConfiguration<Server>
    {
        public void Configure(EntityTypeBuilder<Server> builder)
        {
            builder.ToTable("Server");
            builder.HasKey(server => server.ServerId);
            builder.HasIndex(server => server.HostName).IsUnique();

            //builder.OwnsMany(s => s.Variables);

        }
    }
}
