using Microsoft.AspNetCore.Mvc;

namespace DhrSound.Api.Controllers;

public abstract class ControllerBaseAutenticado : ControllerBase
{
    protected int UsuarioId =>
        int.Parse(User.FindFirst("usuarioId")!.Value);
}
