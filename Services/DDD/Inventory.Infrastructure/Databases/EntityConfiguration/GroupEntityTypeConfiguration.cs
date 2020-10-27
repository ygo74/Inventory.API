using Inventory.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Infrastructure.Databases.EntityConfiguration
{
    public class GroupEntityTypeConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.UseXminAsConcurrencyToken();

            builder.ToTable("Group");
            builder.HasKey(group => group.GroupId);
            builder.Property(g => g.GroupId).UseHiLo();

            builder.HasIndex(group => group.Name).IsUnique();

            builder.HasOne(g => g.Parent)
                   .WithMany(g => g.Children)
                   .HasForeignKey(g => g.ParentId)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
