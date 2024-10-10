using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository_DAL_.Migrations
{
    /// <inheritdoc />
    public partial class SchemaChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "prodUrl",
                table: "controlmaster",
                newName: "produrl");

            migrationBuilder.RenameColumn(
                name: "devUrl",
                table: "controlmaster",
                newName: "devurl");

            migrationBuilder.AddColumn<string>(
                name: "confirmationstatus",
                table: "orders",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "paymentmethod",
                table: "orders",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "transactionref",
                table: "orders",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "confirmationstatus",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "paymentmethod",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "transactionref",
                table: "orders");

            migrationBuilder.RenameColumn(
                name: "produrl",
                table: "controlmaster",
                newName: "prodUrl");

            migrationBuilder.RenameColumn(
                name: "devurl",
                table: "controlmaster",
                newName: "devUrl");
        }
    }
}
