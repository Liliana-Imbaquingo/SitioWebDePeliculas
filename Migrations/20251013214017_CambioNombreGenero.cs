using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SitioWebDePeliculas.Migrations
{
    /// <inheritdoc />
    public partial class CambioNombreGenero : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Generos",
                newName: "Nombre");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Nombre",
                table: "Generos",
                newName: "Name");
        }
    }
}
