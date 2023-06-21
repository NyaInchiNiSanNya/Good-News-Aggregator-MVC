using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities_Context.Data.Migration.UserArticle
{
    /// <inheritdoc />
    public partial class asddfg : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "lemmasRate",
                table: "Articles",
                newName: "SecondRate");

            migrationBuilder.RenameColumn(
                name: "AIrate",
                table: "Articles",
                newName: "FirstRate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SecondRate",
                table: "Articles",
                newName: "lemmasRate");

            migrationBuilder.RenameColumn(
                name: "FirstRate",
                table: "Articles",
                newName: "AIrate");
        }
    }
}
