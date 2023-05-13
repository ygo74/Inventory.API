using Inventory.Configuration.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Configuration.Infrastructure.EntityConfiguration
{
    internal class PluginEntityTypeConfiguration : IEntityTypeConfiguration<Plugin>
    {
        public void Configure(EntityTypeBuilder<Plugin> builder)
        {
            builder.UseXminAsConcurrencyToken();

            builder.ToTable("Plugin");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).UseHiLo("plugin_ids");

            builder.HasIndex(e => e.Name).IsUnique();

            builder.Ignore(e => e.DomainEvents);

        }

    }
}