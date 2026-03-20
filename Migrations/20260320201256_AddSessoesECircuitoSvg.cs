using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace F1Fast.API.Migrations
{
    /// <inheritdoc />
    public partial class AddSessoesECircuitoSvg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CircuitoSvg",
                table: "Etapas",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "Classificacao",
                table: "Etapas",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TreinoLivre1",
                table: "Etapas",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TreinoLivre2",
                table: "Etapas",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TreinoLivre3",
                table: "Etapas",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CircuitoSvg", "Classificacao", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { "<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M50 160 Q30 140 30 100 Q30 50 60 30 Q90 10 140 15 L200 20 Q240 25 260 50 Q280 80 270 120 Q260 150 230 165 L180 175 Q160 170 150 155 Q140 140 130 145 L100 160 Z\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"50\" cy=\"160\" r=\"6\" fill=\"#E8001A\"/></svg>", new DateTime(2026, 3, 7, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 6, 1, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 6, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 7, 1, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CircuitoSvg", "Classificacao", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { "<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M40 180 L40 100 Q40 60 70 40 L130 20 Q160 10 180 30 Q200 50 180 70 Q160 90 180 110 Q200 130 240 120 L270 100 Q290 80 280 50 L270 30 Q260 20 240 25 L200 40\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"40\" cy=\"180\" r=\"6\" fill=\"#E8001A\"/></svg>", new DateTime(2026, 3, 12, 8, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 12, 5, 0, 0, 0, DateTimeKind.Utc), null, null });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CircuitoSvg", "Classificacao", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { "<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M40 180 L40 100 Q40 60 70 40 L130 20 Q160 10 180 30 Q200 50 180 70 Q160 90 180 110 Q200 130 240 120 L270 100 Q290 80 280 50 L270 30 Q260 20 240 25 L200 40\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"40\" cy=\"180\" r=\"6\" fill=\"#E8001A\"/></svg>", new DateTime(2026, 3, 13, 8, 30, 0, 0, DateTimeKind.Utc), null, null, null });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CircuitoSvg", "Classificacao", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { "<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M60 170 Q30 150 20 120 Q10 80 40 50 Q70 20 120 20 L180 25 Q220 30 240 60 Q250 80 230 100 Q210 115 220 135 Q230 155 210 170 Q180 190 140 180 Q100 170 80 140 Q65 120 80 100 Q100 80 130 90 Q150 100 140 120\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"60\" cy=\"170\" r=\"6\" fill=\"#E8001A\"/></svg>", new DateTime(2026, 3, 27, 7, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 26, 3, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 26, 7, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 27, 3, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CircuitoSvg", "Classificacao", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { "<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M70 180 L70 140 Q70 120 90 110 L130 90 Q150 80 150 60 Q150 40 130 35 L90 30 Q70 30 60 45 L50 70 Q45 90 60 100 L100 120 Q120 130 120 150 L120 170 Q120 185 140 185 L200 180 Q230 175 250 150 L260 120 Q265 90 250 70 L230 50 Q210 35 190 40 L170 50\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"70\" cy=\"180\" r=\"6\" fill=\"#E8001A\"/></svg>", new DateTime(2026, 4, 10, 16, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 9, 12, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 9, 16, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 10, 12, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CircuitoSvg", "Classificacao", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { "<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M40 190 L40 140 Q40 110 55 90 Q70 70 90 65 L140 55 Q165 50 175 35 Q185 20 210 20 L250 25 Q275 30 280 55 L280 100 Q280 130 260 150 Q240 170 210 175 L160 180 Q130 180 115 165 Q100 150 85 155 L60 170 Q45 180 40 190 Z\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"40\" cy=\"190\" r=\"6\" fill=\"#E8001A\"/></svg>", new DateTime(2026, 4, 17, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 16, 14, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 16, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 17, 14, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CircuitoSvg", "Classificacao", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { "<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M50 170 L50 50 Q50 30 70 30 L240 30 Q260 30 260 50 L260 110 Q260 130 240 140 L150 145 Q130 145 125 160 Q120 175 100 180 L70 180 Q50 180 50 170 Z\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"50\" cy=\"170\" r=\"6\" fill=\"#E8001A\"/></svg>", new DateTime(2026, 4, 30, 21, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 30, 18, 0, 0, 0, DateTimeKind.Utc), null, null });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CircuitoSvg", "Classificacao", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { "<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M50 170 L50 50 Q50 30 70 30 L240 30 Q260 30 260 50 L260 110 Q260 130 240 140 L150 145 Q130 145 125 160 Q120 175 100 180 L70 180 Q50 180 50 170 Z\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"50\" cy=\"170\" r=\"6\" fill=\"#E8001A\"/></svg>", new DateTime(2026, 5, 1, 21, 30, 0, 0, DateTimeKind.Utc), null, null, null });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CircuitoSvg", "Classificacao", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { "<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M60 180 L50 120 Q45 90 60 70 L100 40 Q120 25 150 25 L200 30 Q230 35 245 55 Q260 80 250 110 L230 140 Q215 160 190 170 L150 180 Q120 185 100 170 Q85 155 90 135 L100 110 Q110 90 130 85\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"60\" cy=\"180\" r=\"6\" fill=\"#E8001A\"/></svg>", new DateTime(2026, 5, 21, 21, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 5, 21, 18, 0, 0, 0, DateTimeKind.Utc), null, null });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CircuitoSvg", "Classificacao", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { "<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M60 180 L50 120 Q45 90 60 70 L100 40 Q120 25 150 25 L200 30 Q230 35 245 55 Q260 80 250 110 L230 140 Q215 160 190 170 L150 180 Q120 185 100 170 Q85 155 90 135 L100 110 Q110 90 130 85\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"60\" cy=\"180\" r=\"6\" fill=\"#E8001A\"/></svg>", new DateTime(2026, 5, 22, 21, 30, 0, 0, DateTimeKind.Utc), null, null, null });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CircuitoSvg", "Classificacao", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { "<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M80 180 L60 150 Q40 120 50 90 Q60 60 90 50 L130 40 Q155 35 165 55 Q175 75 160 90 L140 105 Q125 115 130 135 Q135 155 160 160 L200 155 Q230 145 245 120 Q260 95 250 70 L235 50 Q220 35 195 35\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"80\" cy=\"180\" r=\"6\" fill=\"#E8001A\"/></svg>", new DateTime(2026, 6, 5, 15, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 4, 11, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 4, 15, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 5, 11, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CircuitoSvg", "Classificacao", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { "<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M50 160 L50 80 Q50 50 80 40 L200 30 Q240 28 260 50 Q280 75 270 100 L250 130 Q235 150 210 155 L160 160 Q140 160 130 145 Q120 130 100 135 L70 150 Q55 158 50 160 Z\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"50\" cy=\"160\" r=\"6\" fill=\"#E8001A\"/></svg>", new DateTime(2026, 6, 12, 15, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 11, 11, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 11, 15, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 12, 11, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "CircuitoSvg", "Classificacao", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { "<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M80 180 L60 140 Q50 110 70 80 L120 40 Q145 20 175 30 L220 55 Q245 70 250 100 L245 140 Q240 165 210 175 L150 185 Q110 188 80 180 Z\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"80\" cy=\"180\" r=\"6\" fill=\"#E8001A\"/></svg>", new DateTime(2026, 6, 26, 15, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 25, 11, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 25, 15, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 26, 11, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "CircuitoSvg", "Classificacao", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { "<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M60 170 L40 130 Q25 100 45 70 Q65 45 100 35 L150 25 Q190 20 220 40 Q245 55 255 80 L260 110 Q262 140 240 160 Q215 178 180 180 L140 175 Q115 168 105 145 Q95 125 110 110 Q130 95 150 105 Q165 115 155 135\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"60\" cy=\"170\" r=\"6\" fill=\"#E8001A\"/></svg>", new DateTime(2026, 7, 2, 16, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 2, 13, 0, 0, 0, DateTimeKind.Utc), null, null });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "CircuitoSvg", "Classificacao", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { "<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M60 170 L40 130 Q25 100 45 70 Q65 45 100 35 L150 25 Q190 20 220 40 Q245 55 255 80 L260 110 Q262 140 240 160 Q215 178 180 180 L140 175 Q115 168 105 145 Q95 125 110 110 Q130 95 150 105 Q165 115 155 135\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"60\" cy=\"170\" r=\"6\" fill=\"#E8001A\"/></svg>", new DateTime(2026, 7, 3, 16, 30, 0, 0, DateTimeKind.Utc), null, null, null });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "CircuitoSvg", "Classificacao", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { "<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M40 170 L30 130 Q20 100 35 70 Q50 40 85 30 L130 20 Q160 15 175 35 Q190 55 175 80 L155 110 Q140 135 155 155 Q170 175 200 170 L240 155 Q270 140 280 110 L285 70 Q285 40 260 25\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"40\" cy=\"170\" r=\"6\" fill=\"#E8001A\"/></svg>", new DateTime(2026, 7, 17, 15, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 16, 11, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 16, 15, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 17, 11, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "CircuitoSvg", "Classificacao", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { "<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M70 180 L50 150 Q30 120 40 85 Q50 55 80 40 L130 25 Q170 15 200 35 Q225 50 230 80 L228 120 Q225 150 200 168 L160 180 Q130 188 110 175 Q90 160 95 135 Q100 115 120 110 Q145 105 155 120\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"70\" cy=\"180\" r=\"6\" fill=\"#E8001A\"/></svg>", new DateTime(2026, 7, 24, 15, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 23, 11, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 23, 15, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 24, 11, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "CircuitoSvg", "Classificacao", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { "<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M60 170 L45 130 Q30 90 50 55 Q75 25 120 20 L190 22 Q230 25 255 55 Q275 80 265 115 Q255 145 225 165 L170 180 Q130 188 100 175 Q75 160 80 130 Q85 110 110 105\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"60\" cy=\"170\" r=\"6\" fill=\"#E8001A\"/></svg>", new DateTime(2026, 8, 20, 15, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 20, 12, 0, 0, 0, DateTimeKind.Utc), null, null });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "CircuitoSvg", "Classificacao", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { "<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M60 170 L45 130 Q30 90 50 55 Q75 25 120 20 L190 22 Q230 25 255 55 Q275 80 265 115 Q255 145 225 165 L170 180 Q130 188 100 175 Q75 160 80 130 Q85 110 110 105\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"60\" cy=\"170\" r=\"6\" fill=\"#E8001A\"/></svg>", new DateTime(2026, 8, 21, 15, 30, 0, 0, DateTimeKind.Utc), null, null, null });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "CircuitoSvg", "Classificacao", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { "<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M70 180 L60 100 Q55 60 80 35 L150 20 Q200 15 230 40 Q255 60 250 95 L240 140 Q230 170 195 180 L140 185 Q100 185 85 165 Q75 145 90 125 L120 100 Q140 85 165 90\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"70\" cy=\"180\" r=\"6\" fill=\"#E8001A\"/></svg>", new DateTime(2026, 9, 4, 15, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 3, 11, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 3, 15, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 4, 11, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "CircuitoSvg", "Classificacao", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { "<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M50 170 L40 110 Q35 70 65 45 L120 25 Q160 15 200 30 Q235 45 250 75 L258 115 Q262 150 235 172 L185 188 Q145 195 110 180 Q80 165 75 135 L78 105 Q82 85 105 80\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"50\" cy=\"170\" r=\"6\" fill=\"#E8001A\"/></svg>", new DateTime(2026, 9, 11, 15, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 10, 11, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 10, 15, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 11, 11, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "CircuitoSvg", "Classificacao", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { "<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M50 190 L50 60 Q50 30 80 25 L120 20 Q150 18 160 40 L165 80 Q165 110 140 120 L100 130 Q75 135 75 155 L80 175 Q90 190 120 188 L220 180 Q260 175 275 150 L280 100 Q280 60 260 40\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"50\" cy=\"190\" r=\"6\" fill=\"#E8001A\"/></svg>", new DateTime(2026, 9, 24, 13, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 23, 9, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 23, 13, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 24, 9, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "CircuitoSvg", "Classificacao", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { "<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M60 180 L40 140 Q25 110 40 80 Q55 55 85 45 L130 35 Q160 30 180 45 Q200 60 195 85 L185 110 Q175 130 190 148 Q205 165 235 160 L260 150 Q280 135 278 110 L272 75 Q265 50 240 38 L200 28\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"60\" cy=\"180\" r=\"6\" fill=\"#E8001A\"/></svg>", new DateTime(2026, 10, 8, 13, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 8, 10, 0, 0, 0, DateTimeKind.Utc), null, null });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "CircuitoSvg", "Classificacao", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { "<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M60 180 L40 140 Q25 110 40 80 Q55 55 85 45 L130 35 Q160 30 180 45 Q200 60 195 85 L185 110 Q175 130 190 148 Q205 165 235 160 L260 150 Q280 135 278 110 L272 75 Q265 50 240 38 L200 28\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"60\" cy=\"180\" r=\"6\" fill=\"#E8001A\"/></svg>", new DateTime(2026, 10, 9, 14, 30, 0, 0, DateTimeKind.Utc), null, null, null });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "CircuitoSvg", "Classificacao", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { "<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M60 180 L50 140 Q40 110 55 80 Q70 55 100 40 L140 25 Q170 18 195 30 Q215 40 210 65 Q205 85 185 90 Q165 95 170 115 Q175 135 200 140 L240 135 Q265 125 272 100 L275 65 Q275 35 255 22\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"60\" cy=\"180\" r=\"6\" fill=\"#E8001A\"/></svg>", new DateTime(2026, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 22, 18, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 22, 22, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 23, 18, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "CircuitoSvg", "Classificacao", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { "<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M70 180 L60 130 Q55 90 80 60 L130 30 Q165 15 200 30 L240 55 Q265 75 260 110 L250 150 Q240 175 210 185 L160 190 Q120 190 95 170 Q80 155 85 130 L95 105 Q108 85 135 82 Q160 80 165 100 Q168 120 148 128\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"70\" cy=\"180\" r=\"6\" fill=\"#E8001A\"/></svg>", new DateTime(2026, 10, 30, 22, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 29, 18, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 29, 22, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 30, 18, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "CircuitoSvg", "Classificacao", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { "<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M80 180 L60 140 Q40 100 60 65 Q80 35 120 25 L180 20 Q225 20 250 50 Q270 75 260 110 L240 150 Q220 178 180 185 L130 185 Q95 182 80 160 Q70 140 85 120 Q105 100 130 105\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"80\" cy=\"180\" r=\"6\" fill=\"#E8001A\"/></svg>", new DateTime(2026, 11, 6, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 11, 5, 15, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 11, 5, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 11, 6, 15, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "CircuitoSvg", "Classificacao", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { "<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M50 180 L50 40 Q50 20 70 20 L100 20 Q120 20 120 40 L120 130 Q120 155 145 155 L230 155 Q255 155 255 135 L255 60 Q255 40 275 40 L280 40 Q290 40 290 60 L290 180 Q290 195 270 195 L70 195 Q50 195 50 180 Z\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"50\" cy=\"180\" r=\"6\" fill=\"#E8001A\"/></svg>", new DateTime(2026, 11, 20, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 11, 19, 1, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 11, 19, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 11, 20, 1, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "CircuitoSvg", "Classificacao", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { "<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M60 180 L45 130 Q30 85 55 50 Q80 20 130 15 L200 18 Q245 22 268 55 Q285 85 275 120 L260 155 Q240 180 205 185 L150 188 Q110 188 85 170 Q65 152 72 125 Q80 100 110 95 Q140 92 150 110\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"60\" cy=\"180\" r=\"6\" fill=\"#E8001A\"/></svg>", new DateTime(2026, 11, 27, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 11, 26, 15, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 11, 26, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 11, 27, 15, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "CircuitoSvg", "Classificacao", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { "<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M60 180 L45 140 Q30 100 50 65 Q70 35 110 25 L170 20 Q215 18 245 40 Q270 60 272 95 L270 130 Q265 160 240 175 L200 185 Q165 190 140 178 Q120 165 115 140 Q110 115 130 100 Q150 88 175 95 L200 108 Q218 120 210 142\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"60\" cy=\"180\" r=\"6\" fill=\"#E8001A\"/></svg>", new DateTime(2026, 12, 4, 15, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 3, 11, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 3, 15, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 4, 11, 30, 0, 0, DateTimeKind.Utc) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CircuitoSvg",
                table: "Etapas");

            migrationBuilder.DropColumn(
                name: "Classificacao",
                table: "Etapas");

            migrationBuilder.DropColumn(
                name: "TreinoLivre1",
                table: "Etapas");

            migrationBuilder.DropColumn(
                name: "TreinoLivre2",
                table: "Etapas");

            migrationBuilder.DropColumn(
                name: "TreinoLivre3",
                table: "Etapas");
        }
    }
}
