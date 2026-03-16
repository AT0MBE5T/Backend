using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateAgency.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddComplaintsNViewsNFavorites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "t_complaint_status",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_complaint_status", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "t_complaint_type",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_complaint_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "t_favorite",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    announcement_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_favorite", x => new { x.user_id, x.announcement_id });
                    table.ForeignKey(
                        name: "FK_t_favorite_t_announcement_announcement_id",
                        column: x => x.announcement_id,
                        principalTable: "t_announcement",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_t_favorite_t_user_user_id",
                        column: x => x.user_id,
                        principalTable: "t_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "t_view",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    announcement_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_view", x => x.id);
                    table.ForeignKey(
                        name: "FK_t_view_t_announcement_announcement_id",
                        column: x => x.announcement_id,
                        principalTable: "t_announcement",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_t_view_t_user_user_id",
                        column: x => x.user_id,
                        principalTable: "t_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "t_complaint",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    announcement_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    user_note = table.Column<string>(type: "text", nullable: false),
                    admin_id = table.Column<Guid>(type: "uuid", nullable: false),
                    admin_note = table.Column<string>(type: "text", nullable: false),
                    type_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_complaint", x => x.id);
                    table.ForeignKey(
                        name: "FK_t_complaint_t_announcement_announcement_id",
                        column: x => x.announcement_id,
                        principalTable: "t_announcement",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_t_complaint_t_complaint_status_status_id",
                        column: x => x.status_id,
                        principalTable: "t_complaint_status",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_t_complaint_t_complaint_type_type_id",
                        column: x => x.type_id,
                        principalTable: "t_complaint_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_t_complaint_t_user_admin_id",
                        column: x => x.admin_id,
                        principalTable: "t_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_t_complaint_t_user_user_id",
                        column: x => x.user_id,
                        principalTable: "t_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_t_complaint_admin_id",
                table: "t_complaint",
                column: "admin_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_complaint_announcement_id",
                table: "t_complaint",
                column: "announcement_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_complaint_status_id",
                table: "t_complaint",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_complaint_type_id",
                table: "t_complaint",
                column: "type_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_complaint_user_id",
                table: "t_complaint",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_favorite_announcement_id",
                table: "t_favorite",
                column: "announcement_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_view_announcement_id",
                table: "t_view",
                column: "announcement_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_view_user_id",
                table: "t_view",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "t_complaint");

            migrationBuilder.DropTable(
                name: "t_favorite");

            migrationBuilder.DropTable(
                name: "t_view");

            migrationBuilder.DropTable(
                name: "t_complaint_status");

            migrationBuilder.DropTable(
                name: "t_complaint_type");
        }
    }
}
