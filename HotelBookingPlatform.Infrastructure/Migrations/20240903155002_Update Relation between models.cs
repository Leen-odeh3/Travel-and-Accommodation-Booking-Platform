using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBookingPlatform.Infrastructure.Migrations
{
    public partial class UpdateRelationbetweenmodels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Discounts_RoomClasses_RoomClassID",
                table: "Discounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Hotels_HotelId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "AdultsCapacity",
                table: "RoomClasses");

            migrationBuilder.DropColumn(
                name: "ChildrenCapacity",
                table: "RoomClasses");

            migrationBuilder.DropColumn(
                name: "PricePerNight",
                table: "RoomClasses");

            migrationBuilder.DropColumn(
                name: "FileData",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "ReviewsRating",
                table: "Hotels");

            migrationBuilder.RenameColumn(
                name: "EntityType",
                table: "Images",
                newName: "Url");

            migrationBuilder.RenameColumn(
                name: "confirmationNumber",
                table: "Bookings",
                newName: "ConfirmationNumber");

            migrationBuilder.RenameColumn(
                name: "CheckOutDate",
                table: "Bookings",
                newName: "CheckOutDateUtc");

            migrationBuilder.RenameColumn(
                name: "CheckInDate",
                table: "Bookings",
                newName: "CheckInDateUtc");

            migrationBuilder.AddColumn<int>(
                name: "AdultsCapacity",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ChildrenCapacity",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "PricePerNight",
                table: "Rooms",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Images",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "PublicId",
                table: "Images",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Images",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "RoomClassID",
                table: "Discounts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Discounts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "RoomID",
                table: "Discounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "ConfirmationNumber",
                table: "Bookings",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<decimal>(
                name: "AfterDiscountedPrice",
                table: "Bookings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_Discounts_RoomID",
                table: "Discounts",
                column: "RoomID");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ConfirmationNumber",
                table: "Bookings",
                column: "ConfirmationNumber",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Discounts_RoomClasses_RoomClassID",
                table: "Discounts",
                column: "RoomClassID",
                principalTable: "RoomClasses",
                principalColumn: "RoomClassID");

            migrationBuilder.AddForeignKey(
                name: "FK_Discounts_Rooms_RoomID",
                table: "Discounts",
                column: "RoomID",
                principalTable: "Rooms",
                principalColumn: "RoomID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Hotels_HotelId",
                table: "Reviews",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "HotelId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Discounts_RoomClasses_RoomClassID",
                table: "Discounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Discounts_Rooms_RoomID",
                table: "Discounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Hotels_HotelId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Discounts_RoomID",
                table: "Discounts");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_ConfirmationNumber",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "AdultsCapacity",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "ChildrenCapacity",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "PricePerNight",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "PublicId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "RoomID",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "AfterDiscountedPrice",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "Images",
                newName: "EntityType");

            migrationBuilder.RenameColumn(
                name: "ConfirmationNumber",
                table: "Bookings",
                newName: "confirmationNumber");

            migrationBuilder.RenameColumn(
                name: "CheckOutDateUtc",
                table: "Bookings",
                newName: "CheckOutDate");

            migrationBuilder.RenameColumn(
                name: "CheckInDateUtc",
                table: "Bookings",
                newName: "CheckInDate");

            migrationBuilder.AddColumn<int>(
                name: "AdultsCapacity",
                table: "RoomClasses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ChildrenCapacity",
                table: "RoomClasses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "PricePerNight",
                table: "RoomClasses",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<byte[]>(
                name: "FileData",
                table: "Images",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<double>(
                name: "ReviewsRating",
                table: "Hotels",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<int>(
                name: "RoomClassID",
                table: "Discounts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "confirmationNumber",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddForeignKey(
                name: "FK_Discounts_RoomClasses_RoomClassID",
                table: "Discounts",
                column: "RoomClassID",
                principalTable: "RoomClasses",
                principalColumn: "RoomClassID",
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
