using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eRe.Migrations
{
    /// <inheritdoc />
    public partial class add_RankingField_OverallGrade_Report : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OverallGrade",
                table: "Reports",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Ranking",
                table: "Reports",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OverallGrade",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "Ranking",
                table: "Reports");
        }
    }
}
