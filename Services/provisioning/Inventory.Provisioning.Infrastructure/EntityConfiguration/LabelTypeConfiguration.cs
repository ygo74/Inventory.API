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
    internal class LabelTypeConfiguration : IEntityTypeConfiguration<Label>
    {
        public void Configure(EntityTypeBuilder<Label> builder)
        {
            builder.UseXminAsConcurrencyToken();

            builder.ToTable("Labels");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).UseHiLo("label_ids");

            builder.Ignore(e => e.DomainEvents);

        }
    }
}
