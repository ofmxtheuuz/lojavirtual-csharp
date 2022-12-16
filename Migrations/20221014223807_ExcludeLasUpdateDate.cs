using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LojaVirtual.Migrations
{
    public partial class ExcludeLasUpdateDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.DropColumn(
                name: "LastUpdatedDate",
                table: "Faturas");


            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "LastUpdatedDate",
                table: "Faturas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

        }
    }
}
