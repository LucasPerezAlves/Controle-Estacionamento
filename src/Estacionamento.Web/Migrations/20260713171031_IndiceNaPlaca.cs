using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Estacionamento.Web.Migrations
{
    /// <inheritdoc />
    public partial class IndiceNaPlaca : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_RegistrosEstacionamento_Placa",
                table: "RegistrosEstacionamento",
                column: "Placa");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RegistrosEstacionamento_Placa",
                table: "RegistrosEstacionamento");
        }
    }
}
