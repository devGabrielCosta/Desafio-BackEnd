using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestrutura.Migrations
{
    /// <inheritdoc />
    public partial class LocacaoIncluded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Disponivel",
                table: "Moto",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Locacao",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Plano = table.Column<string>(type: "varchar", nullable: false),
                    EntregadorId = table.Column<Guid>(type: "uuid", nullable: false),
                    MotoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Inicio = table.Column<DateTime>(type: "Date", nullable: false),
                    Termino = table.Column<DateTime>(type: "Date", nullable: false),
                    PrevisaoDevolucao = table.Column<DateTime>(type: "Date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locacao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Locacao_Entregador_EntregadorId",
                        column: x => x.EntregadorId,
                        principalTable: "Entregador",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Locacao_Moto_MotoId",
                        column: x => x.MotoId,
                        principalTable: "Moto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Locacao_EntregadorId",
                table: "Locacao",
                column: "EntregadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Locacao_MotoId",
                table: "Locacao",
                column: "MotoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Locacao");

            migrationBuilder.DropColumn(
                name: "Disponivel",
                table: "Moto");
        }
    }
}
