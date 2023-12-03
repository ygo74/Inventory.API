﻿// <auto-generated />
using System;
using Inventory.Devices.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Inventory.Devices.Infrastructure.Migrations
{
    [DbContext(typeof(ServerDbContext))]
    partial class ServerDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.HasSequence("datacenter_ids")
                .IncrementsBy(10);

            modelBuilder.HasSequence("device_ids")
                .IncrementsBy(10);

            modelBuilder.HasSequence("seqOperatingSystems")
                .IncrementsBy(10);

            modelBuilder.Entity("Inventory.Devices.Domain.Models.DataCenter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseHiLo(b.Property<int>("Id"), "datacenter_ids");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<uint>("xmin")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Datacenter", (string)null);
                });

            modelBuilder.Entity("Inventory.Devices.Domain.Models.Device", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseHiLo(b.Property<int>("Id"), "device_ids");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<int>("DataCenterId")
                        .HasColumnType("integer");

                    b.Property<string>("DeviceType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("DnsDomain")
                        .HasColumnType("text");

                    b.Property<string>("Hostname")
                        .HasColumnType("text");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text");

                    b.Property<string>("PropertyBag")
                        .HasColumnType("text");

                    b.Property<string>("SubnetIP")
                        .HasColumnType("text");

                    b.Property<uint>("xmin")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid");

                    b.HasKey("Id");

                    b.HasIndex("DataCenterId");

                    b.HasIndex("Hostname")
                        .IsUnique();

                    b.ToTable("Device", (string)null);

                    b.HasDiscriminator<string>("DeviceType").HasValue("Device");
                });

            modelBuilder.Entity("Inventory.Devices.Domain.Models.OperatingSystem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseHiLo(b.Property<int>("Id"), "seqOperatingSystems");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<bool>("Deprecated")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("FamilyId")
                        .HasColumnType("integer");

                    b.Property<string>("InventoryCode")
                        .HasColumnType("text");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text");

                    b.Property<string>("Model")
                        .HasColumnType("text");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Version")
                        .HasColumnType("text");

                    b.Property<uint>("xmin")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid");

                    b.HasKey("Id");

                    b.HasIndex("FamilyId");

                    b.ToTable("OperatingSystems", (string)null);
                });

            modelBuilder.Entity("Inventory.Devices.Domain.Models.OperatingSystemFamily", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("OperatingSystemFamily");
                });

            modelBuilder.Entity("Inventory.Devices.Domain.Models.NetworkSwitch", b =>
                {
                    b.HasBaseType("Inventory.Devices.Domain.Models.Device");

                    b.HasDiscriminator().HasValue("NetworkSwitch");
                });

            modelBuilder.Entity("Inventory.Devices.Domain.Models.Server", b =>
                {
                    b.HasBaseType("Inventory.Devices.Domain.Models.Device");

                    b.Property<int>("OperatingSystemId")
                        .HasColumnType("integer");

                    b.HasIndex("OperatingSystemId");

                    b.HasDiscriminator().HasValue("Server");
                });

            modelBuilder.Entity("Inventory.Devices.Domain.Models.VirtualServer", b =>
                {
                    b.HasBaseType("Inventory.Devices.Domain.Models.Server");

                    b.Property<int>("ProviderId")
                        .HasColumnType("integer");

                    b.HasDiscriminator().HasValue("VirtualServer");
                });

            modelBuilder.Entity("Inventory.Devices.Domain.Models.Device", b =>
                {
                    b.HasOne("Inventory.Devices.Domain.Models.DataCenter", "DataCenter")
                        .WithMany()
                        .HasForeignKey("DataCenterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DataCenter");
                });

            modelBuilder.Entity("Inventory.Devices.Domain.Models.OperatingSystem", b =>
                {
                    b.HasOne("Inventory.Devices.Domain.Models.OperatingSystemFamily", "Family")
                        .WithMany()
                        .HasForeignKey("FamilyId");

                    b.Navigation("Family");
                });

            modelBuilder.Entity("Inventory.Devices.Domain.Models.Server", b =>
                {
                    b.HasOne("Inventory.Devices.Domain.Models.OperatingSystem", "OperatingSystem")
                        .WithMany()
                        .HasForeignKey("OperatingSystemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OperatingSystem");
                });
#pragma warning restore 612, 618
        }
    }
}
