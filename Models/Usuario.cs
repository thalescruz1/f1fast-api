// ============================================================
// MODELO: Usuario
// ============================================================
// Em .NET, um "Model" representa uma tabela no banco de dados.
// Cada propriedade aqui vira uma coluna na tabela "Usuarios".
// O Entity Framework lê essa classe e cria/gerencia a tabela
// automaticamente no MySQL — sem precisar escrever SQL manual.
// ============================================================

using System.ComponentModel.DataAnnotations;

namespace F1Fast.API.Models;

/// <summary>
/// Representa um usuário cadastrado no sistema F1Fast.
/// Cada objeto desta classe = uma linha na tabela "Usuarios" do banco.
/// </summary>
public class Usuario
{
    // Chave primária: o banco gera esse número automaticamente (1, 2, 3...).
    // É o identificador único de cada usuário.
    public int Id { get; set; }

    // [Required] = campo obrigatório — não pode ser vazio
    // [MaxLength(10)] = máximo de 10 caracteres
    // "{ get; set; }" = propriedade que pode ser lida e escrita
    [Required, MaxLength(10)]
    public string Login { get; set; } = "";

    [Required, MaxLength(100)]
    public string Nome { get; set; } = "";

    [Required, MaxLength(100)]
    public string Sobrenome { get; set; } = "";

    [Required, MaxLength(14)]
    public string Cpf { get; set; } = "";

    // SEGURANÇA: a senha NUNCA é salva em texto puro!
    // O BCrypt transforma "minha123" em algo como "$2a$11$xK9..."
    // Mesmo que o banco vaze, ninguém consegue descobrir a senha original.
    [Required]
    public string SenhaHash { get; set; } = "";

    [Required, MaxLength(100)]
    public string Localizacao { get; set; } = "";

    [Required, MaxLength(150)]
    public string Email { get; set; } = "";

    // Papel do usuário no sistema.
    // "User"  = participante comum (padrão para todos os novos cadastros)
    // "Admin" = administrador (pode lançar resultados e gerenciar usuários)
    public string Role { get; set; } = "User";

    // Token temporário enviado por e-mail para redefinição de senha.
    // O "?" após o tipo significa que o campo pode ser nulo (null).
    // Quando não há recuperação em andamento, fica null.
    public string? ResetToken { get; set; }

    // Data/hora em que o token de reset expira.
    // Tokens são válidos por apenas 1 hora por segurança.
    public DateTime? ResetTokenExpiry { get; set; }

    // Data de criação do cadastro. DateTime.UtcNow = hora atual universal (UTC).
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    // "Propriedade de Navegação": lista de todos os palpites deste usuário.
    // O Entity Framework faz um JOIN automático quando solicitado.
    // ICollection<T> é uma coleção genérica (semelhante a uma lista).
    public ICollection<Palpite> Palpites { get; set; } = [];
}
