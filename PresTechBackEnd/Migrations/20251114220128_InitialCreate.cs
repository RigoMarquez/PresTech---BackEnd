using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PresTechBackEnd.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TipoDocumentos",
                columns: table => new
                {
                    TipoDocumentoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoDocumentos", x => x.TipoDocumentoId);
                });

            migrationBuilder.CreateTable(
                name: "Personas",
                columns: table => new
                {
                    PersonaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoDocumentoID = table.Column<int>(type: "int", nullable: false),
                    Identificacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sexo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ciudad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Contraseña = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personas", x => x.PersonaId);
                    table.ForeignKey(
                        name: "FK_Personas_TipoDocumentos_TipoDocumentoID",
                        column: x => x.TipoDocumentoID,
                        principalTable: "TipoDocumentos",
                        principalColumn: "TipoDocumentoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Prestamistas",
                columns: table => new
                {
                    PrestamistaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prestamistas", x => x.PrestamistaId);
                    table.ForeignKey(
                        name: "FK_Prestamistas_Personas_PersonaId",
                        column: x => x.PersonaId,
                        principalTable: "Personas",
                        principalColumn: "PersonaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Prestatarios",
                columns: table => new
                {
                    PrestatarioId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prestatarios", x => x.PrestatarioId);
                    table.ForeignKey(
                        name: "FK_Prestatarios_Personas_PersonaId",
                        column: x => x.PersonaId,
                        principalTable: "Personas",
                        principalColumn: "PersonaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OfertasPrestamo",
                columns: table => new
                {
                    OfertaPrestamoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Categoria = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MontoMin = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MontoMax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Interes = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Cuotas = table.Column<int>(type: "int", nullable: false),
                    Frecuencia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrestamistaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfertasPrestamo", x => x.OfertaPrestamoId);
                    table.ForeignKey(
                        name: "FK_OfertasPrestamo_Prestamistas_PrestamistaId",
                        column: x => x.PrestamistaId,
                        principalTable: "Prestamistas",
                        principalColumn: "PrestamistaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Prestamos",
                columns: table => new
                {
                    PrestamoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrestatarioId = table.Column<int>(type: "int", nullable: false),
                    OfertaPrestamoId = table.Column<int>(type: "int", nullable: false),
                    SaldoRestante = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SaldoPrestamo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CuotasRestantes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaProxPago = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prestamos", x => x.PrestamoId);
                    table.ForeignKey(
                        name: "FK_Prestamos_OfertasPrestamo_OfertaPrestamoId",
                        column: x => x.OfertaPrestamoId,
                        principalTable: "OfertasPrestamo",
                        principalColumn: "OfertaPrestamoId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Prestamos_Prestatarios_PrestatarioId",
                        column: x => x.PrestatarioId,
                        principalTable: "Prestatarios",
                        principalColumn: "PrestatarioId",
                        onDelete: ReferentialAction.Restrict);
                });

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

            migrationBuilder.CreateTable(
                name: "Transacciones",
                columns: table => new
                {
                    TransaccionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrestamoId = table.Column<int>(type: "int", nullable: false),
                    Monto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FechaPago = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Imagen = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    TipoTransaccion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transacciones", x => x.TransaccionId);
                    table.ForeignKey(
                        name: "FK_Transacciones_Prestamos_PrestamoId",
                        column: x => x.PrestamoId,
                        principalTable: "Prestamos",
                        principalColumn: "PrestamoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "TipoDocumentos",
                columns: new[] { "TipoDocumentoId", "Nombre" },
                values: new object[,]
                {
                    { 1, "Cédula de Ciudadanía" },
                    { 2, "Cédula de Extranjería" },
                    { 3, "Pasaporte" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OfertasPrestamo_PrestamistaId",
                table: "OfertasPrestamo",
                column: "PrestamistaId");

            migrationBuilder.CreateIndex(
                name: "IX_Personas_TipoDocumentoID",
                table: "Personas",
                column: "TipoDocumentoID");

            migrationBuilder.CreateIndex(
                name: "IX_Prestamistas_PersonaId",
                table: "Prestamistas",
                column: "PersonaId");

            migrationBuilder.CreateIndex(
                name: "IX_Prestamos_OfertaPrestamoId",
                table: "Prestamos",
                column: "OfertaPrestamoId");

            migrationBuilder.CreateIndex(
                name: "IX_Prestamos_PrestatarioId",
                table: "Prestamos",
                column: "PrestatarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Prestatarios_PersonaId",
                table: "Prestatarios",
                column: "PersonaId");

            migrationBuilder.CreateIndex(
                name: "IX_Recordatorios_PrestamoId",
                table: "Recordatorios",
                column: "PrestamoId");

            migrationBuilder.CreateIndex(
                name: "IX_Transacciones_PrestamoId",
                table: "Transacciones",
                column: "PrestamoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Recordatorios");

            migrationBuilder.DropTable(
                name: "Transacciones");

            migrationBuilder.DropTable(
                name: "Prestamos");

            migrationBuilder.DropTable(
                name: "OfertasPrestamo");

            migrationBuilder.DropTable(
                name: "Prestatarios");

            migrationBuilder.DropTable(
                name: "Prestamistas");

            migrationBuilder.DropTable(
                name: "Personas");

            migrationBuilder.DropTable(
                name: "TipoDocumentos");
        }
    }
}
