using DhrSound.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace DhrSound.Api.Controllers;

[ApiController]
[Route("api/planos")]
public class PlanoController : ControllerBase
{
    private readonly AssinaturaService _service;
    public PlanoController(AssinaturaService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> Listar() => Ok(await _service.ListarPlanosAsync());
}
