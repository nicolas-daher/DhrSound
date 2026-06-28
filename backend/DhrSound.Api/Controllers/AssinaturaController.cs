using DhrSound.Api.Dtos;
using DhrSound.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DhrSound.Api.Controllers;

[ApiController]
[Route("api/assinaturas")]
[Authorize]
public class AssinaturaController : ControllerBaseAutenticado
{
    private readonly AssinaturaService _service;
    public AssinaturaController(AssinaturaService service) => _service = service;

    [HttpGet("atual")]
    public async Task<IActionResult> Atual()
    {
        var assinatura = await _service.ObterAtivaAsync(UsuarioId);
        return assinatura is null ? NoContent() : Ok(assinatura);
    }

    [HttpPost]
    public async Task<IActionResult> Assinar([FromBody] CriarAssinaturaDto dto)
    {
        var (ok, erro, assinatura) = await _service.AssinarAsync(UsuarioId, dto.PlanoId);
        if (!ok)
            return BadRequest(new { erro });
        return Ok(assinatura);
    }
}
