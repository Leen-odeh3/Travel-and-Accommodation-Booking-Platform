using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBookingPlatform.Infrastructure.Migrations
{
    public partial class UpdateColumnASPNETUser2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_AspNetUsers_LocalUserId",
                table: "Bookings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bookings",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Bookings");

            migrationBuilder.AlterColumn<int>(
                name: "ReviewID",
                table: "Reviews",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "LocalUserId",
                table: "Bookings",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BookingID",
                table: "Bookings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews",
                column: "ReviewID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bookings",
                table: "Bookings",
                column: "BookingID");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_AspNetUsers_LocalUserId",
                table: "Bookings",
                column: "LocalUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_AspNetUsers_LocalUserId",
                table: "Bookings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bookings",
                table: "Bookings");

            migrationBuilder.AlterColumn<int>(
                name: "ReviewID",
                table: "Reviews",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "Reviews",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "LocalUserId",
                table: "Bookings",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "BookingID",
                table: "Bookings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "Bookings",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bookings",
                table: "Bookings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_AspNetUsers_LocalUserId",
                table: "Bookings",
                column: "LocalUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
