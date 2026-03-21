using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace F1Fast.API.Migrations
{
    /// <inheritdoc />
    public partial class AddCanceladaToEtapa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Cancelada",
                table: "Etapas",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 1,
                column: "Cancelada",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 2,
                column: "Cancelada",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 3,
                column: "Cancelada",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 4,
                column: "Cancelada",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 5,
                column: "Cancelada",
                value: true);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 6,
                column: "Cancelada",
                value: true);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 7,
                column: "Cancelada",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 8,
                column: "Cancelada",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 9,
                column: "Cancelada",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 10,
                column: "Cancelada",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 11,
                column: "Cancelada",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 12,
                column: "Cancelada",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 13,
                column: "Cancelada",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 14,
                column: "Cancelada",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 15,
                column: "Cancelada",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 16,
                column: "Cancelada",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 17,
                column: "Cancelada",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 18,
                column: "Cancelada",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 19,
                column: "Cancelada",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 20,
                column: "Cancelada",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 21,
                column: "Cancelada",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 22,
                column: "Cancelada",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 23,
                column: "Cancelada",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 24,
                column: "Cancelada",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 25,
                column: "Cancelada",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 26,
                column: "Cancelada",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 27,
                column: "Cancelada",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 28,
                column: "Cancelada",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 29,
                column: "Cancelada",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 30,
                column: "Cancelada",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cancelada",
                table: "Etapas");
        }
    }
}
