using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBookingPlatform.Infrastructure.Migrations
{
    public partial class CreateDbSetTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hotels_City_CityID",
                table: "Hotels");

            migrationBuilder.DropForeignKey(
                name: "FK_Hotels_Owner_OwnerID",
                table: "Hotels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Owner",
                table: "Owner");

            migrationBuilder.DropPrimaryKey(
                name: "PK_City",
                table: "City");

            migrationBuilder.RenameTable(
                name: "Owner",
                newName: "owners");

            migrationBuilder.RenameTable(
                name: "City",
                newName: "cities");

            migrationBuilder.AddPrimaryKey(
                name: "PK_owners",
                table: "owners",
                column: "OwnerID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_cities",
                table: "cities",
                column: "CityID");

            migrationBuilder.AddForeignKey(
                name: "FK_Hotels_cities_CityID",
                table: "Hotels",
                column: "CityID",
                principalTable: "cities",
                principalColumn: "CityID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Hotels_owners_OwnerID",
                table: "Hotels",
                column: "OwnerID",
                principalTable: "owners",
                principalColumn: "OwnerID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hotels_cities_CityID",
                table: "Hotels");

            migrationBuilder.DropForeignKey(
                name: "FK_Hotels_owners_OwnerID",
                table: "Hotels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_owners",
                table: "owners");

            migrationBuilder.DropPrimaryKey(
                name: "PK_cities",
                table: "cities");

            migrationBuilder.RenameTable(
                name: "owners",
                newName: "Owner");

            migrationBuilder.RenameTable(
                name: "cities",
                newName: "City");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Owner",
                table: "Owner",
                column: "OwnerID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_City",
                table: "City",
                column: "CityID");

            migrationBuilder.AddForeignKey(
                name: "FK_Hotels_City_CityID",
                table: "Hotels",
                column: "CityID",
                principalTable: "City",
                principalColumn: "CityID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Hotels_Owner_OwnerID",
                table: "Hotels",
                column: "OwnerID",
                principalTable: "Owner",
                principalColumn: "OwnerID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
