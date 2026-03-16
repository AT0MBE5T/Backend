using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateAgency.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIsActive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_active",
                table: "t_announcement");

            migrationBuilder.RenameColumn(
                name: "close_at",
                table: "t_announcement",
                newName: "closed_at");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "closed_at",
                table: "t_announcement",
                newName: "close_at");

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "t_announcement",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
