using DhrSound.Api.Dtos;
using DhrSound.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace DhrSound.Api.Controllers;

[ApiController]
[Route("api/login")]
public class LoginController : ControllerBase
{
    private readonly UsuarioService _service;
    public LoginController(UsuarioService service) => _service = service;

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var resposta = await _service.LoginAsync(dto);
        if (resposta is null)
            return Unauthorized(new { erro = "Credenciais invalidas" });
        return Ok(resposta);
    }
}
