using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Networks.Infrastructure.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
            @"
                EXEC ('CREATE PROCEDURE getFullName
                    @LastName nvarchar(50),
                    @FirstName nvarchar(50)
                AS
                    RETURN @LastName + @FirstName;')"
            );

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
