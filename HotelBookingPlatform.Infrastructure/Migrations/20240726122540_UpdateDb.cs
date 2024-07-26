using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBookingPlatform.Infrastructure.Migrations
{
    public partial class UpdateDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Review_AspNetUsers_LocalUserId",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_Hotels_HotelId",
                table: "Review");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Review",
                table: "Review");

            migrationBuilder.RenameTable(
                name: "Review",
                newName: "Reviews");

            migrationBuilder.RenameIndex(
                name: "IX_Review_LocalUserId",
                table: "Reviews",
                newName: "IX_Reviews_LocalUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Review_HotelId",
                table: "Reviews",
                newName: "IX_Reviews_HotelId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews",
                column: "ReviewID");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_LocalUserId",
                table: "Reviews",
                column: "LocalUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Hotels_HotelId",
                table: "Reviews",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "HotelId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_LocalUserId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Hotels_HotelId",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews");

            migrationBuilder.RenameTable(
                name: "Reviews",
                newName: "Review");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_LocalUserId",
                table: "Review",
                newName: "IX_Review_LocalUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_HotelId",
                table: "Review",
                newName: "IX_Review_HotelId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Review",
                table: "Review",
                column: "ReviewID");

            migrationBuilder.AddForeignKey(
                name: "FK_Review_AspNetUsers_LocalUserId",
                table: "Review",
                column: "LocalUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Hotels_HotelId",
                table: "Review",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "HotelId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
