using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using F1Fast.API.Data;
using F1Fast.API.DTOs;
using F1Fast.API.Models;

namespace F1Fast.API.Services;

public class AuthService(AppDbContext db, IConfiguration config)
{
    public async Task<AuthResponse?> LoginAsync(LoginRequest req)
    {
        var user = await db.Usuarios.FirstOrDefaultAsync(u => u.Login == req.Login);
        if (user is null || !BCrypt.Net.BCrypt.Verify(req.Senha, user.SenhaHash))
            return null;

        return new AuthResponse(GerarToken(user), user.Login, user.Nome, user.Role);
    }

    public async Task<(bool Ok, string Erro)> RegisterAsync(RegisterRequest req)
    {
        if (await db.Usuarios.AnyAsync(u => u.Login == req.Login))
            return (false, "Login já em uso.");
        if (await db.Usuarios.AnyAsync(u => u.Email == req.Email))
            return (false, "E-mail já cadastrado.");
        if (await db.Usuarios.AnyAsync(u => u.Cpf == req.Cpf))
            return (false, "CPF já cadastrado.");

        db.Usuarios.Add(new Usuario
        {
            Login       = req.Login,
            Nome        = req.Nome,
            Sobrenome   = req.Sobrenome,
            Cpf         = req.Cpf,
            SenhaHash   = BCrypt.Net.BCrypt.HashPassword(req.Senha),
            Localizacao = req.Localizacao,
            Email       = req.Email
        });

        await db.SaveChangesAsync();
        return (true, "");
    }

    private string GerarToken(Usuario user)
    {
        var key   = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name,           user.Login),
            new Claim(ClaimTypes.Role,           user.Role)
        };

        var token = new JwtSecurityToken(
            issuer:             config["Jwt:Issuer"],
            audience:           config["Jwt:Audience"],
            claims:             claims,
            expires:            DateTime.UtcNow.AddDays(30),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
