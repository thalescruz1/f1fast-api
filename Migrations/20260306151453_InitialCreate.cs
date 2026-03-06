using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace F1Fast.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Equipes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Cor = table.Column<string>(type: "varchar(7)", maxLength: 7, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Etapas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Numero = table.Column<int>(type: "int", nullable: false),
                    Nome = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Circuito = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Cidade = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Pais = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sprint = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PrazoQualify = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataCorrida = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Encerrada = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Etapas", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Login = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nome = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sobrenome = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Cpf = table.Column<string>(type: "varchar(14)", maxLength: 14, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SenhaHash = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Localizacao = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Role = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CriadoEm = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Pilotos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Numero = table.Column<int>(type: "int", nullable: false),
                    Nome = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EquipeId = table.Column<int>(type: "int", nullable: false),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pilotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pilotos_Equipes_EquipeId",
                        column: x => x.EquipeId,
                        principalTable: "Equipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Resultados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EtapaId = table.Column<int>(type: "int", nullable: false),
                    PoleId = table.Column<int>(type: "int", nullable: false),
                    Pos1Id = table.Column<int>(type: "int", nullable: false),
                    Pos2Id = table.Column<int>(type: "int", nullable: false),
                    Pos3Id = table.Column<int>(type: "int", nullable: false),
                    Pos4Id = table.Column<int>(type: "int", nullable: false),
                    Pos5Id = table.Column<int>(type: "int", nullable: false),
                    Pos6Id = table.Column<int>(type: "int", nullable: false),
                    Pos7Id = table.Column<int>(type: "int", nullable: false),
                    Pos8Id = table.Column<int>(type: "int", nullable: false),
                    Pos9Id = table.Column<int>(type: "int", nullable: false),
                    Pos10Id = table.Column<int>(type: "int", nullable: false),
                    MelhorVoltaId = table.Column<int>(type: "int", nullable: false),
                    InseridoEm = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resultados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Resultados_Etapas_EtapaId",
                        column: x => x.EtapaId,
                        principalTable: "Etapas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Palpites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    EtapaId = table.Column<int>(type: "int", nullable: false),
                    PoleId = table.Column<int>(type: "int", nullable: false),
                    Pos1Id = table.Column<int>(type: "int", nullable: false),
                    Pos2Id = table.Column<int>(type: "int", nullable: false),
                    Pos3Id = table.Column<int>(type: "int", nullable: false),
                    Pos4Id = table.Column<int>(type: "int", nullable: false),
                    Pos5Id = table.Column<int>(type: "int", nullable: false),
                    Pos6Id = table.Column<int>(type: "int", nullable: false),
                    Pos7Id = table.Column<int>(type: "int", nullable: false),
                    Pos8Id = table.Column<int>(type: "int", nullable: false),
                    Pos9Id = table.Column<int>(type: "int", nullable: false),
                    Pos10Id = table.Column<int>(type: "int", nullable: false),
                    MelhorVoltaId = table.Column<int>(type: "int", nullable: false),
                    EnviadoEm = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    PontosObtidos = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Palpites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Palpites_Etapas_EtapaId",
                        column: x => x.EtapaId,
                        principalTable: "Etapas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Palpites_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Pontuacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    EtapaId = table.Column<int>(type: "int", nullable: false),
                    Pontos = table.Column<int>(type: "int", nullable: false),
                    AcertosExatos = table.Column<int>(type: "int", nullable: false),
                    AcertosUmaPos = table.Column<int>(type: "int", nullable: false),
                    AcertosPiloto = table.Column<int>(type: "int", nullable: false),
                    AcertouPole = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AcertouMelhorVolta = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pontuacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pontuacoes_Etapas_EtapaId",
                        column: x => x.EtapaId,
                        principalTable: "Etapas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pontuacoes_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Equipes",
                columns: new[] { "Id", "Cor", "Nome" },
                values: new object[,]
                {
                    { 1, "#FF8000", "McLaren" },
                    { 2, "#3671C6", "Red Bull" },
                    { 3, "#27F4D2", "Mercedes" },
                    { 4, "#E8002D", "Ferrari" },
                    { 5, "#358C75", "Aston Martin" },
                    { 6, "#64C4FF", "Williams" },
                    { 7, "#FF87BC", "Alpine" },
                    { 8, "#BDB246", "Audi" },
                    { 9, "#B0B0B0", "Haas" },
                    { 10, "#6692FF", "Racing Bulls" },
                    { 11, "#999999", "Cadillac" }
                });

            migrationBuilder.InsertData(
                table: "Etapas",
                columns: new[] { "Id", "Cidade", "Circuito", "DataCorrida", "Encerrada", "Nome", "Numero", "Pais", "PrazoQualify", "Sprint" },
                values: new object[,]
                {
                    { 1, "Melbourne", "Albert Park", new DateTime(2026, 3, 8, 1, 0, 0, 0, DateTimeKind.Utc), false, "GP da Austrália", 1, "🇦🇺", new DateTime(2026, 3, 7, 2, 0, 0, 0, DateTimeKind.Utc), false },
                    { 2, "Changai", "Changai", new DateTime(2026, 3, 13, 4, 30, 0, 0, DateTimeKind.Utc), false, "Sprint da China", 2, "🇨🇳", new DateTime(2026, 3, 13, 4, 30, 0, 0, DateTimeKind.Utc), true },
                    { 3, "Changai", "Changai", new DateTime(2026, 3, 14, 4, 0, 0, 0, DateTimeKind.Utc), false, "GP da China", 3, "🇨🇳", new DateTime(2026, 3, 14, 4, 0, 0, 0, DateTimeKind.Utc), false },
                    { 4, "Suzuka", "Suzuka", new DateTime(2026, 3, 28, 3, 0, 0, 0, DateTimeKind.Utc), false, "GP do Japão", 4, "🇯🇵", new DateTime(2026, 3, 28, 3, 0, 0, 0, DateTimeKind.Utc), false },
                    { 5, "Sakhir", "Sakhir", new DateTime(2026, 4, 11, 12, 0, 0, 0, DateTimeKind.Utc), false, "GP do Bahrein", 5, "🇧🇭", new DateTime(2026, 4, 11, 12, 0, 0, 0, DateTimeKind.Utc), false },
                    { 6, "Jeddah", "Jeddah", new DateTime(2026, 4, 18, 14, 0, 0, 0, DateTimeKind.Utc), false, "GP da Arábia Saudita", 6, "🇸🇦", new DateTime(2026, 4, 18, 14, 0, 0, 0, DateTimeKind.Utc), false },
                    { 7, "Miami", "Miami", new DateTime(2026, 5, 1, 17, 30, 0, 0, DateTimeKind.Utc), false, "Sprint de Miami", 7, "🇺🇸", new DateTime(2026, 5, 1, 17, 30, 0, 0, DateTimeKind.Utc), true },
                    { 8, "Miami", "Miami", new DateTime(2026, 5, 2, 17, 0, 0, 0, DateTimeKind.Utc), false, "GP de Miami", 8, "🇺🇸", new DateTime(2026, 5, 2, 17, 0, 0, 0, DateTimeKind.Utc), false },
                    { 9, "Montreal", "Montreal", new DateTime(2026, 5, 22, 17, 30, 0, 0, DateTimeKind.Utc), false, "Sprint do Canadá", 9, "🇨🇦", new DateTime(2026, 5, 22, 17, 30, 0, 0, DateTimeKind.Utc), true },
                    { 10, "Montreal", "Montreal", new DateTime(2026, 5, 23, 17, 0, 0, 0, DateTimeKind.Utc), false, "GP do Canadá", 10, "🇨🇦", new DateTime(2026, 5, 23, 17, 0, 0, 0, DateTimeKind.Utc), false },
                    { 11, "Monte Carlo", "Monte Carlo", new DateTime(2026, 6, 6, 11, 0, 0, 0, DateTimeKind.Utc), false, "GP de Mônaco", 11, "🇲🇨", new DateTime(2026, 6, 6, 11, 0, 0, 0, DateTimeKind.Utc), false },
                    { 12, "Barcelona", "Catalunia", new DateTime(2026, 6, 13, 11, 0, 0, 0, DateTimeKind.Utc), false, "GP de Barcelona", 12, "🇪🇸", new DateTime(2026, 6, 13, 11, 0, 0, 0, DateTimeKind.Utc), false },
                    { 13, "Spielberg", "Red Bull Ring", new DateTime(2026, 6, 27, 11, 0, 0, 0, DateTimeKind.Utc), false, "GP da Áustria", 13, "🇦🇹", new DateTime(2026, 6, 27, 11, 0, 0, 0, DateTimeKind.Utc), false },
                    { 14, "Silverstone", "Silverstone", new DateTime(2026, 7, 3, 12, 30, 0, 0, DateTimeKind.Utc), false, "Sprint da Grã-Bretanha", 14, "🇬🇧", new DateTime(2026, 7, 3, 12, 30, 0, 0, DateTimeKind.Utc), true },
                    { 15, "Silverstone", "Silverstone", new DateTime(2026, 7, 4, 12, 0, 0, 0, DateTimeKind.Utc), false, "GP da Grã-Bretanha", 15, "🇬🇧", new DateTime(2026, 7, 4, 12, 0, 0, 0, DateTimeKind.Utc), false },
                    { 16, "Spa", "Spa-Francorchamps", new DateTime(2026, 7, 18, 11, 0, 0, 0, DateTimeKind.Utc), false, "GP da Bélgica", 16, "🇧🇪", new DateTime(2026, 7, 18, 11, 0, 0, 0, DateTimeKind.Utc), false },
                    { 17, "Budapest", "Hungaroring", new DateTime(2026, 7, 25, 11, 0, 0, 0, DateTimeKind.Utc), false, "GP da Hungria", 17, "🇭🇺", new DateTime(2026, 7, 25, 11, 0, 0, 0, DateTimeKind.Utc), false },
                    { 18, "Zandvoort", "Zandvoort", new DateTime(2026, 8, 21, 11, 30, 0, 0, DateTimeKind.Utc), false, "Sprint da Holanda", 18, "🇳🇱", new DateTime(2026, 8, 21, 11, 30, 0, 0, DateTimeKind.Utc), true },
                    { 19, "Zandvoort", "Zandvoort", new DateTime(2026, 8, 22, 11, 0, 0, 0, DateTimeKind.Utc), false, "GP da Holanda", 19, "🇳🇱", new DateTime(2026, 8, 22, 11, 0, 0, 0, DateTimeKind.Utc), false },
                    { 20, "Monza", "Monza", new DateTime(2026, 9, 5, 11, 0, 0, 0, DateTimeKind.Utc), false, "GP da Itália", 20, "🇮🇹", new DateTime(2026, 9, 5, 11, 0, 0, 0, DateTimeKind.Utc), false },
                    { 21, "Madri", "Madri", new DateTime(2026, 9, 12, 11, 0, 0, 0, DateTimeKind.Utc), false, "GP da Espanha", 21, "🇪🇸", new DateTime(2026, 9, 12, 11, 0, 0, 0, DateTimeKind.Utc), false },
                    { 22, "Baku", "Baku", new DateTime(2026, 9, 25, 9, 0, 0, 0, DateTimeKind.Utc), false, "GP do Azerbaijão", 22, "🇦🇿", new DateTime(2026, 9, 25, 9, 0, 0, 0, DateTimeKind.Utc), false },
                    { 23, "Singapura", "Marina Bay", new DateTime(2026, 10, 9, 9, 30, 0, 0, DateTimeKind.Utc), false, "Sprint de Singapura", 23, "🇸🇬", new DateTime(2026, 10, 9, 9, 30, 0, 0, DateTimeKind.Utc), true },
                    { 24, "Singapura", "Marina Bay", new DateTime(2026, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), false, "GP de Singapura", 24, "🇸🇬", new DateTime(2026, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), false },
                    { 25, "Austin", "Austin", new DateTime(2026, 10, 24, 18, 0, 0, 0, DateTimeKind.Utc), false, "GP dos EUA", 25, "🇺🇸", new DateTime(2026, 10, 24, 18, 0, 0, 0, DateTimeKind.Utc), false },
                    { 26, "Cidade do México", "Cidade do México", new DateTime(2026, 10, 31, 18, 0, 0, 0, DateTimeKind.Utc), false, "GP do México", 26, "🇲🇽", new DateTime(2026, 10, 31, 18, 0, 0, 0, DateTimeKind.Utc), false },
                    { 27, "São Paulo", "Interlagos", new DateTime(2026, 11, 7, 15, 0, 0, 0, DateTimeKind.Utc), false, "GP do Brasil", 27, "🇧🇷", new DateTime(2026, 11, 7, 15, 0, 0, 0, DateTimeKind.Utc), false },
                    { 28, "Las Vegas", "Las Vegas", new DateTime(2026, 11, 21, 1, 0, 0, 0, DateTimeKind.Utc), false, "GP de Las Vegas", 28, "🇺🇸", new DateTime(2026, 11, 21, 1, 0, 0, 0, DateTimeKind.Utc), false },
                    { 29, "Lusail", "Lusail", new DateTime(2026, 11, 28, 15, 0, 0, 0, DateTimeKind.Utc), false, "GP do Catar", 29, "🇶🇦", new DateTime(2026, 11, 28, 15, 0, 0, 0, DateTimeKind.Utc), false },
                    { 30, "Abu Dhabi", "Yas Marina", new DateTime(2026, 12, 5, 11, 0, 0, 0, DateTimeKind.Utc), false, "GP de Abu Dhabi", 30, "🇦🇪", new DateTime(2026, 12, 5, 11, 0, 0, 0, DateTimeKind.Utc), false }
                });

            migrationBuilder.InsertData(
                table: "Pilotos",
                columns: new[] { "Id", "Ativo", "EquipeId", "Nome", "Numero" },
                values: new object[,]
                {
                    { 1, true, 1, "Lando Norris", 1 },
                    { 2, true, 2, "Max Verstappen", 3 },
                    { 3, true, 8, "Gabriel Bortoleto", 5 },
                    { 4, true, 2, "Isack Hadjar", 6 },
                    { 5, true, 7, "Pierre Gasly", 10 },
                    { 6, true, 11, "Sergio Perez", 11 },
                    { 7, true, 3, "Kimi Antonelli", 12 },
                    { 8, true, 5, "Fernando Alonso", 14 },
                    { 9, true, 4, "Charles Leclerc", 16 },
                    { 10, true, 5, "Lance Stroll", 18 },
                    { 11, true, 6, "Alexander Albon", 23 },
                    { 12, true, 8, "Nico Hülkenberg", 27 },
                    { 13, true, 10, "Liam Lawson", 30 },
                    { 14, true, 9, "Esteban Ocon", 31 },
                    { 15, true, 10, "Arvid Lindblad", 41 },
                    { 16, true, 7, "Franco Colapinto", 43 },
                    { 17, true, 4, "Lewis Hamilton", 44 },
                    { 18, true, 6, "Carlos Sainz", 55 },
                    { 19, true, 3, "George Russell", 63 },
                    { 20, true, 11, "Valtteri Bottas", 77 },
                    { 21, true, 1, "Oscar Piastri", 81 },
                    { 22, true, 9, "Oliver Bearman", 87 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Palpites_EtapaId",
                table: "Palpites",
                column: "EtapaId");

            migrationBuilder.CreateIndex(
                name: "IX_Palpites_UsuarioId_EtapaId",
                table: "Palpites",
                columns: new[] { "UsuarioId", "EtapaId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pilotos_EquipeId",
                table: "Pilotos",
                column: "EquipeId");

            migrationBuilder.CreateIndex(
                name: "IX_Pontuacoes_EtapaId",
                table: "Pontuacoes",
                column: "EtapaId");

            migrationBuilder.CreateIndex(
                name: "IX_Pontuacoes_UsuarioId_EtapaId",
                table: "Pontuacoes",
                columns: new[] { "UsuarioId", "EtapaId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Resultados_EtapaId",
                table: "Resultados",
                column: "EtapaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Cpf",
                table: "Usuarios",
                column: "Cpf",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Login",
                table: "Usuarios",
                column: "Login",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Palpites");

            migrationBuilder.DropTable(
                name: "Pilotos");

            migrationBuilder.DropTable(
                name: "Pontuacoes");

            migrationBuilder.DropTable(
                name: "Resultados");

            migrationBuilder.DropTable(
                name: "Equipes");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Etapas");
        }
    }
}
