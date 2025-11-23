using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PresTechBackEnd.Migrations
{
    /// <inheritdoc />
    public partial class ElimineTablaX : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Recordatorios");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Recordatorios",
                columns: table => new
                {
                    RecordatorioId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrestamoId = table.Column<int>(type: "int", nullable: false),
                    FechaRecordatorio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Mensaje = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recordatorios", x => x.RecordatorioId);
                    table.ForeignKey(
                        name: "FK_Recordatorios_Prestamos_PrestamoId",
                        column: x => x.PrestamoId,
                        principalTable: "Prestamos",
                        principalColumn: "PrestamoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Recordatorios_PrestamoId",
                table: "Recordatorios",
                column: "PrestamoId");
        }
    }
}
