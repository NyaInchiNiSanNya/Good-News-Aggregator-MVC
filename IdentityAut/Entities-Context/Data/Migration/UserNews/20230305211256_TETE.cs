using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities_Context.Data.Migration.UserNews
{
    /// <inheritdoc />
    public partial class TETE : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersNews_News_NewsId",
                table: "UsersNews");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersNews_Users_UserId",
                table: "UsersNews");

            migrationBuilder.DropIndex(
                name: "IX_UsersNews_UserId",
                table: "UsersNews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Resource",
                table: "News");

            migrationBuilder.DropColumn(
                name: "Tegs",
                table: "News");

            migrationBuilder.DropColumn(
                name: "Config",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "UserConfiguration");

            migrationBuilder.RenameColumn(
                name: "NewsId",
                table: "UsersNews",
                newName: "ArtincleId");

            migrationBuilder.RenameIndex(
                name: "IX_UsersNews_NewsId",
                table: "UsersNews",
                newName: "IX_UsersNews_ArtincleId");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "UserConfiguration",
                newName: "Theme");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserConfiguration",
                table: "UserConfiguration",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "UserInformation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserConfigId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInformation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserInformation_UserConfiguration_UserConfigId",
                        column: x => x.UserConfigId,
                        principalTable: "UserConfiguration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserInformation_UserConfigId",
                table: "UserInformation",
                column: "UserConfigId");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersNews_News_ArtincleId",
                table: "UsersNews",
                column: "ArtincleId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersNews_News_ArtincleId",
                table: "UsersNews");

            migrationBuilder.DropTable(
                name: "UserInformation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserConfiguration",
                table: "UserConfiguration");

            migrationBuilder.RenameTable(
                name: "UserConfiguration",
                newName: "Users");

            migrationBuilder.RenameColumn(
                name: "ArtincleId",
                table: "UsersNews",
                newName: "NewsId");

            migrationBuilder.RenameIndex(
                name: "IX_UsersNews_ArtincleId",
                table: "UsersNews",
                newName: "IX_UsersNews_NewsId");

            migrationBuilder.RenameColumn(
                name: "Theme",
                table: "Users",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "Resource",
                table: "News",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Tegs",
                table: "News",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Config",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UsersNews_UserId",
                table: "UsersNews",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersNews_News_NewsId",
                table: "UsersNews",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersNews_Users_UserId",
                table: "UsersNews",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
