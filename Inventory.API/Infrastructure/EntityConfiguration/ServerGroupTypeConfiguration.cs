using Inventory.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.API.Infrastructure.EntityConfiguration
{
    public class ServerGroupTypeConfiguration : IEntityTypeConfiguration<ServerGroup>
    {
        public void Configure(EntityTypeBuilder<ServerGroup> builder)
        {
            builder.HasKey(k => new { k.ServerId, k.GroupId });

            builder.HasOne(sg => sg.Group)
                   .WithMany(s => s.Servers)
                   .HasForeignKey(sg => sg.GroupId);


            builder.HasOne(sg => sg.Server)
                   .WithMany(g => g.Groups)
                   .HasForeignKey(sg => sg.ServerId);


        }
    }
}
