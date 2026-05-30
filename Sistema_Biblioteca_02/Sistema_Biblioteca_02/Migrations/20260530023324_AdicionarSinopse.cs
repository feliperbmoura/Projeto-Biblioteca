using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sistema_Biblioteca_02.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarSinopse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Sinopse",
                table: "Livros",
                type: "TEXT",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sinopse",
                table: "Livros");
        }
    }
}
