using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestrutura.Migrations
{
    /// <inheritdoc />
    public partial class EntregadorIncluded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Entregador",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "varchar", nullable: false),
                    Cnpj = table.Column<string>(type: "varchar", maxLength: 14, nullable: false),
                    DataNascimento = table.Column<DateTime>(type: "Date", nullable: false),
                    Cnh = table.Column<string>(type: "varchar", maxLength: 50, nullable: false),
                    CnhTipo = table.Column<string>(type: "varchar", maxLength: 5, nullable: false),
                    CnhImagem = table.Column<string>(type: "varchar", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entregador", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Entregador_Cnh",
                table: "Entregador",
                column: "Cnh",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Entregador_Cnpj",
                table: "Entregador",
                column: "Cnpj",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Entregador");
        }
    }
}
