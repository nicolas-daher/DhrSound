using DhrSound.Api.Data;
using DhrSound.Api.Dtos;
using DhrSound.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DhrSound.Api.Services;

public class FavoritoService
{
    private readonly DhrSoundContext _contexto;

    public FavoritoService(DhrSoundContext contexto) => _contexto = contexto;

    public async Task<bool> AdicionarMusicaAsync(int usuarioId, int musicaId)
    {
        if (!await _contexto.Musicas.AnyAsync(m => m.Id == musicaId))
            return false;
        if (await _contexto.Favoritos.AnyAsync(f => f.UsuarioId == usuarioId && f.MusicaId == musicaId))
            return true;

        _contexto.Favoritos.Add(new Favorito { UsuarioId = usuarioId, MusicaId = musicaId });
        await _contexto.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AdicionarBandaAsync(int usuarioId, int bandaId)
    {
        if (!await _contexto.Bandas.AnyAsync(b => b.Id == bandaId))
            return false;
        if (await _contexto.Favoritos.AnyAsync(f => f.UsuarioId == usuarioId && f.BandaId == bandaId))
            return true;

        _contexto.Favoritos.Add(new Favorito { UsuarioId = usuarioId, BandaId = bandaId });
        await _contexto.SaveChangesAsync();
        return true;
    }

    public async Task RemoverMusicaAsync(int usuarioId, int musicaId)
    {
        var fav = await _contexto.Favoritos
            .FirstOrDefaultAsync(f => f.UsuarioId == usuarioId && f.MusicaId == musicaId);
        if (fav is not null)
        {
            _contexto.Favoritos.Remove(fav);
            await _contexto.SaveChangesAsync();
        }
    }

    public async Task RemoverBandaAsync(int usuarioId, int bandaId)
    {
        var fav = await _contexto.Favoritos
            .FirstOrDefaultAsync(f => f.UsuarioId == usuarioId && f.BandaId == bandaId);
        if (fav is not null)
        {
            _contexto.Favoritos.Remove(fav);
            await _contexto.SaveChangesAsync();
        }
    }

    public async Task<FavoritosDto> ListarAsync(int usuarioId)
    {
        var musicas = await _contexto.Favoritos
            .Where(f => f.UsuarioId == usuarioId && f.MusicaId != null)
            .Select(f => new MusicaDto(
                f.Musica!.Id, f.Musica.Titulo, f.Musica.DuracaoSegundos,
                f.Musica.BandaId, f.Musica.Banda!.Nome))
            .ToListAsync();

        var bandas = await _contexto.Favoritos
            .Where(f => f.UsuarioId == usuarioId && f.BandaId != null)
            .Select(f => new BandaDto(f.Banda!.Id, f.Banda.Nome, f.Banda.Descricao))
            .ToListAsync();

        return new FavoritosDto(musicas, bandas);
    }
}
