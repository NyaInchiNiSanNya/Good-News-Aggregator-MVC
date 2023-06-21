using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities_Context.Data.Migration.UserArticle
{
    /// <inheritdoc />
    public partial class asd : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.AddColumn<double>(
                name: "AIrate",
                table: "Articles",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "lemmasRate",
                table: "Articles",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AIrate",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "lemmasRate",
                table: "Articles");

           
        }
    }
}
