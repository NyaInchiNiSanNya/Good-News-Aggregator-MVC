using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities_Context.Data.Migration.UserArticle
{
    /// <inheritdoc />
    public partial class PictureForArticles2 : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ArticlePicture",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArticlePicture",
                table: "Articles");
        }
    }
}
