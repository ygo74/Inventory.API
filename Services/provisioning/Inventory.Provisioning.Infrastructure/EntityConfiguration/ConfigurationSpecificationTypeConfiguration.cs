using Inventory.Common.Infrastructure.Database.Converters;
using Inventory.Provisioning.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Provisioning.Infrastructure.EntityConfiguration
{
    internal class ConfigurationSpecificationTypeConfiguration : IEntityTypeConfiguration<ConfigurationSpecification>
    {
        public void Configure(EntityTypeBuilder<ConfigurationSpecification> builder)
        {
            builder.UseXminAsConcurrencyToken();

            builder.ToTable("ConfigurationSpecification");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).UseHiLo("configuration_specification_ids");

            builder.HasIndex(e => e.Name).IsUnique();

            // custom column converters
            builder.Property(e => e.Specification).HasConversion<DictionaryJsonConverter>();

            builder.Ignore(e => e.DomainEvents);

        }
    }
}
