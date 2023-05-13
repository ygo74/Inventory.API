using Inventory.Devices.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Devices.Infrastructure.EntityConfiguration
{
    public class DatacenterEntityTypeConfiguration : IEntityTypeConfiguration<DataCenter>
    {
        public void Configure(EntityTypeBuilder<DataCenter> builder)
        {
            builder.UseXminAsConcurrencyToken();

            builder.ToTable("Datacenter");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).UseHiLo("datacenter_ids");

            builder.HasIndex(e => e.Name).IsUnique();


            builder.Ignore(e => e.DomainEvents);

        }
    }
}
