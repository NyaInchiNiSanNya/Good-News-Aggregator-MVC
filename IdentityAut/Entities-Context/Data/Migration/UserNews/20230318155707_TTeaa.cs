using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities_Context.Data.Migration.UserNews
{
    /// <inheritdoc />
    public partial class TTeaa : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PositiveRateFilter",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PositiveRateFilter",
                table: "Users");
        }
    }
}
