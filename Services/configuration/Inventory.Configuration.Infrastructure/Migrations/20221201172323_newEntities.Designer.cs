﻿// <auto-generated />
using System;
using Inventory.Configuration.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Inventory.Configuration.Infrastructure.Migrations
{
    [DbContext(typeof(ConfigurationDbContext))]
    [Migration("20221201172323_newEntities")]
    partial class newEntities
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.HasSequence("credential_ids")
                .IncrementsBy(10);

            modelBuilder.HasSequence("datacenter_ids")
                .IncrementsBy(10);

            modelBuilder.HasSequence("location_ids")
                .IncrementsBy(10);

            modelBuilder.HasSequence("plugin_ids")
                .IncrementsBy(10);

            modelBuilder.Entity("Inventory.Configuration.Domain.Models.Credential", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseHiLo(b.Property<int>("Id"), "credential_ids");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("PropertyBag")
                        .HasColumnType("text");

                    b.Property<uint>("xmin")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Credential", (string)null);
                });

            modelBuilder.Entity("Inventory.Configuration.Domain.Models.Datacenter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseHiLo(b.Property<int>("Id"), "datacenter_ids");

                    b.Property<string>("Code")
                        .HasColumnType("text");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<int>("DataCenterType")
                        .HasColumnType("integer");

                    b.Property<bool>("Deprecated")
                        .HasColumnType("boolean");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("InventoryCode")
                        .HasColumnType("text");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text");

                    b.Property<int?>("LocationId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<uint>("xmin")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.HasIndex("LocationId");

                    b.ToTable("Datacenter", (string)null);
                });

            modelBuilder.Entity("Inventory.Configuration.Domain.Models.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseHiLo(b.Property<int>("Id"), "location_ids");

                    b.Property<string>("CityCode")
                        .HasColumnType("text");

                    b.Property<string>("CountryCode")
                        .HasColumnType("text");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<bool>("Deprecated")
                        .HasColumnType("boolean");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("InventoryCode")
                        .HasColumnType("text");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("RegionCode")
                        .HasColumnType("text");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<uint>("xmin")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid");

                    b.HasKey("Id");

                    b.HasIndex("CityCode", "CountryCode", "RegionCode")
                        .IsUnique();

                    b.ToTable("Location", (string)null);
                });

            modelBuilder.Entity("Inventory.Configuration.Domain.Models.Plugin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseHiLo(b.Property<int>("Id"), "plugin_ids");

                    b.Property<string>("Code")
                        .HasColumnType("text");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<bool>("Deprecated")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("InventoryCode")
                        .HasColumnType("text");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Path")
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

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Plugin", (string)null);
                });

            modelBuilder.Entity("Inventory.Configuration.Domain.Models.Datacenter", b =>
                {
                    b.HasOne("Inventory.Configuration.Domain.Models.Location", "Location")
                        .WithMany("Datacenters")
                        .HasForeignKey("LocationId");

                    b.OwnsMany("Inventory.Configuration.Domain.Models.PluginEndpoint", "Plugins", b1 =>
                        {
                            b1.Property<int>("DatacenterId")
                                .HasColumnType("integer");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<int>("Id"));

                            b1.Property<DateTime>("Created")
                                .HasColumnType("timestamp with time zone");

                            b1.Property<string>("CreatedBy")
                                .HasColumnType("text");

                            b1.Property<int?>("CredentialId")
                                .HasColumnType("integer");

                            b1.Property<DateTime?>("LastModified")
                                .HasColumnType("timestamp with time zone");

                            b1.Property<string>("LastModifiedBy")
                                .HasColumnType("text");

                            b1.Property<int?>("PluginId")
                                .HasColumnType("integer");

                            b1.Property<string>("PropertyBag")
                                .HasColumnType("text");

                            b1.HasKey("DatacenterId", "Id");

                            b1.HasIndex("CredentialId");

                            b1.HasIndex("PluginId");

                            b1.ToTable("PluginEndpoint");

                            b1.HasOne("Inventory.Configuration.Domain.Models.Credential", "Credential")
                                .WithMany()
                                .HasForeignKey("CredentialId");

                            b1.WithOwner()
                                .HasForeignKey("DatacenterId");

                            b1.HasOne("Inventory.Configuration.Domain.Models.Plugin", "Plugin")
                                .WithMany()
                                .HasForeignKey("PluginId");

                            b1.Navigation("Credential");

                            b1.Navigation("Plugin");
                        });

                    b.Navigation("Location");

                    b.Navigation("Plugins");
                });

            modelBuilder.Entity("Inventory.Configuration.Domain.Models.Location", b =>
                {
                    b.Navigation("Datacenters");
                });
#pragma warning restore 612, 618
        }
    }
}
