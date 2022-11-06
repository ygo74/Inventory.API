using Inventory.Configuration.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Configuration.Infrastructure.EntityConfiguration
{
    internal class CredentialEntityTypeConfiguration : IEntityTypeConfiguration<Credential>
    {
        public void Configure(EntityTypeBuilder<Credential> builder)
        {
            builder.UseXminAsConcurrencyToken();

            builder.ToTable("Credential");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).UseHiLo("credential_ids");

            builder.HasIndex(e => e.Name).IsUnique();

            builder.Ignore(e => e.DomainEvents);
        }
    }
}