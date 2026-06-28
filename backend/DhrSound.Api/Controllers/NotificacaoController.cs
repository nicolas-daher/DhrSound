using DhrSound.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DhrSound.Api.Controllers;

[ApiController]
[Route("api/notificacoes")]
[Authorize]
public class NotificacaoController : ControllerBaseAutenticado
{
    private readonly NotificacaoService _service;
    public NotificacaoController(NotificacaoService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> Listar() => Ok(await _service.ListarDoUsuarioAsync(UsuarioId));
}
