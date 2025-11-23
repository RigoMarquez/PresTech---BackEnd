using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PresTechBackEnd.Migrations
{
    /// <inheritdoc />
    public partial class EliminarCampoImagen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Imagen",
                table: "Transacciones");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Imagen",
                table: "Transacciones",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
