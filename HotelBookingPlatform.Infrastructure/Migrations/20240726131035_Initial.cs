using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBookingPlatform.Infrastructure.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Bookings");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "Reviews",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
