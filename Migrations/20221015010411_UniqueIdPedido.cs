using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LojaVirtual.Migrations
{
    public partial class UniqueIdPedido : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UniqueId",
                table: "Faturas");

            migrationBuilder.AddColumn<string>(
                name: "token",
                table: "Pedidos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "token",
                table: "Pedidos");

            migrationBuilder.AddColumn<string>(
                name: "UniqueId",
                table: "Faturas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
