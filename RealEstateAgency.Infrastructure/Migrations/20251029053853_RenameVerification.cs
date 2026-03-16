using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateAgency.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameVerification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Verifications_t_announcement_announcement_id",
                table: "Verifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Verifications_t_user_created_by",
                table: "Verifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Verifications",
                table: "Verifications");

            migrationBuilder.RenameTable(
                name: "Verifications",
                newName: "t_verification");

            migrationBuilder.RenameIndex(
                name: "IX_Verifications_created_by",
                table: "t_verification",
                newName: "IX_t_verification_created_by");

            migrationBuilder.RenameIndex(
                name: "IX_Verifications_announcement_id",
                table: "t_verification",
                newName: "IX_t_verification_announcement_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_t_verification",
                table: "t_verification",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_t_verification_t_announcement_announcement_id",
                table: "t_verification",
                column: "announcement_id",
                principalTable: "t_announcement",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_t_verification_t_user_created_by",
                table: "t_verification",
                column: "created_by",
                principalTable: "t_user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_t_verification_t_announcement_announcement_id",
                table: "t_verification");

            migrationBuilder.DropForeignKey(
                name: "FK_t_verification_t_user_created_by",
                table: "t_verification");

            migrationBuilder.DropPrimaryKey(
                name: "PK_t_verification",
                table: "t_verification");

            migrationBuilder.RenameTable(
                name: "t_verification",
                newName: "Verifications");

            migrationBuilder.RenameIndex(
                name: "IX_t_verification_created_by",
                table: "Verifications",
                newName: "IX_Verifications_created_by");

            migrationBuilder.RenameIndex(
                name: "IX_t_verification_announcement_id",
                table: "Verifications",
                newName: "IX_Verifications_announcement_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Verifications",
                table: "Verifications",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Verifications_t_announcement_announcement_id",
                table: "Verifications",
                column: "announcement_id",
                principalTable: "t_announcement",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Verifications_t_user_created_by",
                table: "Verifications",
                column: "created_by",
                principalTable: "t_user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
