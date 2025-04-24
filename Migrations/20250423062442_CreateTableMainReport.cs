using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eRe.Migrations
{
    /// <inheritdoc />
    public partial class CreateTableMainReport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isActive",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "StudentEmail",
                table: "Enrollments",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CourseId",
                table: "CourseReports",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "CourseReports",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "MainReportId",
                table: "CourseReports",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MainReport__rId",
                table: "CourseReports",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "CourseReports",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "StudentId",
                table: "CourseReports",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "CourseReports",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "MainReports",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    StudentId = table.Column<string>(type: "text", nullable: false),
                    MonthId = table.Column<int>(type: "integer", nullable: false),
                    StudentName = table.Column<string>(type: "text", nullable: false),
                    StudentEmail = table.Column<string>(type: "text", nullable: false),
                    ParentCmt = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainReports", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseReports_MainReport__rId",
                table: "CourseReports",
                column: "MainReport__rId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseReports_MainReportId",
                table: "CourseReports",
                column: "MainReportId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseReports_MainReports_MainReportId",
                table: "CourseReports",
                column: "MainReportId",
                principalTable: "MainReports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseReports_MainReports_MainReport__rId",
                table: "CourseReports",
                column: "MainReport__rId",
                principalTable: "MainReports",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseReports_MainReports_MainReportId",
                table: "CourseReports");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseReports_MainReports_MainReport__rId",
                table: "CourseReports");

            migrationBuilder.DropTable(
                name: "MainReports");

            migrationBuilder.DropIndex(
                name: "IX_CourseReports_MainReport__rId",
                table: "CourseReports");

            migrationBuilder.DropIndex(
                name: "IX_CourseReports_MainReportId",
                table: "CourseReports");

            migrationBuilder.DropColumn(
                name: "isActive",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "StudentEmail",
                table: "Enrollments");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "CourseReports");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "CourseReports");

            migrationBuilder.DropColumn(
                name: "MainReportId",
                table: "CourseReports");

            migrationBuilder.DropColumn(
                name: "MainReport__rId",
                table: "CourseReports");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "CourseReports");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "CourseReports");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "CourseReports");
        }
    }
}
