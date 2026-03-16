using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateAgency.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAnswerNavigationFromAnnouncement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_t_answer_t_announcement_AnnouncementId",
                table: "t_answer");

            migrationBuilder.DropIndex(
                name: "IX_t_answer_AnnouncementId",
                table: "t_answer");

            migrationBuilder.DropColumn(
                name: "AnnouncementId",
                table: "t_answer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AnnouncementId",
                table: "t_answer",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_t_answer_AnnouncementId",
                table: "t_answer",
                column: "AnnouncementId");

            migrationBuilder.AddForeignKey(
                name: "FK_t_answer_t_announcement_AnnouncementId",
                table: "t_answer",
                column: "AnnouncementId",
                principalTable: "t_announcement",
                principalColumn: "id");
        }
    }
}
