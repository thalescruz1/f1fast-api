using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace F1Fast.API.Migrations
{
    /// <inheritdoc />
    public partial class AddCircuitoDataToEtapa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AnoRecord",
                table: "Etapas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CircuitoComprimento",
                table: "Etapas",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "CircuitoTipo",
                table: "Etapas",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Distancia",
                table: "Etapas",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Recordista",
                table: "Etapas",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "TempoRecord",
                table: "Etapas",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "Voltas",
                table: "Etapas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AnoRecord", "CircuitoComprimento", "CircuitoTipo", "Distancia", "Recordista", "TempoRecord", "Voltas" },
                values: new object[] { 2022, "5.278 km", "Circuito semi-permanente", "306.1 km", "Charles Leclerc", "1:19.813", 58 });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AnoRecord", "CircuitoComprimento", "CircuitoTipo", "Distancia", "Recordista", "TempoRecord", "Voltas" },
                values: new object[] { 2004, "5.451 km", "Circuito permanente", "103.6 km", "Michael Schumacher", "1:32.238", 19 });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "AnoRecord", "CircuitoComprimento", "CircuitoTipo", "Distancia", "Recordista", "TempoRecord", "Voltas" },
                values: new object[] { 2004, "5.451 km", "Circuito permanente", "305.1 km", "Michael Schumacher", "1:32.238", 56 });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "AnoRecord", "CircuitoComprimento", "CircuitoTipo", "Distancia", "Recordista", "TempoRecord", "Voltas" },
                values: new object[] { 2019, "5.807 km", "Circuito permanente", "307.5 km", "Lewis Hamilton", "1:30.983", 53 });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "AnoRecord", "CircuitoComprimento", "CircuitoTipo", "Distancia", "Recordista", "TempoRecord", "Voltas" },
                values: new object[] { 2005, "5.412 km", "Circuito permanente (iluminado)", "308.2 km", "Pedro de la Rosa", "1:30.252", 57 });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "AnoRecord", "CircuitoComprimento", "CircuitoTipo", "Distancia", "Recordista", "TempoRecord", "Voltas" },
                values: new object[] { 2023, "6.174 km", "Circuito de rua", "308.5 km", "Sergio Perez", "1:30.734", 50 });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "AnoRecord", "CircuitoComprimento", "CircuitoTipo", "Distancia", "Recordista", "TempoRecord", "Voltas" },
                values: new object[] { 2023, "5.412 km", "Circuito semi-permanente", "97.4 km", "Max Verstappen", "1:27.274", 18 });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "AnoRecord", "CircuitoComprimento", "CircuitoTipo", "Distancia", "Recordista", "TempoRecord", "Voltas" },
                values: new object[] { 2023, "5.412 km", "Circuito semi-permanente", "308.3 km", "Max Verstappen", "1:27.274", 57 });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "AnoRecord", "CircuitoComprimento", "CircuitoTipo", "Distancia", "Recordista", "TempoRecord", "Voltas" },
                values: new object[] { 2019, "4.361 km", "Circuito semi-permanente", "100.3 km", "Valtteri Bottas", "1:13.078", 23 });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "AnoRecord", "CircuitoComprimento", "CircuitoTipo", "Distancia", "Recordista", "TempoRecord", "Voltas" },
                values: new object[] { 2019, "4.361 km", "Circuito semi-permanente", "305.3 km", "Valtteri Bottas", "1:13.078", 70 });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "AnoRecord", "CircuitoComprimento", "CircuitoTipo", "Distancia", "Recordista", "TempoRecord", "Voltas" },
                values: new object[] { 2021, "3.337 km", "Circuito de rua", "260.3 km", "Charles Leclerc", "1:12.909", 78 });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "AnoRecord", "CircuitoComprimento", "CircuitoTipo", "Distancia", "Recordista", "TempoRecord", "Voltas" },
                values: new object[] { 2023, "4.657 km", "Circuito permanente", "307.3 km", "Max Verstappen", "1:16.330", 66 });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "AnoRecord", "CircuitoComprimento", "CircuitoTipo", "Distancia", "Recordista", "TempoRecord", "Voltas" },
                values: new object[] { 2020, "4.318 km", "Circuito permanente", "306.5 km", "Carlos Sainz", "1:05.619", 71 });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "AnoRecord", "CircuitoComprimento", "CircuitoTipo", "Distancia", "Recordista", "TempoRecord", "Voltas" },
                values: new object[] { 2020, "5.891 km", "Circuito permanente", "100.1 km", "Lewis Hamilton", "1:24.303", 17 });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "AnoRecord", "CircuitoComprimento", "CircuitoTipo", "Distancia", "Recordista", "TempoRecord", "Voltas" },
                values: new object[] { 2020, "5.891 km", "Circuito permanente", "306.2 km", "Lewis Hamilton", "1:24.303", 52 });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "AnoRecord", "CircuitoComprimento", "CircuitoTipo", "Distancia", "Recordista", "TempoRecord", "Voltas" },
                values: new object[] { 2018, "7.004 km", "Circuito permanente", "308.1 km", "Valtteri Bottas", "1:46.286", 44 });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "AnoRecord", "CircuitoComprimento", "CircuitoTipo", "Distancia", "Recordista", "TempoRecord", "Voltas" },
                values: new object[] { 2020, "4.381 km", "Circuito permanente", "306.6 km", "Lewis Hamilton", "1:16.627", 70 });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "AnoRecord", "CircuitoComprimento", "CircuitoTipo", "Distancia", "Recordista", "TempoRecord", "Voltas" },
                values: new object[] { 2021, "4.259 km", "Circuito permanente", "102.2 km", "Max Verstappen", "1:11.097", 24 });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "AnoRecord", "CircuitoComprimento", "CircuitoTipo", "Distancia", "Recordista", "TempoRecord", "Voltas" },
                values: new object[] { 2021, "4.259 km", "Circuito permanente", "306.6 km", "Max Verstappen", "1:11.097", 72 });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "AnoRecord", "CircuitoComprimento", "CircuitoTipo", "Distancia", "Recordista", "TempoRecord", "Voltas" },
                values: new object[] { 2004, "5.793 km", "Circuito permanente (alta velocidade)", "306.7 km", "Rubens Barrichello", "1:21.046", 53 });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "AnoRecord", "CircuitoComprimento", "CircuitoTipo", "Distancia", "Recordista", "TempoRecord", "Voltas" },
                values: new object[] { 2026, "5.474 km", "Circuito semi-permanente", "306.5 km", "—", "—", 56 });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "AnoRecord", "CircuitoComprimento", "CircuitoTipo", "Distancia", "Recordista", "TempoRecord", "Voltas" },
                values: new object[] { 2019, "6.003 km", "Circuito de rua", "306.0 km", "Charles Leclerc", "1:43.009", 51 });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "AnoRecord", "CircuitoComprimento", "CircuitoTipo", "Distancia", "Recordista", "TempoRecord", "Voltas" },
                values: new object[] { 2023, "4.940 km", "Circuito de rua (noturno)", "103.7 km", "Lewis Hamilton", "1:35.867", 21 });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "AnoRecord", "CircuitoComprimento", "CircuitoTipo", "Distancia", "Recordista", "TempoRecord", "Voltas" },
                values: new object[] { 2023, "4.940 km", "Circuito de rua (noturno)", "306.1 km", "Lewis Hamilton", "1:35.867", 62 });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "AnoRecord", "CircuitoComprimento", "CircuitoTipo", "Distancia", "Recordista", "TempoRecord", "Voltas" },
                values: new object[] { 2023, "5.513 km", "Circuito permanente", "308.4 km", "Charles Leclerc", "1:36.169", 56 });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "AnoRecord", "CircuitoComprimento", "CircuitoTipo", "Distancia", "Recordista", "TempoRecord", "Voltas" },
                values: new object[] { 2021, "4.304 km", "Circuito permanente (alta altitude)", "305.4 km", "Valtteri Bottas", "1:17.774", 71 });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "AnoRecord", "CircuitoComprimento", "CircuitoTipo", "Distancia", "Recordista", "TempoRecord", "Voltas" },
                values: new object[] { 2018, "4.309 km", "Circuito permanente", "305.9 km", "Valtteri Bottas", "1:10.540", 71 });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "AnoRecord", "CircuitoComprimento", "CircuitoTipo", "Distancia", "Recordista", "TempoRecord", "Voltas" },
                values: new object[] { 2023, "6.201 km", "Circuito de rua (noturno)", "310.0 km", "Max Verstappen", "1:35.490", 50 });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "AnoRecord", "CircuitoComprimento", "CircuitoTipo", "Distancia", "Recordista", "TempoRecord", "Voltas" },
                values: new object[] { 2023, "5.380 km", "Circuito permanente (noturno)", "306.6 km", "Charles Leclerc", "1:24.319", 57 });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "AnoRecord", "CircuitoComprimento", "CircuitoTipo", "Distancia", "Recordista", "TempoRecord", "Voltas" },
                values: new object[] { 2021, "5.281 km", "Circuito permanente", "306.2 km", "Max Verstappen", "1:26.103", 58 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnoRecord",
                table: "Etapas");

            migrationBuilder.DropColumn(
                name: "CircuitoComprimento",
                table: "Etapas");

            migrationBuilder.DropColumn(
                name: "CircuitoTipo",
                table: "Etapas");

            migrationBuilder.DropColumn(
                name: "Distancia",
                table: "Etapas");

            migrationBuilder.DropColumn(
                name: "Recordista",
                table: "Etapas");

            migrationBuilder.DropColumn(
                name: "TempoRecord",
                table: "Etapas");

            migrationBuilder.DropColumn(
                name: "Voltas",
                table: "Etapas");
        }
    }
}
