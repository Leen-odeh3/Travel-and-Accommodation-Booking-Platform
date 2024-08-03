using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBookingPlatform.Infrastructure.Migrations
{
    public partial class CreateImages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Cities_CityID",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_CityID",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "CityID",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Cities");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Cities",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Cities");

            migrationBuilder.AddColumn<int>(
                name: "CityID",
                table: "Images",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Cities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Images_CityID",
                table: "Images",
                column: "CityID");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Cities_CityID",
                table: "Images",
                column: "CityID",
                principalTable: "Cities",
                principalColumn: "CityID");
        }
    }
}
