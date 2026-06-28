using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace DhrSound.Api.Services;

public class TokenService
{
    private readonly IConfiguration _config;

    public TokenService(IConfiguration config) => _config = config;

    public string GerarToken(int usuarioId)
    {
        var chave = _config["Jwt:Chave"] ?? throw new InvalidOperationException("Jwt:Chave nao configurada");
        var credenciais = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chave)),
            SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuarioId.ToString()),
            new Claim("usuarioId", usuarioId.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Emissor"],
            audience: _config["Jwt:Audiencia"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: credenciais);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
