using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniqloFromJapan.Migrations
{
    public partial class AddedAboutUs3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Text",
                table: "AboutUsModels",
                newName: "Title");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AboutUsModels",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "AboutUsModels");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "AboutUsModels",
                newName: "Text");
        }
    }
}
