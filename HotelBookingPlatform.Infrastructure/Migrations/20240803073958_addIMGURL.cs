using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBookingPlatform.Infrastructure.Migrations
{
    public partial class addIMGURL : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Amenities_Hotels_HotelId",
                table: "Amenities");

            migrationBuilder.AddColumn<int>(
                name: "HotelId",
                table: "Images",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Cities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "HotelId",
                table: "Amenities",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Images_HotelId",
                table: "Images",
                column: "HotelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Amenities_Hotels_HotelId",
                table: "Amenities",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "HotelId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Hotels_HotelId",
                table: "Images",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "HotelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Amenities_Hotels_HotelId",
                table: "Amenities");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_Hotels_HotelId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_HotelId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "HotelId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Cities");

            migrationBuilder.AlterColumn<int>(
                name: "HotelId",
                table: "Amenities",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Amenities_Hotels_HotelId",
                table: "Amenities",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "HotelId");
        }
    }
}
