using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClanControlPanel.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class migration4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WasPresent",
                table: "EventAttendances");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "WasPresent",
                table: "EventAttendances",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
