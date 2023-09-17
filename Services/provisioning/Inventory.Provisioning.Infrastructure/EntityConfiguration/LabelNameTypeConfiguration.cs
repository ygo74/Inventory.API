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
    internal class LabelNameTypeConfiguration : IEntityTypeConfiguration<LabelName>
    {
        public void Configure(EntityTypeBuilder<LabelName> builder)
        {
            builder.UseXminAsConcurrencyToken();

            builder.ToTable("LabelName");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).UseHiLo("label_name_ids");

            builder.HasIndex(e => e.Name).IsUnique();

            builder.Ignore(e => e.DomainEvents);

        }
    }
}
