using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Devices.Infrastructure.Migrations
{
    public partial class devicetype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "datacenter_ids",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "device_ids",
                incrementBy: 10);

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "OperatingSystems",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastModified",
                table: "OperatingSystems",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "OperatingSystems",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "OperatingSystems",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AddColumn<string>(
                name: "InventoryCode",
                table: "OperatingSystems",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Datacenter",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Datacenter", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Device",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    DeviceType = table.Column<string>(type: "text", nullable: false),
                    Hostname = table.Column<string>(type: "text", nullable: true),
                    DnsDomain = table.Column<string>(type: "text", nullable: true),
                    SubnetIP = table.Column<string>(type: "text", nullable: true),
                    DataCenterId = table.Column<int>(type: "integer", nullable: false),
                    PropertyBag = table.Column<string>(type: "text", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
                    OperatingSystemId = table.Column<int>(type: "integer", nullable: true),
                    ProviderId = table.Column<int>(type: "integer", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Device", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Device_Datacenter_DataCenterId",
                        column: x => x.DataCenterId,
                        principalTable: "Datacenter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Device_OperatingSystems_OperatingSystemId",
                        column: x => x.OperatingSystemId,
                        principalTable: "OperatingSystems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Datacenter_Name",
                table: "Datacenter",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Device_DataCenterId",
                table: "Device",
                column: "DataCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_Device_Hostname",
                table: "Device",
                column: "Hostname",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Device_OperatingSystemId",
                table: "Device",
                column: "OperatingSystemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Device");

            migrationBuilder.DropTable(
                name: "Datacenter");

            migrationBuilder.DropSequence(
                name: "datacenter_ids");

            migrationBuilder.DropSequence(
                name: "device_ids");

            migrationBuilder.DropColumn(
                name: "InventoryCode",
                table: "OperatingSystems");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "OperatingSystems",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastModified",
                table: "OperatingSystems",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "OperatingSystems",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "OperatingSystems",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");
        }
    }
}
