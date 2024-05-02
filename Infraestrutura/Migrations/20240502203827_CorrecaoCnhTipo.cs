using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestrutura.Migrations
{
    /// <inheritdoc />
    public partial class CorrecaoCnhTipo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CnhTipo",
                table: "Entregador",
                type: "varchar",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(5)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CnhTipo",
                table: "Entregador",
                type: "char(5)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar");
        }
    }
}
