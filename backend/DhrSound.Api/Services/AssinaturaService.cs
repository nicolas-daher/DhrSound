using DhrSound.Api.Data;
using DhrSound.Api.Dtos;
using DhrSound.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DhrSound.Api.Services;

public class AssinaturaService
{
    private readonly DhrSoundContext _contexto;

    public AssinaturaService(DhrSoundContext contexto) => _contexto = contexto;

    public async Task<IEnumerable<PlanoDto>> ListarPlanosAsync()
    {
        return await _contexto.Planos
            .Select(p => new PlanoDto(p.Id, p.Nome, p.Descricao, p.Preco))
            .ToListAsync();
    }

    public async Task<AssinaturaDto?> ObterAtivaAsync(int usuarioId)
    {
        return await _contexto.Assinaturas
            .Where(a => a.UsuarioId == usuarioId && a.Ativa)
            .OrderByDescending(a => a.DataInicio)
            .Select(a => new AssinaturaDto(
                a.Id, a.PlanoId, a.Plano!.Nome, a.DataInicio, a.DataFim, a.Ativa))
            .FirstOrDefaultAsync();
    }

    public async Task<(bool Ok, string Erro, AssinaturaDto? Assinatura)> AssinarAsync(int usuarioId, int planoId)
    {
        var plano = await _contexto.Planos.FindAsync(planoId);
        if (plano is null)
            return (false, "Plano nao encontrado", null);

        var ativas = await _contexto.Assinaturas
            .Where(a => a.UsuarioId == usuarioId && a.Ativa)
            .ToListAsync();
        foreach (var a in ativas)
        {
            a.Ativa = false;
            a.DataFim = DateTime.UtcNow;
        }

        var nova = new Assinatura
        {
            UsuarioId = usuarioId,
            PlanoId = planoId,
            DataInicio = DateTime.UtcNow,
            Ativa = true
        };
        _contexto.Assinaturas.Add(nova);
        await _contexto.SaveChangesAsync();

        return (true, "", new AssinaturaDto(nova.Id, plano.Id, plano.Nome, nova.DataInicio, nova.DataFim, nova.Ativa));
    }
}
