using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBookingPlatform.Infrastructure.Migrations
{
    public partial class CreateAmenityModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Amenities",
                columns: table => new
                {
                    AmenityID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Amenities", x => x.AmenityID);
                });

            migrationBuilder.CreateTable(
                name: "RoomClassAmenity",
                columns: table => new
                {
                    AmenityID = table.Column<int>(type: "int", nullable: false),
                    RoomClassID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomClassAmenity", x => new { x.AmenityID, x.RoomClassID });
                    table.ForeignKey(
                        name: "FK_RoomClassAmenity_Amenities_AmenityID",
                        column: x => x.AmenityID,
                        principalTable: "Amenities",
                        principalColumn: "AmenityID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoomClassAmenity_RoomClasses_RoomClassID",
                        column: x => x.RoomClassID,
                        principalTable: "RoomClasses",
                        principalColumn: "RoomClassID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoomClassAmenity_RoomClassID",
                table: "RoomClassAmenity",
                column: "RoomClassID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoomClassAmenity");

            migrationBuilder.DropTable(
                name: "Amenities");
        }
    }
}
