using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateAgency.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeIdFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "t_user",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "t_role",
                newName: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "t_user",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "t_role",
                newName: "Id");
        }
    }
}
