using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBookingPlatform.Infrastructure.Migrations
{
    public partial class AddNewColumnINCityTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "cities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "cities");
        }
    }
}
