using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestrutura.Migrations
{
    /// <inheritdoc />
    public partial class CorrecaoTipos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Moto_Placa",
                table: "Moto");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Admins",
                table: "Admins");

            migrationBuilder.RenameTable(
                name: "Admins",
                newName: "Admin");

            migrationBuilder.AlterColumn<string>(
                name: "Placa",
                table: "Moto",
                type: "char(7)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar",
                oldMaxLength: 7);

            migrationBuilder.AlterColumn<string>(
                name: "Cnpj",
                table: "Entregador",
                type: "char(14)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar",
                oldMaxLength: 14);

            migrationBuilder.AlterColumn<string>(
                name: "CnhTipo",
                table: "Entregador",
                type: "char(5)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar",
                oldMaxLength: 5);

            migrationBuilder.AlterColumn<string>(
                name: "Cnh",
                table: "Entregador",
                type: "char(12)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar",
                oldMaxLength: 50);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Admin",
                table: "Admin",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Moto_Placa",
                table: "Moto",
                column: "Placa",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Moto_Placa",
                table: "Moto");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Admin",
                table: "Admin");

            migrationBuilder.RenameTable(
                name: "Admin",
                newName: "Admins");

            migrationBuilder.AlterColumn<string>(
                name: "Placa",
                table: "Moto",
                type: "varchar",
                maxLength: 7,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(7)");

            migrationBuilder.AlterColumn<string>(
                name: "Cnpj",
                table: "Entregador",
                type: "varchar",
                maxLength: 14,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(14)");

            migrationBuilder.AlterColumn<string>(
                name: "CnhTipo",
                table: "Entregador",
                type: "varchar",
                maxLength: 5,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(5)");

            migrationBuilder.AlterColumn<string>(
                name: "Cnh",
                table: "Entregador",
                type: "varchar",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(12)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Admins",
                table: "Admins",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Moto_Placa",
                table: "Moto",
                column: "Placa");
        }
    }
}
