using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace tracker.Migrations
{
    /// <inheritdoc />
    public partial class DashboardModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dashboards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Color = table.Column<string>(type: "text", nullable: false),
                    Invite = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dashboards", x => x.Id);
                });

            migrationBuilder.AddColumn<int>(
                name: "DashboardId",
                table: "Tasks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_DashboardId",
                table: "Tasks",
                column: "DashboardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Dashboards_DashboardId",
                table: "Tasks",
                column: "DashboardId",
                principalTable: "Dashboards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Dashboards_DashboardId",
                table: "Tasks");

            migrationBuilder.DropTable(
                name: "Dashboards");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_DashboardId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "DashboardId",
                table: "Tasks");
        }
    }
}
