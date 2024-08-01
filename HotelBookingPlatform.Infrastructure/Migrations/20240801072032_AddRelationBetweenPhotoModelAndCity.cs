using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBookingPlatform.Infrastructure.Migrations
{
    public partial class AddRelationBetweenPhotoModelAndCity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_cities_CityID",
                table: "Photos");

            migrationBuilder.AlterColumn<int>(
                name: "CityID",
                table: "Photos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_cities_CityID",
                table: "Photos",
                column: "CityID",
                principalTable: "cities",
                principalColumn: "CityID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_cities_CityID",
                table: "Photos");

            migrationBuilder.AlterColumn<int>(
                name: "CityID",
                table: "Photos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_cities_CityID",
                table: "Photos",
                column: "CityID",
                principalTable: "cities",
                principalColumn: "CityID");
        }
    }
}
