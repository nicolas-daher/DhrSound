using DhrSound.Api.Dtos;
using DhrSound.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace DhrSound.Api.Controllers;

[ApiController]
[Route("api/usuarios")]
public class UsuarioController : ControllerBase
{
    private readonly UsuarioService _service;
    public UsuarioController(UsuarioService service) => _service = service;

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarUsuarioDto dto)
    {
        var (ok, erro, id) = await _service.CriarAsync(dto);
        if (!ok)
            return BadRequest(new { erro });
        return CreatedAtAction(nameof(Criar), new { id }, new { id });
    }
}
