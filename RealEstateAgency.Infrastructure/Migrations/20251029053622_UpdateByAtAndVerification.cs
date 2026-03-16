using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateAgency.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateByAtAndVerification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "t_announcement",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "updated_by",
                table: "t_announcement",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Verifications",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    announcement_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Verifications", x => x.id);
                    table.ForeignKey(
                        name: "FK_Verifications_t_announcement_announcement_id",
                        column: x => x.announcement_id,
                        principalTable: "t_announcement",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Verifications_t_user_created_by",
                        column: x => x.created_by,
                        principalTable: "t_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_t_announcement_updated_by",
                table: "t_announcement",
                column: "updated_by");

            migrationBuilder.CreateIndex(
                name: "IX_Verifications_announcement_id",
                table: "Verifications",
                column: "announcement_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Verifications_created_by",
                table: "Verifications",
                column: "created_by");

            migrationBuilder.AddForeignKey(
                name: "FK_t_announcement_t_user_updated_by",
                table: "t_announcement",
                column: "updated_by",
                principalTable: "t_user",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_t_announcement_t_user_updated_by",
                table: "t_announcement");

            migrationBuilder.DropTable(
                name: "Verifications");

            migrationBuilder.DropIndex(
                name: "IX_t_announcement_updated_by",
                table: "t_announcement");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "t_announcement");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "t_announcement");
        }
    }
}
