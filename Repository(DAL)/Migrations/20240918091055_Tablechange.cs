using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository_DAL_.Migrations
{
    /// <inheritdoc />
    public partial class Tablechange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Label_Description",
                table: "Labels",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "defaultSearchImg",
                table: "ControlMaster",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "defaultSearchImg",
                table: "ControlMaster");

            migrationBuilder.AlterColumn<int>(
                name: "Label_Description",
                table: "Labels",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
