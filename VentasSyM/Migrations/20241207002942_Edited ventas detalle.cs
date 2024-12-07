using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VentasSyM.Migrations
{
    /// <inheritdoc />
    public partial class Editedventasdetalle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_VentasDetalles_ProductoId",
                table: "VentasDetalles",
                column: "ProductoId");

            migrationBuilder.AddForeignKey(
                name: "FK_VentasDetalles_Productos_ProductoId",
                table: "VentasDetalles",
                column: "ProductoId",
                principalTable: "Productos",
                principalColumn: "ProductoId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VentasDetalles_Productos_ProductoId",
                table: "VentasDetalles");

            migrationBuilder.DropIndex(
                name: "IX_VentasDetalles_ProductoId",
                table: "VentasDetalles");
        }
    }
}
