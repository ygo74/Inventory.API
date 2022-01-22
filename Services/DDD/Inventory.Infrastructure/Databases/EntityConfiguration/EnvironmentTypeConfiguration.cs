using Inventory.Domain.Models.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Infrastructure.Databases.EntityConfiguration
{
    public class EnvironmentTypeConfiguration : IEntityTypeConfiguration<Environment>
    {
        public void Configure(EntityTypeBuilder<Environment> builder)
        {
            builder.UseXminAsConcurrencyToken();

            builder.ToTable("Environments");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).UseHiLo("seqEnvironments");

            builder.HasIndex(e => e.Code).IsUnique();
            builder.HasIndex(e => e.Name).IsUnique();

            builder.Ignore(e => e.DomainEvents);
        }
    }
}
