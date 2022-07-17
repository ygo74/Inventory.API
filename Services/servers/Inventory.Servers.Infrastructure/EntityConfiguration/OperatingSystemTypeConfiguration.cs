using Inventory.Servers.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Servers.Infrastructure.EntityConfiguration
{
    class OperatingSystemTypeConfiguration : IEntityTypeConfiguration<OperatingSystem>
    {
        public void Configure(EntityTypeBuilder<OperatingSystem> builder)
        {
            builder.UseXminAsConcurrencyToken();

            builder.ToTable("OperatingSystems");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).UseHiLo("seqOperatingSystems");

            //builder.HasIndex(e => new { e.Family, e.Model, e.Version }).IsUnique();

            builder.Ignore(e => e.DomainEvents);
        }
    }
}
