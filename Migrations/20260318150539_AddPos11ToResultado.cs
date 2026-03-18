using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace F1Fast.API.Migrations
{
    /// <inheritdoc />
    public partial class AddPos11ToResultado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Pos11Id",
                table: "Resultados",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pos11Id",
                table: "Resultados");
        }
    }
}
