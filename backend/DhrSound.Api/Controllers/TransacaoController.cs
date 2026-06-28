using DhrSound.Api.Dtos;
using DhrSound.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace DhrSound.Api.Controllers;

[ApiController]
[Route("api/transacoes")]
public class TransacaoController : ControllerBase
{
    private readonly TransacaoService _service;
    public TransacaoController(TransacaoService service) => _service = service;

    [HttpPost("autorizar")]
    public async Task<IActionResult> Autorizar([FromBody] AutorizarTransacaoDto dto)
        => Ok(await _service.AutorizarAsync(dto));
}
