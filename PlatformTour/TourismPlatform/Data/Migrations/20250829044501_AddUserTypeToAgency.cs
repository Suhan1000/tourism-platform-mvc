using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourismPlatform.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserTypeToAgency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxToursAllowed",
                table: "Agencies",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserType",
                table: "Agencies",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxToursAllowed",
                table: "Agencies");

            migrationBuilder.DropColumn(
                name: "UserType",
                table: "Agencies");
        }
    }
}
