using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateAgency.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "image_id",
                table: "t_property",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "t_image",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    img_bytes = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_image", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_t_property_image_id",
                table: "t_property",
                column: "image_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_t_property_t_image_image_id",
                table: "t_property",
                column: "image_id",
                principalTable: "t_image",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_t_property_t_image_image_id",
                table: "t_property");

            migrationBuilder.DropTable(
                name: "t_image");

            migrationBuilder.DropIndex(
                name: "IX_t_property_image_id",
                table: "t_property");

            migrationBuilder.DropColumn(
                name: "image_id",
                table: "t_property");
        }
    }
}
