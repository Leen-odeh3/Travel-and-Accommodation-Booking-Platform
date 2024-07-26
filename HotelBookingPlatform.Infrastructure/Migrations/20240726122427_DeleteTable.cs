using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBookingPlatform.Infrastructure.Migrations
{
    public partial class DeleteTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_Id",
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

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Review",
                newName: "LocalUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_HotelId",
                table: "Review",
                newName: "IX_Review_HotelId");

            migrationBuilder.AlterColumn<int>(
                name: "ReviewID",
                table: "Review",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "Review",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Review",
                table: "Review",
                column: "ReviewID");

            migrationBuilder.CreateIndex(
                name: "IX_Review_LocalUserId",
                table: "Review",
                column: "LocalUserId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropIndex(
                name: "IX_Review_LocalUserId",
                table: "Review");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Review");

            migrationBuilder.RenameTable(
                name: "Review",
                newName: "Reviews");

            migrationBuilder.RenameColumn(
                name: "LocalUserId",
                table: "Reviews",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Review_HotelId",
                table: "Reviews",
                newName: "IX_Reviews_HotelId");

            migrationBuilder.AlterColumn<int>(
                name: "ReviewID",
                table: "Reviews",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_Id",
                table: "Reviews",
                column: "Id",
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
    }
}
