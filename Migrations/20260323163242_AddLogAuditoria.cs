using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace F1Fast.API.Migrations
{
    /// <inheritdoc />
    public partial class AddLogAuditoria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LogsAuditoria",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DataHora = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Acao = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UsuarioId = table.Column<int>(type: "int", nullable: true),
                    UsuarioLogin = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Entidade = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EntidadeId = table.Column<int>(type: "int", nullable: true),
                    Detalhes = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sucesso = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Ip = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogsAuditoria", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Classificacao", "DataCorrida", "PrazoQualify", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { new DateTime(2026, 3, 7, 2, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 7, 22, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 6, 23, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 5, 22, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 6, 2, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 6, 22, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Classificacao", "DataCorrida", "PrazoQualify", "TreinoLivre1" },
                values: new object[] { new DateTime(2026, 3, 12, 5, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 13, 1, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 13, 1, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 12, 2, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Classificacao", "DataCorrida", "PrazoQualify" },
                values: new object[] { new DateTime(2026, 3, 13, 5, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 14, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 14, 1, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Classificacao", "DataCorrida", "PrazoQualify", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { new DateTime(2026, 3, 27, 4, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 28, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 28, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 26, 0, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 26, 4, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 27, 0, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Cancelada", "Classificacao", "DataCorrida", "PrazoQualify", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { true, new DateTime(2026, 4, 10, 13, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 11, 9, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 11, 9, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 9, 9, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 9, 13, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 10, 9, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Cancelada", "Classificacao", "DataCorrida", "PrazoQualify", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { true, new DateTime(2026, 4, 17, 15, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 18, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 18, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 16, 11, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 16, 15, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 17, 11, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Classificacao", "DataCorrida", "PrazoQualify", "TreinoLivre1" },
                values: new object[] { new DateTime(2026, 4, 30, 18, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 5, 1, 14, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 5, 1, 14, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 30, 15, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Classificacao", "DataCorrida", "PrazoQualify" },
                values: new object[] { new DateTime(2026, 5, 1, 18, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 5, 2, 14, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 5, 2, 14, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Classificacao", "DataCorrida", "PrazoQualify", "TreinoLivre1" },
                values: new object[] { new DateTime(2026, 5, 21, 18, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 5, 22, 14, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 5, 22, 14, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 5, 21, 15, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Classificacao", "DataCorrida", "PrazoQualify" },
                values: new object[] { new DateTime(2026, 5, 22, 18, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 5, 23, 14, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 5, 23, 14, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Classificacao", "DataCorrida", "PrazoQualify", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { new DateTime(2026, 6, 5, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 6, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 6, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 4, 8, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 4, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 5, 8, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Classificacao", "DataCorrida", "PrazoQualify", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { new DateTime(2026, 6, 12, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 13, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 13, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 11, 8, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 11, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 12, 8, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Classificacao", "DataCorrida", "PrazoQualify", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { new DateTime(2026, 6, 26, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 27, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 27, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 25, 8, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 25, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 26, 8, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Classificacao", "DataCorrida", "PrazoQualify", "TreinoLivre1" },
                values: new object[] { new DateTime(2026, 7, 2, 13, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 3, 9, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 3, 9, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 2, 10, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Classificacao", "DataCorrida", "PrazoQualify" },
                values: new object[] { new DateTime(2026, 7, 3, 13, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 4, 9, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 4, 9, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "Classificacao", "DataCorrida", "PrazoQualify", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { new DateTime(2026, 7, 17, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 18, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 18, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 16, 8, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 16, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 17, 8, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "Classificacao", "DataCorrida", "PrazoQualify", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { new DateTime(2026, 7, 24, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 25, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 25, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 23, 8, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 23, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 24, 8, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "Classificacao", "DataCorrida", "PrazoQualify", "TreinoLivre1" },
                values: new object[] { new DateTime(2026, 8, 20, 12, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 21, 8, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 21, 8, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 20, 9, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "Classificacao", "DataCorrida", "PrazoQualify" },
                values: new object[] { new DateTime(2026, 8, 21, 12, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 22, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 22, 8, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "Classificacao", "DataCorrida", "PrazoQualify", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { new DateTime(2026, 9, 4, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 5, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 5, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 3, 8, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 3, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 4, 8, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "Classificacao", "DataCorrida", "PrazoQualify", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { new DateTime(2026, 9, 11, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 12, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 12, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 10, 8, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 10, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 11, 8, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "Classificacao", "DataCorrida", "PrazoQualify", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { new DateTime(2026, 9, 24, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 25, 6, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 25, 6, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 23, 6, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 23, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 24, 6, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "Classificacao", "DataCorrida", "PrazoQualify", "TreinoLivre1" },
                values: new object[] { new DateTime(2026, 10, 8, 10, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 9, 6, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 9, 6, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 8, 7, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "Classificacao", "DataCorrida", "PrazoQualify" },
                values: new object[] { new DateTime(2026, 10, 9, 11, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 10, 7, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 10, 7, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "Classificacao", "DataCorrida", "PrazoQualify", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { new DateTime(2026, 10, 23, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 24, 15, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 24, 15, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 22, 15, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 22, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 23, 15, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "Classificacao", "DataCorrida", "PrazoQualify", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { new DateTime(2026, 10, 30, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 31, 15, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 31, 15, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 29, 15, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 29, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 30, 15, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "Classificacao", "DataCorrida", "PrazoQualify", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { new DateTime(2026, 11, 6, 16, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 11, 7, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 11, 7, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 11, 5, 12, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 11, 5, 16, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 11, 6, 12, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "Classificacao", "DataCorrida", "PrazoQualify", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { new DateTime(2026, 11, 20, 2, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 11, 20, 22, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 11, 20, 22, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 11, 18, 22, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 11, 19, 2, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 11, 19, 22, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "Classificacao", "DataCorrida", "PrazoQualify", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { new DateTime(2026, 11, 27, 16, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 11, 28, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 11, 28, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 11, 26, 12, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 11, 26, 16, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 11, 27, 12, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "Classificacao", "DataCorrida", "PrazoQualify", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { new DateTime(2026, 12, 4, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 5, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 5, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 3, 8, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 3, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 4, 8, 30, 0, 0, DateTimeKind.Utc) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogsAuditoria");

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Classificacao", "DataCorrida", "PrazoQualify", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { new DateTime(2026, 3, 7, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 8, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 7, 2, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 6, 1, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 6, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 7, 1, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Classificacao", "DataCorrida", "PrazoQualify", "TreinoLivre1" },
                values: new object[] { new DateTime(2026, 3, 12, 8, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 13, 4, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 13, 4, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 12, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Classificacao", "DataCorrida", "PrazoQualify" },
                values: new object[] { new DateTime(2026, 3, 13, 8, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 14, 4, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 14, 4, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Classificacao", "DataCorrida", "PrazoQualify", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { new DateTime(2026, 3, 27, 7, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 28, 3, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 28, 3, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 26, 3, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 26, 7, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 27, 3, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Cancelada", "Classificacao", "DataCorrida", "PrazoQualify", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { false, new DateTime(2026, 4, 10, 16, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 11, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 11, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 9, 12, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 9, 16, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 10, 12, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Cancelada", "Classificacao", "DataCorrida", "PrazoQualify", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { false, new DateTime(2026, 4, 17, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 18, 14, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 18, 14, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 16, 14, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 16, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 17, 14, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Classificacao", "DataCorrida", "PrazoQualify", "TreinoLivre1" },
                values: new object[] { new DateTime(2026, 4, 30, 21, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 5, 1, 17, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 5, 1, 17, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 30, 18, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Classificacao", "DataCorrida", "PrazoQualify" },
                values: new object[] { new DateTime(2026, 5, 1, 21, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 5, 2, 17, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 5, 2, 17, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Classificacao", "DataCorrida", "PrazoQualify", "TreinoLivre1" },
                values: new object[] { new DateTime(2026, 5, 21, 21, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 5, 22, 17, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 5, 22, 17, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 5, 21, 18, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Classificacao", "DataCorrida", "PrazoQualify" },
                values: new object[] { new DateTime(2026, 5, 22, 21, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 5, 23, 17, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 5, 23, 17, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Classificacao", "DataCorrida", "PrazoQualify", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { new DateTime(2026, 6, 5, 15, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 6, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 6, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 4, 11, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 4, 15, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 5, 11, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Classificacao", "DataCorrida", "PrazoQualify", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { new DateTime(2026, 6, 12, 15, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 13, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 13, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 11, 11, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 11, 15, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 12, 11, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Classificacao", "DataCorrida", "PrazoQualify", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { new DateTime(2026, 6, 26, 15, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 27, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 27, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 25, 11, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 25, 15, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 26, 11, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Classificacao", "DataCorrida", "PrazoQualify", "TreinoLivre1" },
                values: new object[] { new DateTime(2026, 7, 2, 16, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 3, 12, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 3, 12, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 2, 13, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Classificacao", "DataCorrida", "PrazoQualify" },
                values: new object[] { new DateTime(2026, 7, 3, 16, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 4, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 4, 12, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "Classificacao", "DataCorrida", "PrazoQualify", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { new DateTime(2026, 7, 17, 15, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 18, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 18, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 16, 11, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 16, 15, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 17, 11, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "Classificacao", "DataCorrida", "PrazoQualify", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { new DateTime(2026, 7, 24, 15, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 25, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 25, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 23, 11, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 23, 15, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 24, 11, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "Classificacao", "DataCorrida", "PrazoQualify", "TreinoLivre1" },
                values: new object[] { new DateTime(2026, 8, 20, 15, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 21, 11, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 21, 11, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 20, 12, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "Classificacao", "DataCorrida", "PrazoQualify" },
                values: new object[] { new DateTime(2026, 8, 21, 15, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 22, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 22, 11, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "Classificacao", "DataCorrida", "PrazoQualify", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { new DateTime(2026, 9, 4, 15, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 5, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 5, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 3, 11, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 3, 15, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 4, 11, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "Classificacao", "DataCorrida", "PrazoQualify", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { new DateTime(2026, 9, 11, 15, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 12, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 12, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 10, 11, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 10, 15, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 11, 11, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "Classificacao", "DataCorrida", "PrazoQualify", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { new DateTime(2026, 9, 24, 13, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 25, 9, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 25, 9, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 23, 9, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 23, 13, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 24, 9, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "Classificacao", "DataCorrida", "PrazoQualify", "TreinoLivre1" },
                values: new object[] { new DateTime(2026, 10, 8, 13, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 9, 9, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 9, 9, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 8, 10, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "Classificacao", "DataCorrida", "PrazoQualify" },
                values: new object[] { new DateTime(2026, 10, 9, 14, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "Classificacao", "DataCorrida", "PrazoQualify", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { new DateTime(2026, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 24, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 24, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 22, 18, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 22, 22, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 23, 18, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "Classificacao", "DataCorrida", "PrazoQualify", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { new DateTime(2026, 10, 30, 22, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 31, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 31, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 29, 18, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 29, 22, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 30, 18, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "Classificacao", "DataCorrida", "PrazoQualify", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { new DateTime(2026, 11, 6, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 11, 7, 15, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 11, 7, 15, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 11, 5, 15, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 11, 5, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 11, 6, 15, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "Classificacao", "DataCorrida", "PrazoQualify", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { new DateTime(2026, 11, 20, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 11, 21, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 11, 21, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 11, 19, 1, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 11, 19, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 11, 20, 1, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "Classificacao", "DataCorrida", "PrazoQualify", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { new DateTime(2026, 11, 27, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 11, 28, 15, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 11, 28, 15, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 11, 26, 15, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 11, 26, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 11, 27, 15, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Etapas",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "Classificacao", "DataCorrida", "PrazoQualify", "TreinoLivre1", "TreinoLivre2", "TreinoLivre3" },
                values: new object[] { new DateTime(2026, 12, 4, 15, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 5, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 5, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 3, 11, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 3, 15, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 4, 11, 30, 0, 0, DateTimeKind.Utc) });
        }
    }
}
