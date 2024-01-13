using Inventory.Networks.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Networks.Infrastructure.EntityConfiguration
{
    internal class PluginEntityTypeConfiguration : IEntityTypeConfiguration<NetworkPlugin>
    {
        public void Configure(EntityTypeBuilder<NetworkPlugin> builder)
        {
            builder.ToView("Plugin");
            builder.HasKey(e => e.Code);

            builder.HasIndex(e => e.Name).IsUnique();

        }

    }
}