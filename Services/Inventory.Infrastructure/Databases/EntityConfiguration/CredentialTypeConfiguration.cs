using Inventory.Domain.Models.Credentials;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Infrastructure.Databases.EntityConfiguration
{
    public class CredentialTypeConfiguration : IEntityTypeConfiguration<Credential>
    {
        public void Configure(EntityTypeBuilder<Credential> builder)
        {
            builder.UseXminAsConcurrencyToken();

            builder.ToTable("Credentials");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).UseHiLo("seqCredentials");

            builder.HasIndex(e => e.Name).IsUnique();

            builder.Ignore(e => e.DomainEvents);

            builder
                .HasDiscriminator<string>("blog_type")
                .HasValue<BasicCredential>("basic")
                .HasValue<AzureCredential>("azure");

        }
    }
}
