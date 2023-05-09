using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities_Context.Migrations
{
    /// <inheritdoc />
    public partial class sdgvg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "Articles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
