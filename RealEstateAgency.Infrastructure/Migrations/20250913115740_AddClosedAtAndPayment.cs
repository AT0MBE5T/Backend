using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateAgency.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddClosedAtAndPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AnnouncementId",
                table: "t_answer",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "close_at",
                table: "t_announcement",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "t_payment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    customer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    announcement_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_payment", x => x.id);
                    table.ForeignKey(
                        name: "FK_t_payment_t_announcement_announcement_id",
                        column: x => x.announcement_id,
                        principalTable: "t_announcement",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_t_payment_t_user_customer_id",
                        column: x => x.customer_id,
                        principalTable: "t_user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_t_answer_AnnouncementId",
                table: "t_answer",
                column: "AnnouncementId");

            migrationBuilder.CreateIndex(
                name: "IX_t_payment_announcement_id",
                table: "t_payment",
                column: "announcement_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_t_payment_customer_id",
                table: "t_payment",
                column: "customer_id");

            migrationBuilder.AddForeignKey(
                name: "FK_t_answer_t_announcement_AnnouncementId",
                table: "t_answer",
                column: "AnnouncementId",
                principalTable: "t_announcement",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_t_answer_t_announcement_AnnouncementId",
                table: "t_answer");

            migrationBuilder.DropTable(
                name: "t_payment");

            migrationBuilder.DropIndex(
                name: "IX_t_answer_AnnouncementId",
                table: "t_answer");

            migrationBuilder.DropColumn(
                name: "AnnouncementId",
                table: "t_answer");

            migrationBuilder.DropColumn(
                name: "close_at",
                table: "t_announcement");
        }
    }
}
