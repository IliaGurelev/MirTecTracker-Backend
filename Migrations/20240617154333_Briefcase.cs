using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace tracker.Migrations
{
    /// <inheritdoc />
    public partial class Briefcase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Briefcase",
                table: "Briefcase");

            migrationBuilder.RenameTable(
                name: "Briefcase",
                newName: "Briefcases");

            migrationBuilder.AddColumn<int>(
                name: "ColorId",
                table: "Briefcases",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Briefcases",
                table: "Briefcases",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Colors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colors", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Briefcases_ColorId",
                table: "Briefcases",
                column: "ColorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Briefcases_Colors_ColorId",
                table: "Briefcases",
                column: "ColorId",
                principalTable: "Colors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Briefcases_Colors_ColorId",
                table: "Briefcases");

            migrationBuilder.DropTable(
                name: "Colors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Briefcases",
                table: "Briefcases");

            migrationBuilder.DropIndex(
                name: "IX_Briefcases_ColorId",
                table: "Briefcases");

            migrationBuilder.DropColumn(
                name: "ColorId",
                table: "Briefcases");

            migrationBuilder.RenameTable(
                name: "Briefcases",
                newName: "Briefcase");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Briefcase",
                table: "Briefcase",
                column: "Id");
        }
    }
}
