using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace F1Fast.API.Migrations
{
    /// <inheritdoc />
    public partial class AddLembreteUrgenteEnviadoToEtapa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "LembreteUrgenteEnviado",
                table: "Etapas",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 1,
                column: "LembreteUrgenteEnviado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 2,
                column: "LembreteUrgenteEnviado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 3,
                column: "LembreteUrgenteEnviado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 4,
                column: "LembreteUrgenteEnviado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 5,
                column: "LembreteUrgenteEnviado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 6,
                column: "LembreteUrgenteEnviado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 7,
                column: "LembreteUrgenteEnviado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 8,
                column: "LembreteUrgenteEnviado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 9,
                column: "LembreteUrgenteEnviado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 10,
                column: "LembreteUrgenteEnviado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 11,
                column: "LembreteUrgenteEnviado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 12,
                column: "LembreteUrgenteEnviado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 13,
                column: "LembreteUrgenteEnviado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 14,
                column: "LembreteUrgenteEnviado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 15,
                column: "LembreteUrgenteEnviado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 16,
                column: "LembreteUrgenteEnviado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 17,
                column: "LembreteUrgenteEnviado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 18,
                column: "LembreteUrgenteEnviado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 19,
                column: "LembreteUrgenteEnviado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 20,
                column: "LembreteUrgenteEnviado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 21,
                column: "LembreteUrgenteEnviado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 22,
                column: "LembreteUrgenteEnviado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 23,
                column: "LembreteUrgenteEnviado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 24,
                column: "LembreteUrgenteEnviado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 25,
                column: "LembreteUrgenteEnviado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 26,
                column: "LembreteUrgenteEnviado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 27,
                column: "LembreteUrgenteEnviado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 28,
                column: "LembreteUrgenteEnviado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 29,
                column: "LembreteUrgenteEnviado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 30,
                column: "LembreteUrgenteEnviado",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LembreteUrgenteEnviado",
                table: "Etapas");
        }
    }
}
