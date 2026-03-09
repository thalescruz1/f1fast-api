// ============================================================
// DTOs: AuthDTOs (Data Transfer Objects de Autenticação)
// ============================================================
// "DTO" (Data Transfer Object) é um objeto criado especialmente
// para transportar dados entre o frontend e a API.
//
// Por que usar DTOs em vez do Model diretamente?
//   - Segurança: frontend só envia os campos necessários
//   - Não expõe campos internos (ex: SenhaHash, Role, ResetToken)
//   - Pode ter validações diferentes do Model
//
// "record" em C# é um tipo compacto para objetos que só
// transportam dados (sem lógica). Parecido com uma classe, mas
// mais simples — os campos ficam direto no construtor.
// ============================================================

using System.ComponentModel.DataAnnotations;

namespace F1Fast.API.DTOs;

/// <summary>
/// Dados enviados pelo frontend ao fazer CADASTRO de um novo usuário.
/// </summary>
public record RegisterRequest(
    [Required, MaxLength(10)]  string Login,      // Apelido único (máx. 10 chars)
    [Required, MaxLength(100)] string Nome,        // Primeiro nome
    [Required, MaxLength(100)] string Sobrenome,   // Sobrenome
    [Required, MaxLength(14)]  string Cpf,         // CPF (formato livre)
    [Required, MaxLength(8)]   string Senha,       // Senha em texto puro (será criptografada)
    [Required, MaxLength(100)] string Localizacao, // Ex: "São Paulo, SP"
    [Required, EmailAddress]   string Email        // [EmailAddress] valida formato de e-mail
);

/// <summary>
/// Dados enviados pelo frontend ao fazer LOGIN.
/// </summary>
public record LoginRequest(
    [Required] string Login, // Login do usuário
    [Required] string Senha  // Senha em texto puro (comparada com o hash no banco)
);

/// <summary>
/// Resposta enviada pela API após um login bem-sucedido.
/// O frontend salva o Token no localStorage para autenticar chamadas futuras.
/// </summary>
public record AuthResponse(
    string Token, // JWT: token de autenticação (válido por 30 dias)
    string Login, // Login do usuário (para exibir na navbar)
    string Nome,  // Nome do usuário (para personalizar a interface)
    string Role   // "User" ou "Admin" (controla o acesso às funcionalidades)
);

/// <summary>
/// Dados enviados ao solicitar recuperação de senha por e-mail.
/// </summary>
public record EsqueciSenhaRequest([Required, EmailAddress] string Email);

/// <summary>
/// Dados enviados ao redefinir a senha usando o token recebido por e-mail.
/// </summary>
public record RedefinirSenhaRequest(
    [Required] string Token,                               // Token único do e-mail de recuperação
    [Required, MinLength(6), MaxLength(8)] string NovaSenha // Nova senha (entre 6 e 8 caracteres)
);
