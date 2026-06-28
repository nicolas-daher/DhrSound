using DhrSound.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DhrSound.Api.Controllers;

[ApiController]
[Route("api/favoritos")]
[Authorize]
public class FavoritoController : ControllerBaseAutenticado
{
    private readonly FavoritoService _service;
    public FavoritoController(FavoritoService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> Listar() => Ok(await _service.ListarAsync(UsuarioId));

    [HttpPost("musicas/{id:int}")]
    public async Task<IActionResult> AdicionarMusica(int id)
        => await _service.AdicionarMusicaAsync(UsuarioId, id) ? NoContent() : NotFound();

    [HttpDelete("musicas/{id:int}")]
    public async Task<IActionResult> RemoverMusica(int id)
    {
        await _service.RemoverMusicaAsync(UsuarioId, id);
        return NoContent();
    }

    [HttpPost("bandas/{id:int}")]
    public async Task<IActionResult> AdicionarBanda(int id)
        => await _service.AdicionarBandaAsync(UsuarioId, id) ? NoContent() : NotFound();

    [HttpDelete("bandas/{id:int}")]
    public async Task<IActionResult> RemoverBanda(int id)
    {
        await _service.RemoverBandaAsync(UsuarioId, id);
        return NoContent();
    }
}
