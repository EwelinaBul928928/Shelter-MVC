using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shelter.Migrations
{
    /// <inheritdoc />
    public partial class AddAdoptionApplicationFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "AdoptionApplications",
                type: "TEXT",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ExperienceWithAnimals",
                table: "AdoptionApplications",
                type: "TEXT",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HasGarden",
                table: "AdoptionApplications",
                type: "TEXT",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HasOtherPets",
                table: "AdoptionApplications",
                type: "TEXT",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LivingSituation",
                table: "AdoptionApplications",
                type: "TEXT",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "AdoptionApplications",
                type: "TEXT",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "AdoptionApplications");

            migrationBuilder.DropColumn(
                name: "ExperienceWithAnimals",
                table: "AdoptionApplications");

            migrationBuilder.DropColumn(
                name: "HasGarden",
                table: "AdoptionApplications");

            migrationBuilder.DropColumn(
                name: "HasOtherPets",
                table: "AdoptionApplications");

            migrationBuilder.DropColumn(
                name: "LivingSituation",
                table: "AdoptionApplications");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "AdoptionApplications");
        }
    }
}
