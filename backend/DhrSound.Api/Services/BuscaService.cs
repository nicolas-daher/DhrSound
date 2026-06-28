using DhrSound.Api.Data;
using DhrSound.Api.Dtos;
using Microsoft.EntityFrameworkCore;

namespace DhrSound.Api.Services;

public class BuscaService
{
    private const int TamanhoPagina = 20;
    private readonly DhrSoundContext _contexto;

    public BuscaService(DhrSoundContext contexto) => _contexto = contexto;

    public async Task<ResultadoBuscaDto> BuscarAsync(string? termo, int pagina = 1)
    {
        termo = (termo ?? "").Trim();
        var pular = Math.Max(0, pagina - 1) * TamanhoPagina;

        var consultaBandas = _contexto.Bandas.AsQueryable();
        var consultaMusicas = _contexto.Musicas.AsQueryable();
        if (termo.Length > 0)
        {
            consultaBandas = consultaBandas.Where(b => b.Nome.Contains(termo));
            consultaMusicas = consultaMusicas.Where(m => m.Titulo.Contains(termo));
        }

        var bandas = await consultaBandas
            .OrderBy(b => b.Nome)
            .Skip(pular).Take(TamanhoPagina)
            .Select(b => new BandaDto(b.Id, b.Nome, b.Descricao))
            .ToListAsync();

        var musicas = await consultaMusicas
            .OrderBy(m => m.Titulo)
            .Skip(pular).Take(TamanhoPagina)
            .Select(m => new MusicaDto(m.Id, m.Titulo, m.DuracaoSegundos, m.BandaId, m.Banda!.Nome))
            .ToListAsync();

        return new ResultadoBuscaDto(bandas, musicas);
    }
}
