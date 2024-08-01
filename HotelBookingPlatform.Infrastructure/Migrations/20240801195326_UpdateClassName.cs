using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBookingPlatform.Infrastructure.Migrations
{
    public partial class UpdateClassName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hotels_cities_CityID",
                table: "Hotels");

            migrationBuilder.DropTable(
                name: "Photos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_cities",
                table: "cities");

            migrationBuilder.RenameTable(
                name: "cities",
                newName: "Cities");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cities",
                table: "Cities",
                column: "CityID");

            migrationBuilder.AddForeignKey(
                name: "FK_Hotels_Cities_CityID",
                table: "Hotels",
                column: "CityID",
                principalTable: "Cities",
                principalColumn: "CityID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hotels_Cities_CityID",
                table: "Hotels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cities",
                table: "Cities");

            migrationBuilder.RenameTable(
                name: "Cities",
                newName: "cities");

            migrationBuilder.AddPrimaryKey(
                name: "PK_cities",
                table: "cities",
                column: "CityID");

            migrationBuilder.CreateTable(
                name: "Photos",
                columns: table => new
                {
                    PhotoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CityID = table.Column<int>(type: "int", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EntityId = table.Column<int>(type: "int", nullable: false),
                    EntityType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HotelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.PhotoId);
                    table.ForeignKey(
                        name: "FK_Photos_cities_CityID",
                        column: x => x.CityID,
                        principalTable: "cities",
                        principalColumn: "CityID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Photos_Hotels_HotelId",
                        column: x => x.HotelId,
                        principalTable: "Hotels",
                        principalColumn: "HotelId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Photos_CityID",
                table: "Photos",
                column: "CityID");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_HotelId",
                table: "Photos",
                column: "HotelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Hotels_cities_CityID",
                table: "Hotels",
                column: "CityID",
                principalTable: "cities",
                principalColumn: "CityID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
