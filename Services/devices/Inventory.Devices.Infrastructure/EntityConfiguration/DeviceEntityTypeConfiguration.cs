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
    public class DeviceEntityTypeConfiguration : IEntityTypeConfiguration<Device>
    {
        public void Configure(EntityTypeBuilder<Device> builder)
        {
            builder.UseXminAsConcurrencyToken();

            builder.ToTable("Device");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).UseHiLo("device_ids");

            builder.HasIndex(e => e.Hostname).IsUnique();


            builder.Property(e => e.PropertyBag)
                .HasConversion(
                    value => System.Text.Json.JsonSerializer.Serialize(value, typeof(Dictionary<string,object>),new System.Text.Json.JsonSerializerOptions()),
                    value => System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(value, new System.Text.Json.JsonSerializerOptions())
                );

            builder.Ignore(e => e.DomainEvents);

        }
    }
}
