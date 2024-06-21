using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tracker.Migrations
{
    /// <inheritdoc />
    public partial class userConnectDiaryV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Diary_Users_StatusId",
                table: "Diary");

            migrationBuilder.DropIndex(
                name: "IX_Diary_StatusId",
                table: "Diary");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Diary");

            migrationBuilder.CreateIndex(
                name: "IX_Diary_UserId",
                table: "Diary",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Diary_Users_UserId",
                table: "Diary",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Diary_Users_UserId",
                table: "Diary");

            migrationBuilder.DropIndex(
                name: "IX_Diary_UserId",
                table: "Diary");

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Diary",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Diary_StatusId",
                table: "Diary",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Diary_Users_StatusId",
                table: "Diary",
                column: "StatusId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
