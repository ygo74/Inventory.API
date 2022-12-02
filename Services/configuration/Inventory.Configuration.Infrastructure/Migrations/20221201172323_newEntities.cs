using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Inventory.Configuration.Infrastructure.Migrations
{
    public partial class newEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Credential_Plugin_PluginProviderId",
                table: "Credential");

            migrationBuilder.DropIndex(
                name: "IX_Credential_PluginProviderId",
                table: "Credential");

            migrationBuilder.DropColumn(
                name: "PluginProviderId",
                table: "Credential");

            migrationBuilder.CreateSequence(
                name: "location_ids",
                incrementBy: 10);

            migrationBuilder.AddColumn<string>(
                name: "Version",
                table: "Plugin",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Datacenter",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "Datacenter",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PropertyBag",
                table: "Credential",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CountryCode = table.Column<string>(type: "text", nullable: true),
                    CityCode = table.Column<string>(type: "text", nullable: true),
                    RegionCode = table.Column<string>(type: "text", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    Deprecated = table.Column<bool>(type: "boolean", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    InventoryCode = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PluginEndpoint",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DatacenterId = table.Column<int>(type: "integer", nullable: false),
                    CredentialId = table.Column<int>(type: "integer", nullable: true),
                    PluginId = table.Column<int>(type: "integer", nullable: true),
                    PropertyBag = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PluginEndpoint", x => new { x.DatacenterId, x.Id });
                    table.ForeignKey(
                        name: "FK_PluginEndpoint_Credential_CredentialId",
                        column: x => x.CredentialId,
                        principalTable: "Credential",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PluginEndpoint_Datacenter_DatacenterId",
                        column: x => x.DatacenterId,
                        principalTable: "Datacenter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PluginEndpoint_Plugin_PluginId",
                        column: x => x.PluginId,
                        principalTable: "Plugin",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Datacenter_LocationId",
                table: "Datacenter",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Location_CityCode_CountryCode_RegionCode",
                table: "Location",
                columns: new[] { "CityCode", "CountryCode", "RegionCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PluginEndpoint_CredentialId",
                table: "PluginEndpoint",
                column: "CredentialId");

            migrationBuilder.CreateIndex(
                name: "IX_PluginEndpoint_PluginId",
                table: "PluginEndpoint",
                column: "PluginId");

            migrationBuilder.AddForeignKey(
                name: "FK_Datacenter_Location_LocationId",
                table: "Datacenter",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Datacenter_Location_LocationId",
                table: "Datacenter");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropTable(
                name: "PluginEndpoint");

            migrationBuilder.DropIndex(
                name: "IX_Datacenter_LocationId",
                table: "Datacenter");

            migrationBuilder.DropSequence(
                name: "location_ids");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Plugin");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Datacenter");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Datacenter");

            migrationBuilder.DropColumn(
                name: "PropertyBag",
                table: "Credential");

            migrationBuilder.AddColumn<int>(
                name: "PluginProviderId",
                table: "Credential",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Credential_PluginProviderId",
                table: "Credential",
                column: "PluginProviderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Credential_Plugin_PluginProviderId",
                table: "Credential",
                column: "PluginProviderId",
                principalTable: "Plugin",
                principalColumn: "Id");
        }
    }
}
