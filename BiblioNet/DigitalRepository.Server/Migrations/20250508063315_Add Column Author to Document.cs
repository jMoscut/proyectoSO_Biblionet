using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalRepository.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnAuthortoDocument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "Documents",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Author",
                table: "Documents");
        }
    }
}
