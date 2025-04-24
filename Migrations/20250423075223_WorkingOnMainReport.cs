using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eRe.Migrations
{
    /// <inheritdoc />
    public partial class WorkingOnMainReport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LevelId",
                table: "MainReports",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LevelId",
                table: "Enrollments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_MainReports_StudentId_MonthId_LevelId",
                table: "MainReports",
                columns: new[] { "StudentId", "MonthId", "LevelId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_MainReports_StudentId_MonthId_LevelId",
                table: "MainReports");

            migrationBuilder.DropColumn(
                name: "LevelId",
                table: "MainReports");

            migrationBuilder.DropColumn(
                name: "LevelId",
                table: "Enrollments");
        }
    }
}
