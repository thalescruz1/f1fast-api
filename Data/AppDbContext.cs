// ============================================================
// DATA: AppDbContext
// ============================================================
// O "DbContext" é o coração do Entity Framework Core (EF Core).
// É a "ponte" entre o código C# e o banco de dados MySQL.
//
// Responsabilidades:
//   - Define quais tabelas existem no banco (DbSet<T>)
//   - Configura índices e restrições de unicidade
//   - Popula dados iniciais (Seed: equipes, pilotos, etapas)
//   - Executa queries e persiste mudanças (SaveChanges)
//
// "Migrations" usam esta classe para gerar os scripts SQL
// que criam/atualizam o schema do banco de dados.
// ============================================================

using Microsoft.EntityFrameworkCore;
using F1Fast.API.Models;

namespace F1Fast.API.Data;

// DbContextOptions = configurações passadas pelo Program.cs (string de conexão MySQL)
// : DbContext(options) = herda de DbContext passando as opções para a classe base
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    // DbSet<T> = representa uma tabela no banco de dados.
    // "=> Set<T>()" é a forma moderna de declarar DbSet.
    // Com eles, fazemos queries assim: db.Usuarios.Where(...).ToListAsync()
    public DbSet<Usuario>   Usuarios   => Set<Usuario>();
    public DbSet<Equipe>    Equipes    => Set<Equipe>();
    public DbSet<Piloto>    Pilotos    => Set<Piloto>();
    public DbSet<Etapa>     Etapas     => Set<Etapa>();
    public DbSet<Palpite>   Palpites   => Set<Palpite>();
    public DbSet<Resultado> Resultados => Set<Resultado>();
    public DbSet<Pontuacao> Pontuacoes => Set<Pontuacao>();

    /// <summary>
    /// Configuração avançada do modelo do banco de dados.
    /// Chamado pelo EF Core ao criar as migrations e ao inicializar.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder mb)
    {
        // ÍNDICES ÚNICOS: garantem que não existam dois registros com o mesmo valor
        // Isso é validado PELO BANCO, como uma segunda linha de defesa além do C#
        mb.Entity<Usuario>().HasIndex(u => u.Login).IsUnique();  // não pode ter login duplicado
        mb.Entity<Usuario>().HasIndex(u => u.Cpf).IsUnique();    // não pode ter CPF duplicado
        mb.Entity<Usuario>().HasIndex(u => u.Email).IsUnique();  // não pode ter e-mail duplicado
        mb.Entity<Resultado>().HasIndex(r => r.EtapaId).IsUnique();                    // 1 resultado por etapa
        mb.Entity<Pontuacao>().HasIndex(p => new { p.UsuarioId, p.EtapaId }).IsUnique(); // 1 pontuação por usuário/etapa
        mb.Entity<Palpite>().HasIndex(p => new { p.UsuarioId, p.EtapaId }).IsUnique();   // 1 palpite por usuário/etapa

        // SEED DATA: dados pré-carregados automaticamente na criação do banco.
        // ".HasData()" instrui o EF a inserir estes registros na migration inicial.
        // Seed Equipes
        mb.Entity<Equipe>().HasData(
            new Equipe { Id=1,  Nome="McLaren",      Cor="#FF8000" },
            new Equipe { Id=2,  Nome="Red Bull",     Cor="#3671C6" },
            new Equipe { Id=3,  Nome="Mercedes",     Cor="#27F4D2" },
            new Equipe { Id=4,  Nome="Ferrari",      Cor="#E8002D" },
            new Equipe { Id=5,  Nome="Aston Martin", Cor="#358C75" },
            new Equipe { Id=6,  Nome="Williams",     Cor="#64C4FF" },
            new Equipe { Id=7,  Nome="Alpine",       Cor="#FF87BC" },
            new Equipe { Id=8,  Nome="Audi",         Cor="#BDB246" },
            new Equipe { Id=9,  Nome="Haas",         Cor="#B0B0B0" },
            new Equipe { Id=10, Nome="Racing Bulls",  Cor="#6692FF" },
            new Equipe { Id=11, Nome="Cadillac",     Cor="#999999" }
        );

        // Seed Pilotos
        mb.Entity<Piloto>().HasData(
            new Piloto { Id=1,  Numero=1,  Nome="Lando Norris",      EquipeId=1  },
            new Piloto { Id=2,  Numero=3,  Nome="Max Verstappen",    EquipeId=2  },
            new Piloto { Id=3,  Numero=5,  Nome="Gabriel Bortoleto", EquipeId=8  },
            new Piloto { Id=4,  Numero=6,  Nome="Isack Hadjar",      EquipeId=2  },
            new Piloto { Id=5,  Numero=10, Nome="Pierre Gasly",      EquipeId=7  },
            new Piloto { Id=6,  Numero=11, Nome="Sergio Perez",      EquipeId=11 },
            new Piloto { Id=7,  Numero=12, Nome="Kimi Antonelli",    EquipeId=3  },
            new Piloto { Id=8,  Numero=14, Nome="Fernando Alonso",   EquipeId=5  },
            new Piloto { Id=9,  Numero=16, Nome="Charles Leclerc",   EquipeId=4  },
            new Piloto { Id=10, Numero=18, Nome="Lance Stroll",      EquipeId=5  },
            new Piloto { Id=11, Numero=23, Nome="Alexander Albon",   EquipeId=6  },
            new Piloto { Id=12, Numero=27, Nome="Nico Hülkenberg",   EquipeId=8  },
            new Piloto { Id=13, Numero=30, Nome="Liam Lawson",       EquipeId=10 },
            new Piloto { Id=14, Numero=31, Nome="Esteban Ocon",      EquipeId=9  },
            new Piloto { Id=15, Numero=41, Nome="Arvid Lindblad",    EquipeId=10 },
            new Piloto { Id=16, Numero=43, Nome="Franco Colapinto",  EquipeId=7  },
            new Piloto { Id=17, Numero=44, Nome="Lewis Hamilton",    EquipeId=4  },
            new Piloto { Id=18, Numero=55, Nome="Carlos Sainz",      EquipeId=6  },
            new Piloto { Id=19, Numero=63, Nome="George Russell",    EquipeId=3  },
            new Piloto { Id=20, Numero=77, Nome="Valtteri Bottas",   EquipeId=11 },
            new Piloto { Id=21, Numero=81, Nome="Oscar Piastri",     EquipeId=1  },
            new Piloto { Id=22, Numero=87, Nome="Oliver Bearman",    EquipeId=9  }
        );

        // Seed Etapas (com dados de circuito editáveis no banco)
        // Sprints usam ~100km de distância; GPs usam ~305km
        mb.Entity<Etapa>().HasData(
            // Id=1: GP da Austrália — Regular GP
            // TL1 = 2026-03-06 01:30, TL2 = 2026-03-06 05:00, TL3 = 2026-03-07 01:30, Classificacao = 2026-03-07 05:00
            new Etapa {
                Id=1, Numero=1, Nome="GP da Austrália", Circuito="Albert Park", Cidade="Melbourne", Pais="🇦🇺",
                Sprint=false, PrazoQualify=new DateTime(2026,3,7,2,0,0,DateTimeKind.Utc),
                DataCorrida=new DateTime(2026,3,8,1,0,0,DateTimeKind.Utc),
                CircuitoTipo="Circuito semi-permanente", CircuitoComprimento="5.278 km", Voltas=58, Distancia="306.1 km",
                Recordista="Charles Leclerc", TempoRecord="1:19.813", AnoRecord=2022,
                TreinoLivre1=new DateTime(2026,3,6,1,30,0,DateTimeKind.Utc),
                TreinoLivre2=new DateTime(2026,3,6,5,0,0,DateTimeKind.Utc),
                TreinoLivre3=new DateTime(2026,3,7,1,30,0,DateTimeKind.Utc),
                Classificacao=new DateTime(2026,3,7,5,0,0,DateTimeKind.Utc),
                CircuitoSvg="<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M50 160 Q30 140 30 100 Q30 50 60 30 Q90 10 140 15 L200 20 Q240 25 260 50 Q280 80 270 120 Q260 150 230 165 L180 175 Q160 170 150 155 Q140 140 130 145 L100 160 Z\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"50\" cy=\"160\" r=\"6\" fill=\"#E8001A\"/></svg>"
            },
            // Id=2: Sprint da China — Sprint
            // TL1 = 2026-03-12 05:00, TL2 = null, TL3 = null, Classificacao = 2026-03-12 08:30
            new Etapa {
                Id=2, Numero=2, Nome="Sprint da China", Circuito="Changai", Cidade="Changai", Pais="🇨🇳",
                Sprint=true, PrazoQualify=new DateTime(2026,3,13,4,30,0,DateTimeKind.Utc),
                DataCorrida=new DateTime(2026,3,13,4,30,0,DateTimeKind.Utc),
                CircuitoTipo="Circuito permanente", CircuitoComprimento="5.451 km", Voltas=19, Distancia="103.6 km",
                Recordista="Michael Schumacher", TempoRecord="1:32.238", AnoRecord=2004,
                TreinoLivre1=new DateTime(2026,3,12,5,0,0,DateTimeKind.Utc),
                TreinoLivre2=null,
                TreinoLivre3=null,
                Classificacao=new DateTime(2026,3,12,8,30,0,DateTimeKind.Utc),
                CircuitoSvg="<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M40 180 L40 100 Q40 60 70 40 L130 20 Q160 10 180 30 Q200 50 180 70 Q160 90 180 110 Q200 130 240 120 L270 100 Q290 80 280 50 L270 30 Q260 20 240 25 L200 40\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"40\" cy=\"180\" r=\"6\" fill=\"#E8001A\"/></svg>"
            },
            // Id=3: GP da China — Sprint weekend GP (after Sprint Id=2)
            // TL1 = null, TL2 = null, TL3 = null, Classificacao = 2026-03-13 08:30
            new Etapa {
                Id=3, Numero=3, Nome="GP da China", Circuito="Changai", Cidade="Changai", Pais="🇨🇳",
                Sprint=false, PrazoQualify=new DateTime(2026,3,14,4,0,0,DateTimeKind.Utc),
                DataCorrida=new DateTime(2026,3,14,4,0,0,DateTimeKind.Utc),
                CircuitoTipo="Circuito permanente", CircuitoComprimento="5.451 km", Voltas=56, Distancia="305.1 km",
                Recordista="Michael Schumacher", TempoRecord="1:32.238", AnoRecord=2004,
                TreinoLivre1=null,
                TreinoLivre2=null,
                TreinoLivre3=null,
                Classificacao=new DateTime(2026,3,13,8,30,0,DateTimeKind.Utc),
                CircuitoSvg="<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M40 180 L40 100 Q40 60 70 40 L130 20 Q160 10 180 30 Q200 50 180 70 Q160 90 180 110 Q200 130 240 120 L270 100 Q290 80 280 50 L270 30 Q260 20 240 25 L200 40\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"40\" cy=\"180\" r=\"6\" fill=\"#E8001A\"/></svg>"
            },
            // Id=4: GP do Japão — Regular GP
            // TL1 = 2026-03-26 03:30, TL2 = 2026-03-26 07:00, TL3 = 2026-03-27 03:30, Classificacao = 2026-03-27 07:00
            new Etapa {
                Id=4, Numero=4, Nome="GP do Japão", Circuito="Suzuka", Cidade="Suzuka", Pais="🇯🇵",
                Sprint=false, PrazoQualify=new DateTime(2026,3,28,3,0,0,DateTimeKind.Utc),
                DataCorrida=new DateTime(2026,3,28,3,0,0,DateTimeKind.Utc),
                CircuitoTipo="Circuito permanente", CircuitoComprimento="5.807 km", Voltas=53, Distancia="307.5 km",
                Recordista="Lewis Hamilton", TempoRecord="1:30.983", AnoRecord=2019,
                TreinoLivre1=new DateTime(2026,3,26,3,30,0,DateTimeKind.Utc),
                TreinoLivre2=new DateTime(2026,3,26,7,0,0,DateTimeKind.Utc),
                TreinoLivre3=new DateTime(2026,3,27,3,30,0,DateTimeKind.Utc),
                Classificacao=new DateTime(2026,3,27,7,0,0,DateTimeKind.Utc),
                CircuitoSvg="<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M60 170 Q30 150 20 120 Q10 80 40 50 Q70 20 120 20 L180 25 Q220 30 240 60 Q250 80 230 100 Q210 115 220 135 Q230 155 210 170 Q180 190 140 180 Q100 170 80 140 Q65 120 80 100 Q100 80 130 90 Q150 100 140 120\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"60\" cy=\"170\" r=\"6\" fill=\"#E8001A\"/></svg>"
            },
            // Id=5: GP do Bahrein — Regular GP
            // TL1 = 2026-04-09 12:30, TL2 = 2026-04-09 16:00, TL3 = 2026-04-10 12:30, Classificacao = 2026-04-10 16:00
            new Etapa {
                Id=5, Numero=5, Nome="GP do Bahrein", Circuito="Sakhir", Cidade="Sakhir", Pais="🇧🇭",
                Sprint=false, PrazoQualify=new DateTime(2026,4,11,12,0,0,DateTimeKind.Utc),
                DataCorrida=new DateTime(2026,4,11,12,0,0,DateTimeKind.Utc),
                CircuitoTipo="Circuito permanente (iluminado)", CircuitoComprimento="5.412 km", Voltas=57, Distancia="308.2 km",
                Recordista="Pedro de la Rosa", TempoRecord="1:30.252", AnoRecord=2005,
                TreinoLivre1=new DateTime(2026,4,9,12,30,0,DateTimeKind.Utc),
                TreinoLivre2=new DateTime(2026,4,9,16,0,0,DateTimeKind.Utc),
                TreinoLivre3=new DateTime(2026,4,10,12,30,0,DateTimeKind.Utc),
                Classificacao=new DateTime(2026,4,10,16,0,0,DateTimeKind.Utc),
                CircuitoSvg="<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M70 180 L70 140 Q70 120 90 110 L130 90 Q150 80 150 60 Q150 40 130 35 L90 30 Q70 30 60 45 L50 70 Q45 90 60 100 L100 120 Q120 130 120 150 L120 170 Q120 185 140 185 L200 180 Q230 175 250 150 L260 120 Q265 90 250 70 L230 50 Q210 35 190 40 L170 50\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"70\" cy=\"180\" r=\"6\" fill=\"#E8001A\"/></svg>"
            },
            // Id=6: GP da Arábia Saudita — Regular GP
            // TL1 = 2026-04-16 14:30, TL2 = 2026-04-16 18:00, TL3 = 2026-04-17 14:30, Classificacao = 2026-04-17 18:00
            new Etapa {
                Id=6, Numero=6, Nome="GP da Arábia Saudita", Circuito="Jeddah", Cidade="Jeddah", Pais="🇸🇦",
                Sprint=false, PrazoQualify=new DateTime(2026,4,18,14,0,0,DateTimeKind.Utc),
                DataCorrida=new DateTime(2026,4,18,14,0,0,DateTimeKind.Utc),
                CircuitoTipo="Circuito de rua", CircuitoComprimento="6.174 km", Voltas=50, Distancia="308.5 km",
                Recordista="Sergio Perez", TempoRecord="1:30.734", AnoRecord=2023,
                TreinoLivre1=new DateTime(2026,4,16,14,30,0,DateTimeKind.Utc),
                TreinoLivre2=new DateTime(2026,4,16,18,0,0,DateTimeKind.Utc),
                TreinoLivre3=new DateTime(2026,4,17,14,30,0,DateTimeKind.Utc),
                Classificacao=new DateTime(2026,4,17,18,0,0,DateTimeKind.Utc),
                CircuitoSvg="<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M40 190 L40 140 Q40 110 55 90 Q70 70 90 65 L140 55 Q165 50 175 35 Q185 20 210 20 L250 25 Q275 30 280 55 L280 100 Q280 130 260 150 Q240 170 210 175 L160 180 Q130 180 115 165 Q100 150 85 155 L60 170 Q45 180 40 190 Z\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"40\" cy=\"190\" r=\"6\" fill=\"#E8001A\"/></svg>"
            },
            // Id=7: Sprint de Miami — Sprint
            // TL1 = 2026-04-30 18:00, TL2 = null, TL3 = null, Classificacao = 2026-04-30 21:30
            new Etapa {
                Id=7, Numero=7, Nome="Sprint de Miami", Circuito="Miami", Cidade="Miami", Pais="🇺🇸",
                Sprint=true, PrazoQualify=new DateTime(2026,5,1,17,30,0,DateTimeKind.Utc),
                DataCorrida=new DateTime(2026,5,1,17,30,0,DateTimeKind.Utc),
                CircuitoTipo="Circuito semi-permanente", CircuitoComprimento="5.412 km", Voltas=18, Distancia="97.4 km",
                Recordista="Max Verstappen", TempoRecord="1:27.274", AnoRecord=2023,
                TreinoLivre1=new DateTime(2026,4,30,18,0,0,DateTimeKind.Utc),
                TreinoLivre2=null,
                TreinoLivre3=null,
                Classificacao=new DateTime(2026,4,30,21,30,0,DateTimeKind.Utc),
                CircuitoSvg="<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M50 170 L50 50 Q50 30 70 30 L240 30 Q260 30 260 50 L260 110 Q260 130 240 140 L150 145 Q130 145 125 160 Q120 175 100 180 L70 180 Q50 180 50 170 Z\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"50\" cy=\"170\" r=\"6\" fill=\"#E8001A\"/></svg>"
            },
            // Id=8: GP de Miami — Sprint weekend GP (after Sprint Id=7)
            // TL1 = null, TL2 = null, TL3 = null, Classificacao = 2026-05-01 21:30
            new Etapa {
                Id=8, Numero=8, Nome="GP de Miami", Circuito="Miami", Cidade="Miami", Pais="🇺🇸",
                Sprint=false, PrazoQualify=new DateTime(2026,5,2,17,0,0,DateTimeKind.Utc),
                DataCorrida=new DateTime(2026,5,2,17,0,0,DateTimeKind.Utc),
                CircuitoTipo="Circuito semi-permanente", CircuitoComprimento="5.412 km", Voltas=57, Distancia="308.3 km",
                Recordista="Max Verstappen", TempoRecord="1:27.274", AnoRecord=2023,
                TreinoLivre1=null,
                TreinoLivre2=null,
                TreinoLivre3=null,
                Classificacao=new DateTime(2026,5,1,21,30,0,DateTimeKind.Utc),
                CircuitoSvg="<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M50 170 L50 50 Q50 30 70 30 L240 30 Q260 30 260 50 L260 110 Q260 130 240 140 L150 145 Q130 145 125 160 Q120 175 100 180 L70 180 Q50 180 50 170 Z\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"50\" cy=\"170\" r=\"6\" fill=\"#E8001A\"/></svg>"
            },
            // Id=9: Sprint do Canadá — Sprint
            // TL1 = 2026-05-21 18:00, TL2 = null, TL3 = null, Classificacao = 2026-05-21 21:30
            new Etapa {
                Id=9, Numero=9, Nome="Sprint do Canadá", Circuito="Montreal", Cidade="Montreal", Pais="🇨🇦",
                Sprint=true, PrazoQualify=new DateTime(2026,5,22,17,30,0,DateTimeKind.Utc),
                DataCorrida=new DateTime(2026,5,22,17,30,0,DateTimeKind.Utc),
                CircuitoTipo="Circuito semi-permanente", CircuitoComprimento="4.361 km", Voltas=23, Distancia="100.3 km",
                Recordista="Valtteri Bottas", TempoRecord="1:13.078", AnoRecord=2019,
                TreinoLivre1=new DateTime(2026,5,21,18,0,0,DateTimeKind.Utc),
                TreinoLivre2=null,
                TreinoLivre3=null,
                Classificacao=new DateTime(2026,5,21,21,30,0,DateTimeKind.Utc),
                CircuitoSvg="<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M60 180 L50 120 Q45 90 60 70 L100 40 Q120 25 150 25 L200 30 Q230 35 245 55 Q260 80 250 110 L230 140 Q215 160 190 170 L150 180 Q120 185 100 170 Q85 155 90 135 L100 110 Q110 90 130 85\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"60\" cy=\"180\" r=\"6\" fill=\"#E8001A\"/></svg>"
            },
            // Id=10: GP do Canadá — Sprint weekend GP (after Sprint Id=9)
            // TL1 = null, TL2 = null, TL3 = null, Classificacao = 2026-05-22 21:30
            new Etapa {
                Id=10, Numero=10, Nome="GP do Canadá", Circuito="Montreal", Cidade="Montreal", Pais="🇨🇦",
                Sprint=false, PrazoQualify=new DateTime(2026,5,23,17,0,0,DateTimeKind.Utc),
                DataCorrida=new DateTime(2026,5,23,17,0,0,DateTimeKind.Utc),
                CircuitoTipo="Circuito semi-permanente", CircuitoComprimento="4.361 km", Voltas=70, Distancia="305.3 km",
                Recordista="Valtteri Bottas", TempoRecord="1:13.078", AnoRecord=2019,
                TreinoLivre1=null,
                TreinoLivre2=null,
                TreinoLivre3=null,
                Classificacao=new DateTime(2026,5,22,21,30,0,DateTimeKind.Utc),
                CircuitoSvg="<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M60 180 L50 120 Q45 90 60 70 L100 40 Q120 25 150 25 L200 30 Q230 35 245 55 Q260 80 250 110 L230 140 Q215 160 190 170 L150 180 Q120 185 100 170 Q85 155 90 135 L100 110 Q110 90 130 85\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"60\" cy=\"180\" r=\"6\" fill=\"#E8001A\"/></svg>"
            },
            // Id=11: GP de Mônaco — Regular GP
            // TL1 = 2026-06-04 11:30, TL2 = 2026-06-04 15:00, TL3 = 2026-06-05 11:30, Classificacao = 2026-06-05 15:00
            new Etapa {
                Id=11, Numero=11, Nome="GP de Mônaco", Circuito="Monte Carlo", Cidade="Monte Carlo", Pais="🇲🇨",
                Sprint=false, PrazoQualify=new DateTime(2026,6,6,11,0,0,DateTimeKind.Utc),
                DataCorrida=new DateTime(2026,6,6,11,0,0,DateTimeKind.Utc),
                CircuitoTipo="Circuito de rua", CircuitoComprimento="3.337 km", Voltas=78, Distancia="260.3 km",
                Recordista="Charles Leclerc", TempoRecord="1:12.909", AnoRecord=2021,
                TreinoLivre1=new DateTime(2026,6,4,11,30,0,DateTimeKind.Utc),
                TreinoLivre2=new DateTime(2026,6,4,15,0,0,DateTimeKind.Utc),
                TreinoLivre3=new DateTime(2026,6,5,11,30,0,DateTimeKind.Utc),
                Classificacao=new DateTime(2026,6,5,15,0,0,DateTimeKind.Utc),
                CircuitoSvg="<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M80 180 L60 150 Q40 120 50 90 Q60 60 90 50 L130 40 Q155 35 165 55 Q175 75 160 90 L140 105 Q125 115 130 135 Q135 155 160 160 L200 155 Q230 145 245 120 Q260 95 250 70 L235 50 Q220 35 195 35\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"80\" cy=\"180\" r=\"6\" fill=\"#E8001A\"/></svg>"
            },
            // Id=12: GP de Barcelona — Regular GP
            // TL1 = 2026-06-11 11:30, TL2 = 2026-06-11 15:00, TL3 = 2026-06-12 11:30, Classificacao = 2026-06-12 15:00
            new Etapa {
                Id=12, Numero=12, Nome="GP de Barcelona", Circuito="Catalunia", Cidade="Barcelona", Pais="🇪🇸",
                Sprint=false, PrazoQualify=new DateTime(2026,6,13,11,0,0,DateTimeKind.Utc),
                DataCorrida=new DateTime(2026,6,13,11,0,0,DateTimeKind.Utc),
                CircuitoTipo="Circuito permanente", CircuitoComprimento="4.657 km", Voltas=66, Distancia="307.3 km",
                Recordista="Max Verstappen", TempoRecord="1:16.330", AnoRecord=2023,
                TreinoLivre1=new DateTime(2026,6,11,11,30,0,DateTimeKind.Utc),
                TreinoLivre2=new DateTime(2026,6,11,15,0,0,DateTimeKind.Utc),
                TreinoLivre3=new DateTime(2026,6,12,11,30,0,DateTimeKind.Utc),
                Classificacao=new DateTime(2026,6,12,15,0,0,DateTimeKind.Utc),
                CircuitoSvg="<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M50 160 L50 80 Q50 50 80 40 L200 30 Q240 28 260 50 Q280 75 270 100 L250 130 Q235 150 210 155 L160 160 Q140 160 130 145 Q120 130 100 135 L70 150 Q55 158 50 160 Z\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"50\" cy=\"160\" r=\"6\" fill=\"#E8001A\"/></svg>"
            },
            // Id=13: GP da Áustria — Regular GP
            // TL1 = 2026-06-25 11:30, TL2 = 2026-06-25 15:00, TL3 = 2026-06-26 11:30, Classificacao = 2026-06-26 15:00
            new Etapa {
                Id=13, Numero=13, Nome="GP da Áustria", Circuito="Red Bull Ring", Cidade="Spielberg", Pais="🇦🇹",
                Sprint=false, PrazoQualify=new DateTime(2026,6,27,11,0,0,DateTimeKind.Utc),
                DataCorrida=new DateTime(2026,6,27,11,0,0,DateTimeKind.Utc),
                CircuitoTipo="Circuito permanente", CircuitoComprimento="4.318 km", Voltas=71, Distancia="306.5 km",
                Recordista="Carlos Sainz", TempoRecord="1:05.619", AnoRecord=2020,
                TreinoLivre1=new DateTime(2026,6,25,11,30,0,DateTimeKind.Utc),
                TreinoLivre2=new DateTime(2026,6,25,15,0,0,DateTimeKind.Utc),
                TreinoLivre3=new DateTime(2026,6,26,11,30,0,DateTimeKind.Utc),
                Classificacao=new DateTime(2026,6,26,15,0,0,DateTimeKind.Utc),
                CircuitoSvg="<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M80 180 L60 140 Q50 110 70 80 L120 40 Q145 20 175 30 L220 55 Q245 70 250 100 L245 140 Q240 165 210 175 L150 185 Q110 188 80 180 Z\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"80\" cy=\"180\" r=\"6\" fill=\"#E8001A\"/></svg>"
            },
            // Id=14: Sprint da Grã-Bretanha — Sprint
            // TL1 = 2026-07-02 13:00, TL2 = null, TL3 = null, Classificacao = 2026-07-02 16:30
            new Etapa {
                Id=14, Numero=14, Nome="Sprint da Grã-Bretanha", Circuito="Silverstone", Cidade="Silverstone", Pais="🇬🇧",
                Sprint=true, PrazoQualify=new DateTime(2026,7,3,12,30,0,DateTimeKind.Utc),
                DataCorrida=new DateTime(2026,7,3,12,30,0,DateTimeKind.Utc),
                CircuitoTipo="Circuito permanente", CircuitoComprimento="5.891 km", Voltas=17, Distancia="100.1 km",
                Recordista="Lewis Hamilton", TempoRecord="1:24.303", AnoRecord=2020,
                TreinoLivre1=new DateTime(2026,7,2,13,0,0,DateTimeKind.Utc),
                TreinoLivre2=null,
                TreinoLivre3=null,
                Classificacao=new DateTime(2026,7,2,16,30,0,DateTimeKind.Utc),
                CircuitoSvg="<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M60 170 L40 130 Q25 100 45 70 Q65 45 100 35 L150 25 Q190 20 220 40 Q245 55 255 80 L260 110 Q262 140 240 160 Q215 178 180 180 L140 175 Q115 168 105 145 Q95 125 110 110 Q130 95 150 105 Q165 115 155 135\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"60\" cy=\"170\" r=\"6\" fill=\"#E8001A\"/></svg>"
            },
            // Id=15: GP da Grã-Bretanha — Sprint weekend GP (after Sprint Id=14)
            // TL1 = null, TL2 = null, TL3 = null, Classificacao = 2026-07-03 16:30
            new Etapa {
                Id=15, Numero=15, Nome="GP da Grã-Bretanha", Circuito="Silverstone", Cidade="Silverstone", Pais="🇬🇧",
                Sprint=false, PrazoQualify=new DateTime(2026,7,4,12,0,0,DateTimeKind.Utc),
                DataCorrida=new DateTime(2026,7,4,12,0,0,DateTimeKind.Utc),
                CircuitoTipo="Circuito permanente", CircuitoComprimento="5.891 km", Voltas=52, Distancia="306.2 km",
                Recordista="Lewis Hamilton", TempoRecord="1:24.303", AnoRecord=2020,
                TreinoLivre1=null,
                TreinoLivre2=null,
                TreinoLivre3=null,
                Classificacao=new DateTime(2026,7,3,16,30,0,DateTimeKind.Utc),
                CircuitoSvg="<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M60 170 L40 130 Q25 100 45 70 Q65 45 100 35 L150 25 Q190 20 220 40 Q245 55 255 80 L260 110 Q262 140 240 160 Q215 178 180 180 L140 175 Q115 168 105 145 Q95 125 110 110 Q130 95 150 105 Q165 115 155 135\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"60\" cy=\"170\" r=\"6\" fill=\"#E8001A\"/></svg>"
            },
            // Id=16: GP da Bélgica — Regular GP
            // TL1 = 2026-07-16 11:30, TL2 = 2026-07-16 15:00, TL3 = 2026-07-17 11:30, Classificacao = 2026-07-17 15:00
            new Etapa {
                Id=16, Numero=16, Nome="GP da Bélgica", Circuito="Spa-Francorchamps", Cidade="Spa", Pais="🇧🇪",
                Sprint=false, PrazoQualify=new DateTime(2026,7,18,11,0,0,DateTimeKind.Utc),
                DataCorrida=new DateTime(2026,7,18,11,0,0,DateTimeKind.Utc),
                CircuitoTipo="Circuito permanente", CircuitoComprimento="7.004 km", Voltas=44, Distancia="308.1 km",
                Recordista="Valtteri Bottas", TempoRecord="1:46.286", AnoRecord=2018,
                TreinoLivre1=new DateTime(2026,7,16,11,30,0,DateTimeKind.Utc),
                TreinoLivre2=new DateTime(2026,7,16,15,0,0,DateTimeKind.Utc),
                TreinoLivre3=new DateTime(2026,7,17,11,30,0,DateTimeKind.Utc),
                Classificacao=new DateTime(2026,7,17,15,0,0,DateTimeKind.Utc),
                CircuitoSvg="<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M40 170 L30 130 Q20 100 35 70 Q50 40 85 30 L130 20 Q160 15 175 35 Q190 55 175 80 L155 110 Q140 135 155 155 Q170 175 200 170 L240 155 Q270 140 280 110 L285 70 Q285 40 260 25\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"40\" cy=\"170\" r=\"6\" fill=\"#E8001A\"/></svg>"
            },
            // Id=17: GP da Hungria — Regular GP
            // TL1 = 2026-07-23 11:30, TL2 = 2026-07-23 15:00, TL3 = 2026-07-24 11:30, Classificacao = 2026-07-24 15:00
            new Etapa {
                Id=17, Numero=17, Nome="GP da Hungria", Circuito="Hungaroring", Cidade="Budapest", Pais="🇭🇺",
                Sprint=false, PrazoQualify=new DateTime(2026,7,25,11,0,0,DateTimeKind.Utc),
                DataCorrida=new DateTime(2026,7,25,11,0,0,DateTimeKind.Utc),
                CircuitoTipo="Circuito permanente", CircuitoComprimento="4.381 km", Voltas=70, Distancia="306.6 km",
                Recordista="Lewis Hamilton", TempoRecord="1:16.627", AnoRecord=2020,
                TreinoLivre1=new DateTime(2026,7,23,11,30,0,DateTimeKind.Utc),
                TreinoLivre2=new DateTime(2026,7,23,15,0,0,DateTimeKind.Utc),
                TreinoLivre3=new DateTime(2026,7,24,11,30,0,DateTimeKind.Utc),
                Classificacao=new DateTime(2026,7,24,15,0,0,DateTimeKind.Utc),
                CircuitoSvg="<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M70 180 L50 150 Q30 120 40 85 Q50 55 80 40 L130 25 Q170 15 200 35 Q225 50 230 80 L228 120 Q225 150 200 168 L160 180 Q130 188 110 175 Q90 160 95 135 Q100 115 120 110 Q145 105 155 120\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"70\" cy=\"180\" r=\"6\" fill=\"#E8001A\"/></svg>"
            },
            // Id=18: Sprint da Holanda — Sprint
            // TL1 = 2026-08-20 12:00, TL2 = null, TL3 = null, Classificacao = 2026-08-20 15:30
            new Etapa {
                Id=18, Numero=18, Nome="Sprint da Holanda", Circuito="Zandvoort", Cidade="Zandvoort", Pais="🇳🇱",
                Sprint=true, PrazoQualify=new DateTime(2026,8,21,11,30,0,DateTimeKind.Utc),
                DataCorrida=new DateTime(2026,8,21,11,30,0,DateTimeKind.Utc),
                CircuitoTipo="Circuito permanente", CircuitoComprimento="4.259 km", Voltas=24, Distancia="102.2 km",
                Recordista="Max Verstappen", TempoRecord="1:11.097", AnoRecord=2021,
                TreinoLivre1=new DateTime(2026,8,20,12,0,0,DateTimeKind.Utc),
                TreinoLivre2=null,
                TreinoLivre3=null,
                Classificacao=new DateTime(2026,8,20,15,30,0,DateTimeKind.Utc),
                CircuitoSvg="<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M60 170 L45 130 Q30 90 50 55 Q75 25 120 20 L190 22 Q230 25 255 55 Q275 80 265 115 Q255 145 225 165 L170 180 Q130 188 100 175 Q75 160 80 130 Q85 110 110 105\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"60\" cy=\"170\" r=\"6\" fill=\"#E8001A\"/></svg>"
            },
            // Id=19: GP da Holanda — Sprint weekend GP (after Sprint Id=18)
            // TL1 = null, TL2 = null, TL3 = null, Classificacao = 2026-08-21 15:30
            new Etapa {
                Id=19, Numero=19, Nome="GP da Holanda", Circuito="Zandvoort", Cidade="Zandvoort", Pais="🇳🇱",
                Sprint=false, PrazoQualify=new DateTime(2026,8,22,11,0,0,DateTimeKind.Utc),
                DataCorrida=new DateTime(2026,8,22,11,0,0,DateTimeKind.Utc),
                CircuitoTipo="Circuito permanente", CircuitoComprimento="4.259 km", Voltas=72, Distancia="306.6 km",
                Recordista="Max Verstappen", TempoRecord="1:11.097", AnoRecord=2021,
                TreinoLivre1=null,
                TreinoLivre2=null,
                TreinoLivre3=null,
                Classificacao=new DateTime(2026,8,21,15,30,0,DateTimeKind.Utc),
                CircuitoSvg="<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M60 170 L45 130 Q30 90 50 55 Q75 25 120 20 L190 22 Q230 25 255 55 Q275 80 265 115 Q255 145 225 165 L170 180 Q130 188 100 175 Q75 160 80 130 Q85 110 110 105\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"60\" cy=\"170\" r=\"6\" fill=\"#E8001A\"/></svg>"
            },
            // Id=20: GP da Itália — Regular GP
            // TL1 = 2026-09-03 11:30, TL2 = 2026-09-03 15:00, TL3 = 2026-09-04 11:30, Classificacao = 2026-09-04 15:00
            new Etapa {
                Id=20, Numero=20, Nome="GP da Itália", Circuito="Monza", Cidade="Monza", Pais="🇮🇹",
                Sprint=false, PrazoQualify=new DateTime(2026,9,5,11,0,0,DateTimeKind.Utc),
                DataCorrida=new DateTime(2026,9,5,11,0,0,DateTimeKind.Utc),
                CircuitoTipo="Circuito permanente (alta velocidade)", CircuitoComprimento="5.793 km", Voltas=53, Distancia="306.7 km",
                Recordista="Rubens Barrichello", TempoRecord="1:21.046", AnoRecord=2004,
                TreinoLivre1=new DateTime(2026,9,3,11,30,0,DateTimeKind.Utc),
                TreinoLivre2=new DateTime(2026,9,3,15,0,0,DateTimeKind.Utc),
                TreinoLivre3=new DateTime(2026,9,4,11,30,0,DateTimeKind.Utc),
                Classificacao=new DateTime(2026,9,4,15,0,0,DateTimeKind.Utc),
                CircuitoSvg="<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M70 180 L60 100 Q55 60 80 35 L150 20 Q200 15 230 40 Q255 60 250 95 L240 140 Q230 170 195 180 L140 185 Q100 185 85 165 Q75 145 90 125 L120 100 Q140 85 165 90\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"70\" cy=\"180\" r=\"6\" fill=\"#E8001A\"/></svg>"
            },
            // Id=21: GP da Espanha — Regular GP
            // TL1 = 2026-09-10 11:30, TL2 = 2026-09-10 15:00, TL3 = 2026-09-11 11:30, Classificacao = 2026-09-11 15:00
            new Etapa {
                Id=21, Numero=21, Nome="GP da Espanha", Circuito="Madri", Cidade="Madri", Pais="🇪🇸",
                Sprint=false, PrazoQualify=new DateTime(2026,9,12,11,0,0,DateTimeKind.Utc),
                DataCorrida=new DateTime(2026,9,12,11,0,0,DateTimeKind.Utc),
                CircuitoTipo="Circuito semi-permanente", CircuitoComprimento="5.474 km", Voltas=56, Distancia="306.5 km",
                Recordista="—", TempoRecord="—", AnoRecord=2026,
                TreinoLivre1=new DateTime(2026,9,10,11,30,0,DateTimeKind.Utc),
                TreinoLivre2=new DateTime(2026,9,10,15,0,0,DateTimeKind.Utc),
                TreinoLivre3=new DateTime(2026,9,11,11,30,0,DateTimeKind.Utc),
                Classificacao=new DateTime(2026,9,11,15,0,0,DateTimeKind.Utc),
                CircuitoSvg="<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M50 170 L40 110 Q35 70 65 45 L120 25 Q160 15 200 30 Q235 45 250 75 L258 115 Q262 150 235 172 L185 188 Q145 195 110 180 Q80 165 75 135 L78 105 Q82 85 105 80\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"50\" cy=\"170\" r=\"6\" fill=\"#E8001A\"/></svg>"
            },
            // Id=22: GP do Azerbaijão — Regular GP
            // TL1 = 2026-09-23 09:30, TL2 = 2026-09-23 13:00, TL3 = 2026-09-24 09:30, Classificacao = 2026-09-24 13:00
            new Etapa {
                Id=22, Numero=22, Nome="GP do Azerbaijão", Circuito="Baku", Cidade="Baku", Pais="🇦🇿",
                Sprint=false, PrazoQualify=new DateTime(2026,9,25,9,0,0,DateTimeKind.Utc),
                DataCorrida=new DateTime(2026,9,25,9,0,0,DateTimeKind.Utc),
                CircuitoTipo="Circuito de rua", CircuitoComprimento="6.003 km", Voltas=51, Distancia="306.0 km",
                Recordista="Charles Leclerc", TempoRecord="1:43.009", AnoRecord=2019,
                TreinoLivre1=new DateTime(2026,9,23,9,30,0,DateTimeKind.Utc),
                TreinoLivre2=new DateTime(2026,9,23,13,0,0,DateTimeKind.Utc),
                TreinoLivre3=new DateTime(2026,9,24,9,30,0,DateTimeKind.Utc),
                Classificacao=new DateTime(2026,9,24,13,0,0,DateTimeKind.Utc),
                CircuitoSvg="<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M50 190 L50 60 Q50 30 80 25 L120 20 Q150 18 160 40 L165 80 Q165 110 140 120 L100 130 Q75 135 75 155 L80 175 Q90 190 120 188 L220 180 Q260 175 275 150 L280 100 Q280 60 260 40\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"50\" cy=\"190\" r=\"6\" fill=\"#E8001A\"/></svg>"
            },
            // Id=23: Sprint de Singapura — Sprint
            // TL1 = 2026-10-08 10:00, TL2 = null, TL3 = null, Classificacao = 2026-10-08 13:30
            new Etapa {
                Id=23, Numero=23, Nome="Sprint de Singapura", Circuito="Marina Bay", Cidade="Singapura", Pais="🇸🇬",
                Sprint=true, PrazoQualify=new DateTime(2026,10,9,9,30,0,DateTimeKind.Utc),
                DataCorrida=new DateTime(2026,10,9,9,30,0,DateTimeKind.Utc),
                CircuitoTipo="Circuito de rua (noturno)", CircuitoComprimento="4.940 km", Voltas=21, Distancia="103.7 km",
                Recordista="Lewis Hamilton", TempoRecord="1:35.867", AnoRecord=2023,
                TreinoLivre1=new DateTime(2026,10,8,10,0,0,DateTimeKind.Utc),
                TreinoLivre2=null,
                TreinoLivre3=null,
                Classificacao=new DateTime(2026,10,8,13,30,0,DateTimeKind.Utc),
                CircuitoSvg="<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M60 180 L40 140 Q25 110 40 80 Q55 55 85 45 L130 35 Q160 30 180 45 Q200 60 195 85 L185 110 Q175 130 190 148 Q205 165 235 160 L260 150 Q280 135 278 110 L272 75 Q265 50 240 38 L200 28\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"60\" cy=\"180\" r=\"6\" fill=\"#E8001A\"/></svg>"
            },
            // Id=24: GP de Singapura — Sprint weekend GP (after Sprint Id=23)
            // TL1 = null, TL2 = null, TL3 = null, Classificacao = 2026-10-09 14:30
            new Etapa {
                Id=24, Numero=24, Nome="GP de Singapura", Circuito="Marina Bay", Cidade="Singapura", Pais="🇸🇬",
                Sprint=false, PrazoQualify=new DateTime(2026,10,10,10,0,0,DateTimeKind.Utc),
                DataCorrida=new DateTime(2026,10,10,10,0,0,DateTimeKind.Utc),
                CircuitoTipo="Circuito de rua (noturno)", CircuitoComprimento="4.940 km", Voltas=62, Distancia="306.1 km",
                Recordista="Lewis Hamilton", TempoRecord="1:35.867", AnoRecord=2023,
                TreinoLivre1=null,
                TreinoLivre2=null,
                TreinoLivre3=null,
                Classificacao=new DateTime(2026,10,9,14,30,0,DateTimeKind.Utc),
                CircuitoSvg="<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M60 180 L40 140 Q25 110 40 80 Q55 55 85 45 L130 35 Q160 30 180 45 Q200 60 195 85 L185 110 Q175 130 190 148 Q205 165 235 160 L260 150 Q280 135 278 110 L272 75 Q265 50 240 38 L200 28\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"60\" cy=\"180\" r=\"6\" fill=\"#E8001A\"/></svg>"
            },
            // Id=25: GP dos EUA — Regular GP
            // TL1 = 2026-10-22 18:30, TL2 = 2026-10-22 22:00, TL3 = 2026-10-23 18:30, Classificacao = 2026-10-23 22:00
            new Etapa {
                Id=25, Numero=25, Nome="GP dos EUA", Circuito="Austin", Cidade="Austin", Pais="🇺🇸",
                Sprint=false, PrazoQualify=new DateTime(2026,10,24,18,0,0,DateTimeKind.Utc),
                DataCorrida=new DateTime(2026,10,24,18,0,0,DateTimeKind.Utc),
                CircuitoTipo="Circuito permanente", CircuitoComprimento="5.513 km", Voltas=56, Distancia="308.4 km",
                Recordista="Charles Leclerc", TempoRecord="1:36.169", AnoRecord=2023,
                TreinoLivre1=new DateTime(2026,10,22,18,30,0,DateTimeKind.Utc),
                TreinoLivre2=new DateTime(2026,10,22,22,0,0,DateTimeKind.Utc),
                TreinoLivre3=new DateTime(2026,10,23,18,30,0,DateTimeKind.Utc),
                Classificacao=new DateTime(2026,10,23,22,0,0,DateTimeKind.Utc),
                CircuitoSvg="<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M60 180 L50 140 Q40 110 55 80 Q70 55 100 40 L140 25 Q170 18 195 30 Q215 40 210 65 Q205 85 185 90 Q165 95 170 115 Q175 135 200 140 L240 135 Q265 125 272 100 L275 65 Q275 35 255 22\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"60\" cy=\"180\" r=\"6\" fill=\"#E8001A\"/></svg>"
            },
            // Id=26: GP do México — Regular GP
            // TL1 = 2026-10-29 18:30, TL2 = 2026-10-29 22:00, TL3 = 2026-10-30 18:30, Classificacao = 2026-10-30 22:00
            new Etapa {
                Id=26, Numero=26, Nome="GP do México", Circuito="Cidade do México", Cidade="Cidade do México", Pais="🇲🇽",
                Sprint=false, PrazoQualify=new DateTime(2026,10,31,18,0,0,DateTimeKind.Utc),
                DataCorrida=new DateTime(2026,10,31,18,0,0,DateTimeKind.Utc),
                CircuitoTipo="Circuito permanente (alta altitude)", CircuitoComprimento="4.304 km", Voltas=71, Distancia="305.4 km",
                Recordista="Valtteri Bottas", TempoRecord="1:17.774", AnoRecord=2021,
                TreinoLivre1=new DateTime(2026,10,29,18,30,0,DateTimeKind.Utc),
                TreinoLivre2=new DateTime(2026,10,29,22,0,0,DateTimeKind.Utc),
                TreinoLivre3=new DateTime(2026,10,30,18,30,0,DateTimeKind.Utc),
                Classificacao=new DateTime(2026,10,30,22,0,0,DateTimeKind.Utc),
                CircuitoSvg="<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M70 180 L60 130 Q55 90 80 60 L130 30 Q165 15 200 30 L240 55 Q265 75 260 110 L250 150 Q240 175 210 185 L160 190 Q120 190 95 170 Q80 155 85 130 L95 105 Q108 85 135 82 Q160 80 165 100 Q168 120 148 128\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"70\" cy=\"180\" r=\"6\" fill=\"#E8001A\"/></svg>"
            },
            // Id=27: GP do Brasil — Regular GP
            // TL1 = 2026-11-05 15:30, TL2 = 2026-11-05 19:00, TL3 = 2026-11-06 15:30, Classificacao = 2026-11-06 19:00
            new Etapa {
                Id=27, Numero=27, Nome="GP do Brasil", Circuito="Interlagos", Cidade="São Paulo", Pais="🇧🇷",
                Sprint=false, PrazoQualify=new DateTime(2026,11,7,15,0,0,DateTimeKind.Utc),
                DataCorrida=new DateTime(2026,11,7,15,0,0,DateTimeKind.Utc),
                CircuitoTipo="Circuito permanente", CircuitoComprimento="4.309 km", Voltas=71, Distancia="305.9 km",
                Recordista="Valtteri Bottas", TempoRecord="1:10.540", AnoRecord=2018,
                TreinoLivre1=new DateTime(2026,11,5,15,30,0,DateTimeKind.Utc),
                TreinoLivre2=new DateTime(2026,11,5,19,0,0,DateTimeKind.Utc),
                TreinoLivre3=new DateTime(2026,11,6,15,30,0,DateTimeKind.Utc),
                Classificacao=new DateTime(2026,11,6,19,0,0,DateTimeKind.Utc),
                CircuitoSvg="<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M80 180 L60 140 Q40 100 60 65 Q80 35 120 25 L180 20 Q225 20 250 50 Q270 75 260 110 L240 150 Q220 178 180 185 L130 185 Q95 182 80 160 Q70 140 85 120 Q105 100 130 105\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"80\" cy=\"180\" r=\"6\" fill=\"#E8001A\"/></svg>"
            },
            // Id=28: GP de Las Vegas — Regular GP
            // TL1 = 2026-11-19 01:30, TL2 = 2026-11-19 05:00, TL3 = 2026-11-20 01:30, Classificacao = 2026-11-20 05:00
            new Etapa {
                Id=28, Numero=28, Nome="GP de Las Vegas", Circuito="Las Vegas", Cidade="Las Vegas", Pais="🇺🇸",
                Sprint=false, PrazoQualify=new DateTime(2026,11,21,1,0,0,DateTimeKind.Utc),
                DataCorrida=new DateTime(2026,11,21,1,0,0,DateTimeKind.Utc),
                CircuitoTipo="Circuito de rua (noturno)", CircuitoComprimento="6.201 km", Voltas=50, Distancia="310.0 km",
                Recordista="Max Verstappen", TempoRecord="1:35.490", AnoRecord=2023,
                TreinoLivre1=new DateTime(2026,11,19,1,30,0,DateTimeKind.Utc),
                TreinoLivre2=new DateTime(2026,11,19,5,0,0,DateTimeKind.Utc),
                TreinoLivre3=new DateTime(2026,11,20,1,30,0,DateTimeKind.Utc),
                Classificacao=new DateTime(2026,11,20,5,0,0,DateTimeKind.Utc),
                CircuitoSvg="<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M50 180 L50 40 Q50 20 70 20 L100 20 Q120 20 120 40 L120 130 Q120 155 145 155 L230 155 Q255 155 255 135 L255 60 Q255 40 275 40 L280 40 Q290 40 290 60 L290 180 Q290 195 270 195 L70 195 Q50 195 50 180 Z\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"50\" cy=\"180\" r=\"6\" fill=\"#E8001A\"/></svg>"
            },
            // Id=29: GP do Catar — Regular GP
            // TL1 = 2026-11-26 15:30, TL2 = 2026-11-26 19:00, TL3 = 2026-11-27 15:30, Classificacao = 2026-11-27 19:00
            new Etapa {
                Id=29, Numero=29, Nome="GP do Catar", Circuito="Lusail", Cidade="Lusail", Pais="🇶🇦",
                Sprint=false, PrazoQualify=new DateTime(2026,11,28,15,0,0,DateTimeKind.Utc),
                DataCorrida=new DateTime(2026,11,28,15,0,0,DateTimeKind.Utc),
                CircuitoTipo="Circuito permanente (noturno)", CircuitoComprimento="5.380 km", Voltas=57, Distancia="306.6 km",
                Recordista="Charles Leclerc", TempoRecord="1:24.319", AnoRecord=2023,
                TreinoLivre1=new DateTime(2026,11,26,15,30,0,DateTimeKind.Utc),
                TreinoLivre2=new DateTime(2026,11,26,19,0,0,DateTimeKind.Utc),
                TreinoLivre3=new DateTime(2026,11,27,15,30,0,DateTimeKind.Utc),
                Classificacao=new DateTime(2026,11,27,19,0,0,DateTimeKind.Utc),
                CircuitoSvg="<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M60 180 L45 130 Q30 85 55 50 Q80 20 130 15 L200 18 Q245 22 268 55 Q285 85 275 120 L260 155 Q240 180 205 185 L150 188 Q110 188 85 170 Q65 152 72 125 Q80 100 110 95 Q140 92 150 110\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"60\" cy=\"180\" r=\"6\" fill=\"#E8001A\"/></svg>"
            },
            // Id=30: GP de Abu Dhabi — Regular GP
            // TL1 = 2026-12-03 11:30, TL2 = 2026-12-03 15:00, TL3 = 2026-12-04 11:30, Classificacao = 2026-12-04 15:00
            new Etapa {
                Id=30, Numero=30, Nome="GP de Abu Dhabi", Circuito="Yas Marina", Cidade="Abu Dhabi", Pais="🇦🇪",
                Sprint=false, PrazoQualify=new DateTime(2026,12,5,11,0,0,DateTimeKind.Utc),
                DataCorrida=new DateTime(2026,12,5,11,0,0,DateTimeKind.Utc),
                CircuitoTipo="Circuito permanente", CircuitoComprimento="5.281 km", Voltas=58, Distancia="306.2 km",
                Recordista="Max Verstappen", TempoRecord="1:26.103", AnoRecord=2021,
                TreinoLivre1=new DateTime(2026,12,3,11,30,0,DateTimeKind.Utc),
                TreinoLivre2=new DateTime(2026,12,3,15,0,0,DateTimeKind.Utc),
                TreinoLivre3=new DateTime(2026,12,4,11,30,0,DateTimeKind.Utc),
                Classificacao=new DateTime(2026,12,4,15,0,0,DateTimeKind.Utc),
                CircuitoSvg="<svg viewBox=\"0 0 300 200\" fill=\"none\"><path d=\"M60 180 L45 140 Q30 100 50 65 Q70 35 110 25 L170 20 Q215 18 245 40 Q270 60 272 95 L270 130 Q265 160 240 175 L200 185 Q165 190 140 178 Q120 165 115 140 Q110 115 130 100 Q150 88 175 95 L200 108 Q218 120 210 142\" stroke=\"white\" stroke-width=\"8\" stroke-linejoin=\"round\" stroke-linecap=\"round\"/><circle cx=\"60\" cy=\"180\" r=\"6\" fill=\"#E8001A\"/></svg>"
            }
        );
    }
}
