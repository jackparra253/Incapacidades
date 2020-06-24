using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Incapacidades",
                columns: table => new
                {
                    IncapacidadId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdEmpleado = table.Column<int>(nullable: false),
                    TipoIncapacidad = table.Column<byte>(nullable: false),
                    FechaIncial = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    FechaFinal = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    CantidadDias = table.Column<int>(nullable: false),
                    Observaciones = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incapacidades", x => x.IncapacidadId);
                });

            migrationBuilder.CreateTable(
                name: "ReconocimientosEconomicos",
                columns: table => new
                {
                    ReconocimientoEconomicoId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdEmpleado = table.Column<int>(nullable: false),
                    FechaInicial = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    FechaFinal = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    DineroCantidad = table.Column<decimal>(type: "decimal(5, 2)", nullable: true),
                    DineroMoneda = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: true),
                    ResponsablePago = table.Column<byte>(nullable: false),
                    IncapacidadId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReconocimientosEconomicos", x => x.ReconocimientoEconomicoId);
                    table.ForeignKey(
                        name: "FK_ReconocimientosEconomicos_Incapacidades_IncapacidadId",
                        column: x => x.IncapacidadId,
                        principalTable: "Incapacidades",
                        principalColumn: "IncapacidadId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReconocimientosEconomicos_IncapacidadId",
                table: "ReconocimientosEconomicos",
                column: "IncapacidadId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReconocimientosEconomicos");

            migrationBuilder.DropTable(
                name: "Incapacidades");
        }
    }
}
