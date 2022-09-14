using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopping.Data.Migrations
{
    public partial class ChangesInCart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "carts",
                newName: "CartId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "carts",
                newName: "RecordId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CartId",
                table: "carts",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "RecordId",
                table: "carts",
                newName: "Id");
        }
    }
}
