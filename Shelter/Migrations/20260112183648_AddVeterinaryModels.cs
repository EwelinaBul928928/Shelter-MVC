using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shelter.Migrations
{
    /// <inheritdoc />
    public partial class AddVeterinaryModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VeterinarianName",
                table: "VeterinaryVisits");

            migrationBuilder.AddColumn<int>(
                name: "ServiceId",
                table: "VeterinaryVisits",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VeterinarianId",
                table: "VeterinaryVisits",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Veterinarians",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Specialization = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Phone = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Veterinarians", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VeterinaryServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Category = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VeterinaryServices", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VeterinaryVisits_ServiceId",
                table: "VeterinaryVisits",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_VeterinaryVisits_VeterinarianId",
                table: "VeterinaryVisits",
                column: "VeterinarianId");

            migrationBuilder.AddForeignKey(
                name: "FK_VeterinaryVisits_Veterinarians_VeterinarianId",
                table: "VeterinaryVisits",
                column: "VeterinarianId",
                principalTable: "Veterinarians",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_VeterinaryVisits_VeterinaryServices_ServiceId",
                table: "VeterinaryVisits",
                column: "ServiceId",
                principalTable: "VeterinaryServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VeterinaryVisits_Veterinarians_VeterinarianId",
                table: "VeterinaryVisits");

            migrationBuilder.DropForeignKey(
                name: "FK_VeterinaryVisits_VeterinaryServices_ServiceId",
                table: "VeterinaryVisits");

            migrationBuilder.DropTable(
                name: "Veterinarians");

            migrationBuilder.DropTable(
                name: "VeterinaryServices");

            migrationBuilder.DropIndex(
                name: "IX_VeterinaryVisits_ServiceId",
                table: "VeterinaryVisits");

            migrationBuilder.DropIndex(
                name: "IX_VeterinaryVisits_VeterinarianId",
                table: "VeterinaryVisits");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "VeterinaryVisits");

            migrationBuilder.DropColumn(
                name: "VeterinarianId",
                table: "VeterinaryVisits");

            migrationBuilder.AddColumn<string>(
                name: "VeterinarianName",
                table: "VeterinaryVisits",
                type: "TEXT",
                maxLength: 100,
                nullable: true);
        }
    }
}
