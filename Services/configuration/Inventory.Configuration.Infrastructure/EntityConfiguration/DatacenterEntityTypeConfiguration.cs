using Inventory.Common.Infrastructure.Database.Converters;
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
    public class DatacenterEntityTypeConfiguration : IEntityTypeConfiguration<Datacenter>
    {
        public void Configure(EntityTypeBuilder<Datacenter> builder)
        {
            builder.UseXminAsConcurrencyToken();

            builder.ToTable("Datacenter");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).UseHiLo("datacenter_ids");

            builder.HasIndex(e => e.Code).IsUnique();

            // relations
            builder.HasOne(e => e.Location)
                .WithMany(e => e.Datacenters);

            builder.OwnsMany(e => e.Plugins, a =>
            {
                a.Property(x => x.PropertyBag).HasConversion<DictionaryJsonConverter>();
                a.Ignore(x => x.DomainEvents);
            });

            builder.Ignore(e => e.DomainEvents);

        }
    }
}
