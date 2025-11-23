using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PresTechBackEnd.Migrations
{
    /// <inheritdoc />
    public partial class AddDisponibleToOferta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Disponible",
                table: "OfertasPrestamo",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Disponible",
                table: "OfertasPrestamo");
        }
    }
}
