using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository_DAL_.Migrations
{
    /// <inheritdoc />
    public partial class controlmasterchanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "defaultSearchImg",
                table: "ControlMaster",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "defaultSearchImg",
                table: "ControlMaster",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}
