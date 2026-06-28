using System.Text.RegularExpressions;
using DhrSound.Api.Data;
using DhrSound.Api.Dtos;
using DhrSound.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DhrSound.Api.Services;

public class UsuarioService
{
    private const int TamanhoMinimoSenha = 6;
    private static readonly Regex EmailRegex =
        new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

    private readonly DhrSoundContext _contexto;
    private readonly TokenService _tokenService;

    public UsuarioService(DhrSoundContext contexto, TokenService tokenService)
    {
        _contexto = contexto;
        _tokenService = tokenService;
    }

    public async Task<(bool Ok, string Erro, int UsuarioId)> CriarAsync(CriarUsuarioDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nome))
            return (false, "Nome obrigatorio", 0);
        if (!EmailRegex.IsMatch(dto.Email ?? ""))
            return (false, "E-mail invalido", 0);
        if ((dto.Senha?.Length ?? 0) < TamanhoMinimoSenha)
            return (false, $"Senha deve ter ao menos {TamanhoMinimoSenha} caracteres", 0);

        if (await _contexto.Usuarios.AnyAsync(u => u.Email == dto.Email))
            return (false, "E-mail ja cadastrado", 0);

        var usuario = new Usuario
        {
            Nome = dto.Nome,
            Email = dto.Email!,
            SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha)
        };

        usuario.Conta = new Conta { Ativa = true, LimiteDisponivel = 1000m };

        _contexto.Usuarios.Add(usuario);
        await _contexto.SaveChangesAsync();

        return (true, "", usuario.Id);
    }

    public async Task<LoginRespostaDto?> LoginAsync(LoginDto dto)
    {
        var usuario = await _contexto.Usuarios.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (usuario is null || !BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.SenhaHash))
            return null;

        var token = _tokenService.GerarToken(usuario.Id);
        return new LoginRespostaDto(usuario.Id, usuario.Nome, token);
    }
}
