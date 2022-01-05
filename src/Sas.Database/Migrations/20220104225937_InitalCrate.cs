using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sas.BodyDatabase.Migrations
{
    public partial class InitalCrate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Positions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    X = table.Column<double>(type: "float", nullable: false),
                    Y = table.Column<double>(type: "float", nullable: false),
                    Z = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Velocities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    X = table.Column<double>(type: "float", nullable: false),
                    Y = table.Column<double>(type: "float", nullable: false),
                    Z = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Velocities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bodies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mass = table.Column<double>(type: "float", nullable: false),
                    Radius = table.Column<double>(type: "float", nullable: false),
                    PositionId = table.Column<int>(type: "int", nullable: false),
                    VelocityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bodies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bodies_Positions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bodies_Velocities_VelocityId",
                        column: x => x.VelocityId,
                        principalTable: "Velocities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Positions",
                columns: new[] { "Id", "X", "Y", "Z" },
                values: new object[,]
                {
                    { 1, 10.0, 0.0, 0.0 },
                    { 2, -10.0, 0.0, 0.0 }
                });

            migrationBuilder.InsertData(
                table: "Velocities",
                columns: new[] { "Id", "X", "Y", "Z" },
                values: new object[,]
                {
                    { 1, 0.0, 10.0, 0.0 },
                    { 2, 0.0, -10.0, 0.0 }
                });

            migrationBuilder.InsertData(
                table: "Bodies",
                columns: new[] { "Id", "Mass", "Name", "PositionId", "Radius", "VelocityId" },
                values: new object[] { 1, 1000.0, "body1", 1, 5.0, 1 });

            migrationBuilder.InsertData(
                table: "Bodies",
                columns: new[] { "Id", "Mass", "Name", "PositionId", "Radius", "VelocityId" },
                values: new object[] { 2, 1000.0, "body2", 2, 5.0, 2 });

            migrationBuilder.CreateIndex(
                name: "IX_Bodies_PositionId",
                table: "Bodies",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Bodies_VelocityId",
                table: "Bodies",
                column: "VelocityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bodies");

            migrationBuilder.DropTable(
                name: "Positions");

            migrationBuilder.DropTable(
                name: "Velocities");
        }
    }
}
