using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VentasSyM.Migrations
{
    /// <inheritdoc />
    public partial class Actualizacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Categoria",
                table: "Productos");

            migrationBuilder.AddColumn<int>(
                name: "CategoriaId",
                table: "Productos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoriaId",
                table: "Productos");

            migrationBuilder.AddColumn<string>(
                name: "Categoria",
                table: "Productos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
