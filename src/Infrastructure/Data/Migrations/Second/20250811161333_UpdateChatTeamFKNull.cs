    using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations.Second
{
    /// <inheritdoc />
    public partial class UpdateChatTeamFKNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Teams_ChatId",
                table: "Teams");

            migrationBuilder.AlterColumn<long>(
                name: "ChatId",
                table: "Teams",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_ChatId",
                table: "Teams",
                column: "ChatId",
                unique: true,
                filter: "[ChatId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Teams_ChatId",
                table: "Teams");

            migrationBuilder.AlterColumn<long>(
                name: "ChatId",
                table: "Teams",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teams_ChatId",
                table: "Teams",
                column: "ChatId",
                unique: true);
        }
    }
}
