using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Configuration.Infrastructure.Migrations
{
    public partial class PluginPath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "Plugin",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Path",
                table: "Plugin");
        }
    }
}
