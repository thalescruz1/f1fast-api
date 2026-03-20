using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace F1Fast.API.Migrations
{
    /// <inheritdoc />
    public partial class AddLembreteEnviadoToEtapa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "LembreteEnviado",
                table: "Etapas",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 1,
                column: "LembreteEnviado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 2,
                column: "LembreteEnviado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 3,
                column: "LembreteEnviado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 4,
                column: "LembreteEnviado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 5,
                column: "LembreteEnviado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 6,
                column: "LembreteEnviado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 7,
                column: "LembreteEnviado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 8,
                column: "LembreteEnviado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 9,
                column: "LembreteEnviado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 10,
                column: "LembreteEnviado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 11,
                column: "LembreteEnviado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 12,
                column: "LembreteEnviado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 13,
                column: "LembreteEnviado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 14,
                column: "LembreteEnviado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 15,
                column: "LembreteEnviado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 16,
                column: "LembreteEnviado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 17,
                column: "LembreteEnviado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 18,
                column: "LembreteEnviado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 19,
                column: "LembreteEnviado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 20,
                column: "LembreteEnviado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 21,
                column: "LembreteEnviado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 22,
                column: "LembreteEnviado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 23,
                column: "LembreteEnviado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 24,
                column: "LembreteEnviado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 25,
                column: "LembreteEnviado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 26,
                column: "LembreteEnviado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 27,
                column: "LembreteEnviado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 28,
                column: "LembreteEnviado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 29,
                column: "LembreteEnviado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 30,
                column: "LembreteEnviado",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LembreteEnviado",
                table: "Etapas");
        }
    }
}
