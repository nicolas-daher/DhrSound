using DhrSound.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace DhrSound.Api.Controllers;

[ApiController]
[Route("api/busca")]
public class BuscaController : ControllerBase
{
    private readonly BuscaService _service;
    public BuscaController(BuscaService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> Buscar([FromQuery] string? termo = null, [FromQuery] int pagina = 1)
        => Ok(await _service.BuscarAsync(termo, pagina));
}
