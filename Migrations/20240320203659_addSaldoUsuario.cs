using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dotnet_banco_digital.Migrations
{
    /// <inheritdoc />
    public partial class addSaldoUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Saldo",
                table: "Usuarios",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Saldo",
                table: "Usuarios");
        }
    }
}
