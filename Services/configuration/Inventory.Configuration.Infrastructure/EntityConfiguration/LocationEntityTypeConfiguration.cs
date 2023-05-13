using Inventory.Configuration.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Configuration.Infrastructure.EntityConfiguration
{
    public class LocationEntityTypeConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.UseXminAsConcurrencyToken();

            builder.ToTable("Location");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).UseHiLo("location_ids");

            builder.HasIndex(e => new {e.CityCode, e.CountryCode, e.RegionCode}).IsUnique();

            builder.Ignore(e => e.DomainEvents);
        }
    }
}
