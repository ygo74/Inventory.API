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
    internal class DataCenterTypeConfiguration : IEntityTypeConfiguration<DataCenter>
    {
        public void Configure(EntityTypeBuilder<DataCenter> builder)
        {
            builder.UseXminAsConcurrencyToken();

            builder.ToTable("DataCenters");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).UseHiLo("seqDataCenters");

            builder.HasIndex(e => e.Code).IsUnique();
            builder.HasIndex(e => e.Name).IsUnique();

            builder.Ignore(e => e.DomainEvents);
        }
    }
}
