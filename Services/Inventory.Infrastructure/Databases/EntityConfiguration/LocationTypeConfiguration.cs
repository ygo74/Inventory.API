using Inventory.Domain.Models.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Infrastructure.Databases.EntityConfiguration
{
    class LocationTypeConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.UseXminAsConcurrencyToken();

            builder.ToTable("Locations");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).UseHiLo("seqLocations");

            builder.HasIndex(e => e.CityCode).IsUnique();

            builder.Ignore(e => e.DomainEvents);

        }
    }
}
