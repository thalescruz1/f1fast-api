// ============================================================
// PROGRAM.CS — Ponto de entrada da aplicação
// ============================================================
// Este é o arquivo mais importante do backend: é o "startup"
// da aplicação, onde tudo é configurado e iniciado.
//
// Funciona em duas fases:
//   1. FASE DE CONFIGURAÇÃO (builder.*): registra serviços,
//      banco de dados, autenticação JWT, CORS e Swagger.
//   2. FASE DE EXECUÇÃO (app.*): configura o pipeline HTTP
//      (middlewares) e inicia o servidor.
//
// "Pipeline HTTP" = sequência de passos que toda requisição
// percorre antes de chegar nos controllers.
// ============================================================

using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using F1Fast.API.Background;
using F1Fast.API.Data;
using F1Fast.API.Middleware;
using F1Fast.API.Services;

// Cria o "builder" — objeto que configura e constrói a aplicação
var builder = WebApplication.CreateBuilder(args);

// ============================================================
// BANCO DE DADOS
// ============================================================
// Registra o AppDbContext para usar MySQL com Entity Framework.
// A string de conexão vem do appsettings.json ("Default").
// ServerVersion.AutoDetect detecta a versão do MySQL automaticamente.
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseMySql(
        builder.Configuration.GetConnectionString("Default"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("Default"))
    )
);

// ============================================================
// INJEÇÃO DE DEPENDÊNCIA — Services
// ============================================================
// "AddScoped" = cria uma nova instância por requisição HTTP.
// O .NET gerencia o ciclo de vida — não precisamos fazer "new".
// "AddHostedService" = registra o serviço de background para
// rodar continuamente em segundo plano.
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<PontuacaoService>();
builder.Services.AddScoped<NotificacaoService>();
builder.Services.AddScoped<AuditoriaService>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddHostedService<LembreteBackgroundService>();

// ============================================================
// AUTENTICAÇÃO JWT
// ============================================================
// Configura o sistema para aceitar tokens JWT nas requisições.
// Quando o cliente envia "Authorization: Bearer {token}",
// o .NET valida automaticamente antes de chegar nos controllers.
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer           = true, // verifica quem emitiu o token
            ValidateAudience         = true, // verifica para quem é o token
            ValidateLifetime         = true, // verifica se o token não expirou
            ValidateIssuerSigningKey = true, // verifica a assinatura digital
            ValidIssuer              = builder.Configuration["Jwt:Issuer"],
            ValidAudience            = builder.Configuration["Jwt:Audience"],
            // A chave secreta usada para verificar a assinatura do token
            IssuerSigningKey         = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

// Habilita o sistema de autorização ([Authorize], [AllowAnonymous], roles)
builder.Services.AddAuthorization();

// ============================================================
// CORS (Cross-Origin Resource Sharing)
// ============================================================
// O navegador bloqueia requisições de domínios diferentes por padrão.
// CORS libera domínios específicos para acessar esta API.
// Sem isso, o Angular (localhost:4200) não conseguiria chamar a API.
builder.Services.AddCors(opt =>
    opt.AddPolicy("Angular", p => p
        .WithOrigins(
            "http://localhost:4200",              // desenvolvimento local
            "https://f1fast.com.br",              // produção
            "https://blue-pond-0da734610.2.azurestaticapps.net",
            "https://www.f1fast.com.br",     "https://f1fast.com.br", "https://novo.f1fast.com.br"
            //  // Azure Static Web Apps
        )
        .AllowAnyMethod()  // permite GET, POST, PATCH, etc.
        .AllowAnyHeader()  // permite qualquer header (incluindo Authorization)
    )
);

// ============================================================
// CONTROLLERS + SWAGGER
// ============================================================
// AddControllers = habilita o sistema de controllers (MVC API)
// Swagger = interface web para testar a API em /swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "F1Fast API", Version = "v1" });

    // Adiciona suporte a JWT no Swagger UI (botão "Authorize")
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In          = ParameterLocation.Header,
        Description = "JWT: Bearer {token}",
        Name        = "Authorization",
        Type        = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {{
        new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
        Array.Empty<string>()
    }});
});

// "builder.Build()" finaliza a configuração e cria a aplicação
var app = builder.Build();

// ============================================================
// MIGRATIONS AUTOMÁTICAS
// ============================================================
// Executa as migrations pendentes ao iniciar a aplicação.
// Garante que o banco de dados está sempre atualizado.
// Em produção, isso evita a necessidade de rodar "dotnet ef database update" manualmente.
using (var scope = app.Services.CreateScope())
    scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.Migrate();

// Swagger ativo sempre (inclusive em produção, para facilitar testes)
    app.UseSwagger();
    app.UseSwaggerUI();

// ============================================================
// PIPELINE DE MIDDLEWARES
// ============================================================
// A ORDEM importa! Cada middleware processa a requisição em sequência.
// UseCors → UseAuthentication → UseAuthorization → MapControllers

app.UseMiddleware<ExceptionMiddleware>(); // captura exceções não tratadas → ApiError
app.UseCors("Angular");       // libera as origens permitidas
app.UseAuthentication();      // lê e valida o token JWT
app.UseAuthorization();       // verifica [Authorize] / roles
app.MapControllers();         // roteia para os controllers corretos
app.Run();                    // inicia o servidor HTTP
