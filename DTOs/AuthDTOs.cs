using System.ComponentModel.DataAnnotations;

namespace F1Fast.API.DTOs;

public record RegisterRequest(
    [Required, MaxLength(10)]  string Login,
    [Required, MaxLength(100)] string Nome,
    [Required, MaxLength(100)] string Sobrenome,
    [Required, MaxLength(14)]  string Cpf,
    [Required, MaxLength(8)]   string Senha,
    [Required, MaxLength(100)] string Localizacao,
    [Required, EmailAddress]   string Email
);

public record LoginRequest(
    [Required] string Login,
    [Required] string Senha
);

public record AuthResponse(
    string Token,
    string Login,
    string Nome,
    string Role
);
