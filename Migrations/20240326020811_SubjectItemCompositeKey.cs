using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eRe.Migrations
{
    /// <inheritdoc />
    public partial class SubjectItemCompositeKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SubjectItems_SubjectId",
                table: "SubjectItems");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_SubjectItems_SubjectId_ClassroomId",
                table: "SubjectItems",
                columns: new[] { "SubjectId", "ClassroomId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_SubjectItems_SubjectId_ClassroomId",
                table: "SubjectItems");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectItems_SubjectId",
                table: "SubjectItems",
                column: "SubjectId");
        }
    }
}
