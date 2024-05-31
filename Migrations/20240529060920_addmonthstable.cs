using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eRe.Migrations
{
    /// <inheritdoc />
    public partial class addmonthstable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Month",
                table: "Month");

            migrationBuilder.RenameTable(
                name: "Month",
                newName: "Months");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Months",
                table: "Months",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Months",
                table: "Months");

            migrationBuilder.RenameTable(
                name: "Months",
                newName: "Month");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Month",
                table: "Month",
                column: "Id");
        }
    }
}
