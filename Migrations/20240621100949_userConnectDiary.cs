using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tracker.Migrations
{
    /// <inheritdoc />
    public partial class userConnectDiary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Diary",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Diary");
        }
    }
}
