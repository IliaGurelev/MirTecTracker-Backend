using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tracker.Migrations
{
    /// <inheritdoc />
    public partial class BriefcaseTOSashboard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BriefcaseDashboard");

            migrationBuilder.AddColumn<int>(
                name: "BriefcaseId",
                table: "Briefcases",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DashboardId",
                table: "Briefcases",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Briefcases_BriefcaseId",
                table: "Briefcases",
                column: "BriefcaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Briefcases_DashboardId",
                table: "Briefcases",
                column: "DashboardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Briefcases_Briefcases_BriefcaseId",
                table: "Briefcases",
                column: "BriefcaseId",
                principalTable: "Briefcases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Briefcases_Dashboards_DashboardId",
                table: "Briefcases",
                column: "DashboardId",
                principalTable: "Dashboards",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Briefcases_Briefcases_BriefcaseId",
                table: "Briefcases");

            migrationBuilder.DropForeignKey(
                name: "FK_Briefcases_Dashboards_DashboardId",
                table: "Briefcases");

            migrationBuilder.DropIndex(
                name: "IX_Briefcases_BriefcaseId",
                table: "Briefcases");

            migrationBuilder.DropIndex(
                name: "IX_Briefcases_DashboardId",
                table: "Briefcases");

            migrationBuilder.DropColumn(
                name: "BriefcaseId",
                table: "Briefcases");

            migrationBuilder.DropColumn(
                name: "DashboardId",
                table: "Briefcases");

            migrationBuilder.CreateTable(
                name: "BriefcaseDashboard",
                columns: table => new
                {
                    BriefcaseId = table.Column<int>(type: "integer", nullable: false),
                    DashboardsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BriefcaseDashboard", x => new { x.BriefcaseId, x.DashboardsId });
                    table.ForeignKey(
                        name: "FK_BriefcaseDashboard_Briefcases_BriefcaseId",
                        column: x => x.BriefcaseId,
                        principalTable: "Briefcases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BriefcaseDashboard_Dashboards_DashboardsId",
                        column: x => x.DashboardsId,
                        principalTable: "Dashboards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BriefcaseDashboard_DashboardsId",
                table: "BriefcaseDashboard",
                column: "DashboardsId");
        }
    }
}
