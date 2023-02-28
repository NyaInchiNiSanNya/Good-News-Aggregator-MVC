using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities_Context.Data.Migration.UserNews
{
    /// <inheritdoc />
    public partial class Test : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    URL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Resource = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tegs = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Config = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsersNews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    NewsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersNews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersNews_News_NewsId",
                        column: x => x.NewsId,
                        principalTable: "News",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersNews_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsersNews_NewsId",
                table: "UsersNews",
                column: "NewsId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersNews_UserId",
                table: "UsersNews",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsersNews");

            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
