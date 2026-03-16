using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateAgency.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddChat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "t_chat_type",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_chat_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "t_chat",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    type_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_chat", x => x.id);
                    table.ForeignKey(
                        name: "FK_t_chat_t_chat_type_type_id",
                        column: x => x.type_id,
                        principalTable: "t_chat_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "t_chat_member",
                columns: table => new
                {
                    chat_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    joined_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_chat_member", x => new { x.chat_id, x.user_id });
                    table.ForeignKey(
                        name: "FK_t_chat_member_t_chat_chat_id",
                        column: x => x.chat_id,
                        principalTable: "t_chat",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_t_chat_member_t_user_user_id",
                        column: x => x.user_id,
                        principalTable: "t_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "t_message",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    chat_id = table.Column<Guid>(type: "uuid", nullable: false),
                    sender_id = table.Column<Guid>(type: "uuid", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_read = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_message", x => x.id);
                    table.ForeignKey(
                        name: "FK_t_message_t_chat_chat_id",
                        column: x => x.chat_id,
                        principalTable: "t_chat",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_t_message_t_user_sender_id",
                        column: x => x.sender_id,
                        principalTable: "t_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_t_chat_type_id",
                table: "t_chat",
                column: "type_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_chat_member_user_id",
                table: "t_chat_member",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_message_chat_id",
                table: "t_message",
                column: "chat_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_message_sender_id",
                table: "t_message",
                column: "sender_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "t_chat_member");

            migrationBuilder.DropTable(
                name: "t_message");

            migrationBuilder.DropTable(
                name: "t_chat");

            migrationBuilder.DropTable(
                name: "t_chat_type");
        }
    }
}
