using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourismPlatform.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddImagePathToTour : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Tours",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Tours");
        }
    }
}
