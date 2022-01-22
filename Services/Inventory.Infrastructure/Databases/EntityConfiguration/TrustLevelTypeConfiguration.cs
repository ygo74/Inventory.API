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
    internal class TrustLevelTypeConfiguration : IEntityTypeConfiguration<TrustLevel>
    {
        public void Configure(EntityTypeBuilder<TrustLevel> builder)
        {
            builder.UseXminAsConcurrencyToken();

            builder.ToTable("TrustLevels");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).UseHiLo("seqTrustLevels");

            builder.HasIndex(e => e.Code).IsUnique();
            builder.HasIndex(e => e.Name).IsUnique();

            builder.Ignore(e => e.DomainEvents);

        }
    }
}
