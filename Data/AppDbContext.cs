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

        // Seed Etapas
        mb.Entity<Etapa>().HasData(
            new Etapa { Id=1,  Numero=1,  Nome="GP da Austrália",        Circuito="Albert Park",       Cidade="Melbourne",        Pais="🇦🇺", Sprint=false, PrazoQualify=new DateTime(2026,3,7,2,0,0,DateTimeKind.Utc),    DataCorrida=new DateTime(2026,3,8,1,0,0,DateTimeKind.Utc)    },
            new Etapa { Id=2,  Numero=2,  Nome="Sprint da China",         Circuito="Changai",           Cidade="Changai",          Pais="🇨🇳", Sprint=true,  PrazoQualify=new DateTime(2026,3,13,4,30,0,DateTimeKind.Utc),  DataCorrida=new DateTime(2026,3,13,4,30,0,DateTimeKind.Utc)  },
            new Etapa { Id=3,  Numero=3,  Nome="GP da China",             Circuito="Changai",           Cidade="Changai",          Pais="🇨🇳", Sprint=false, PrazoQualify=new DateTime(2026,3,14,4,0,0,DateTimeKind.Utc),   DataCorrida=new DateTime(2026,3,14,4,0,0,DateTimeKind.Utc)   },
            new Etapa { Id=4,  Numero=4,  Nome="GP do Japão",             Circuito="Suzuka",            Cidade="Suzuka",           Pais="🇯🇵", Sprint=false, PrazoQualify=new DateTime(2026,3,28,3,0,0,DateTimeKind.Utc),   DataCorrida=new DateTime(2026,3,28,3,0,0,DateTimeKind.Utc)   },
            new Etapa { Id=5,  Numero=5,  Nome="GP do Bahrein",           Circuito="Sakhir",            Cidade="Sakhir",           Pais="🇧🇭", Sprint=false, PrazoQualify=new DateTime(2026,4,11,12,0,0,DateTimeKind.Utc),  DataCorrida=new DateTime(2026,4,11,12,0,0,DateTimeKind.Utc)  },
            new Etapa { Id=6,  Numero=6,  Nome="GP da Arábia Saudita",    Circuito="Jeddah",            Cidade="Jeddah",           Pais="🇸🇦", Sprint=false, PrazoQualify=new DateTime(2026,4,18,14,0,0,DateTimeKind.Utc),  DataCorrida=new DateTime(2026,4,18,14,0,0,DateTimeKind.Utc)  },
            new Etapa { Id=7,  Numero=7,  Nome="Sprint de Miami",         Circuito="Miami",             Cidade="Miami",            Pais="🇺🇸", Sprint=true,  PrazoQualify=new DateTime(2026,5,1,17,30,0,DateTimeKind.Utc),  DataCorrida=new DateTime(2026,5,1,17,30,0,DateTimeKind.Utc)  },
            new Etapa { Id=8,  Numero=8,  Nome="GP de Miami",             Circuito="Miami",             Cidade="Miami",            Pais="🇺🇸", Sprint=false, PrazoQualify=new DateTime(2026,5,2,17,0,0,DateTimeKind.Utc),   DataCorrida=new DateTime(2026,5,2,17,0,0,DateTimeKind.Utc)   },
            new Etapa { Id=9,  Numero=9,  Nome="Sprint do Canadá",        Circuito="Montreal",          Cidade="Montreal",         Pais="🇨🇦", Sprint=true,  PrazoQualify=new DateTime(2026,5,22,17,30,0,DateTimeKind.Utc), DataCorrida=new DateTime(2026,5,22,17,30,0,DateTimeKind.Utc) },
            new Etapa { Id=10, Numero=10, Nome="GP do Canadá",            Circuito="Montreal",          Cidade="Montreal",         Pais="🇨🇦", Sprint=false, PrazoQualify=new DateTime(2026,5,23,17,0,0,DateTimeKind.Utc),  DataCorrida=new DateTime(2026,5,23,17,0,0,DateTimeKind.Utc)  },
            new Etapa { Id=11, Numero=11, Nome="GP de Mônaco",            Circuito="Monte Carlo",       Cidade="Monte Carlo",      Pais="🇲🇨", Sprint=false, PrazoQualify=new DateTime(2026,6,6,11,0,0,DateTimeKind.Utc),   DataCorrida=new DateTime(2026,6,6,11,0,0,DateTimeKind.Utc)   },
            new Etapa { Id=12, Numero=12, Nome="GP de Barcelona",         Circuito="Catalunia",         Cidade="Barcelona",        Pais="🇪🇸", Sprint=false, PrazoQualify=new DateTime(2026,6,13,11,0,0,DateTimeKind.Utc),  DataCorrida=new DateTime(2026,6,13,11,0,0,DateTimeKind.Utc)  },
            new Etapa { Id=13, Numero=13, Nome="GP da Áustria",           Circuito="Red Bull Ring",     Cidade="Spielberg",        Pais="🇦🇹", Sprint=false, PrazoQualify=new DateTime(2026,6,27,11,0,0,DateTimeKind.Utc),  DataCorrida=new DateTime(2026,6,27,11,0,0,DateTimeKind.Utc)  },
            new Etapa { Id=14, Numero=14, Nome="Sprint da Grã-Bretanha",  Circuito="Silverstone",       Cidade="Silverstone",      Pais="🇬🇧", Sprint=true,  PrazoQualify=new DateTime(2026,7,3,12,30,0,DateTimeKind.Utc),  DataCorrida=new DateTime(2026,7,3,12,30,0,DateTimeKind.Utc)  },
            new Etapa { Id=15, Numero=15, Nome="GP da Grã-Bretanha",      Circuito="Silverstone",       Cidade="Silverstone",      Pais="🇬🇧", Sprint=false, PrazoQualify=new DateTime(2026,7,4,12,0,0,DateTimeKind.Utc),   DataCorrida=new DateTime(2026,7,4,12,0,0,DateTimeKind.Utc)   },
            new Etapa { Id=16, Numero=16, Nome="GP da Bélgica",           Circuito="Spa-Francorchamps", Cidade="Spa",              Pais="🇧🇪", Sprint=false, PrazoQualify=new DateTime(2026,7,18,11,0,0,DateTimeKind.Utc),  DataCorrida=new DateTime(2026,7,18,11,0,0,DateTimeKind.Utc)  },
            new Etapa { Id=17, Numero=17, Nome="GP da Hungria",           Circuito="Hungaroring",       Cidade="Budapest",         Pais="🇭🇺", Sprint=false, PrazoQualify=new DateTime(2026,7,25,11,0,0,DateTimeKind.Utc),  DataCorrida=new DateTime(2026,7,25,11,0,0,DateTimeKind.Utc)  },
            new Etapa { Id=18, Numero=18, Nome="Sprint da Holanda",       Circuito="Zandvoort",         Cidade="Zandvoort",        Pais="🇳🇱", Sprint=true,  PrazoQualify=new DateTime(2026,8,21,11,30,0,DateTimeKind.Utc), DataCorrida=new DateTime(2026,8,21,11,30,0,DateTimeKind.Utc) },
            new Etapa { Id=19, Numero=19, Nome="GP da Holanda",           Circuito="Zandvoort",         Cidade="Zandvoort",        Pais="🇳🇱", Sprint=false, PrazoQualify=new DateTime(2026,8,22,11,0,0,DateTimeKind.Utc),  DataCorrida=new DateTime(2026,8,22,11,0,0,DateTimeKind.Utc)  },
            new Etapa { Id=20, Numero=20, Nome="GP da Itália",            Circuito="Monza",             Cidade="Monza",            Pais="🇮🇹", Sprint=false, PrazoQualify=new DateTime(2026,9,5,11,0,0,DateTimeKind.Utc),   DataCorrida=new DateTime(2026,9,5,11,0,0,DateTimeKind.Utc)   },
            new Etapa { Id=21, Numero=21, Nome="GP da Espanha",           Circuito="Madri",             Cidade="Madri",            Pais="🇪🇸", Sprint=false, PrazoQualify=new DateTime(2026,9,12,11,0,0,DateTimeKind.Utc),  DataCorrida=new DateTime(2026,9,12,11,0,0,DateTimeKind.Utc)  },
            new Etapa { Id=22, Numero=22, Nome="GP do Azerbaijão",        Circuito="Baku",              Cidade="Baku",             Pais="🇦🇿", Sprint=false, PrazoQualify=new DateTime(2026,9,25,9,0,0,DateTimeKind.Utc),   DataCorrida=new DateTime(2026,9,25,9,0,0,DateTimeKind.Utc)   },
            new Etapa { Id=23, Numero=23, Nome="Sprint de Singapura",     Circuito="Marina Bay",        Cidade="Singapura",        Pais="🇸🇬", Sprint=true,  PrazoQualify=new DateTime(2026,10,9,9,30,0,DateTimeKind.Utc),  DataCorrida=new DateTime(2026,10,9,9,30,0,DateTimeKind.Utc)  },
            new Etapa { Id=24, Numero=24, Nome="GP de Singapura",         Circuito="Marina Bay",        Cidade="Singapura",        Pais="🇸🇬", Sprint=false, PrazoQualify=new DateTime(2026,10,10,10,0,0,DateTimeKind.Utc), DataCorrida=new DateTime(2026,10,10,10,0,0,DateTimeKind.Utc) },
            new Etapa { Id=25, Numero=25, Nome="GP dos EUA",              Circuito="Austin",            Cidade="Austin",           Pais="🇺🇸", Sprint=false, PrazoQualify=new DateTime(2026,10,24,18,0,0,DateTimeKind.Utc), DataCorrida=new DateTime(2026,10,24,18,0,0,DateTimeKind.Utc) },
            new Etapa { Id=26, Numero=26, Nome="GP do México",            Circuito="Cidade do México",  Cidade="Cidade do México", Pais="🇲🇽", Sprint=false, PrazoQualify=new DateTime(2026,10,31,18,0,0,DateTimeKind.Utc), DataCorrida=new DateTime(2026,10,31,18,0,0,DateTimeKind.Utc) },
            new Etapa { Id=27, Numero=27, Nome="GP do Brasil",            Circuito="Interlagos",        Cidade="São Paulo",        Pais="🇧🇷", Sprint=false, PrazoQualify=new DateTime(2026,11,7,15,0,0,DateTimeKind.Utc),  DataCorrida=new DateTime(2026,11,7,15,0,0,DateTimeKind.Utc)  },
            new Etapa { Id=28, Numero=28, Nome="GP de Las Vegas",         Circuito="Las Vegas",         Cidade="Las Vegas",        Pais="🇺🇸", Sprint=false, PrazoQualify=new DateTime(2026,11,21,1,0,0,DateTimeKind.Utc),  DataCorrida=new DateTime(2026,11,21,1,0,0,DateTimeKind.Utc)  },
            new Etapa { Id=29, Numero=29, Nome="GP do Catar",             Circuito="Lusail",            Cidade="Lusail",           Pais="🇶🇦", Sprint=false, PrazoQualify=new DateTime(2026,11,28,15,0,0,DateTimeKind.Utc), DataCorrida=new DateTime(2026,11,28,15,0,0,DateTimeKind.Utc) },
            new Etapa { Id=30, Numero=30, Nome="GP de Abu Dhabi",         Circuito="Yas Marina",        Cidade="Abu Dhabi",        Pais="🇦🇪", Sprint=false, PrazoQualify=new DateTime(2026,12,5,11,0,0,DateTimeKind.Utc),  DataCorrida=new DateTime(2026,12,5,11,0,0,DateTimeKind.Utc)  }
        );
    }
}
