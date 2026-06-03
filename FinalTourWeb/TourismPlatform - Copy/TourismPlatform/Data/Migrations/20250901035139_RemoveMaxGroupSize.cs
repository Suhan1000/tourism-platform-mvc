using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourismPlatform.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveMaxGroupSize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxGroupSize",
                table: "Tours");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxGroupSize",
                table: "Tours",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
