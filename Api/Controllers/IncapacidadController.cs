using Bitakora.Incapacidades;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class IncapacidadController : ControllerBase
{
    private readonly ICreadorIncapacidad _creadorIncapacidad;

    public IncapacidadController(ICreadorIncapacidad creadorIncapacidad)
    {
        _creadorIncapacidad = creadorIncapacidad;
    }

    [HttpPost]
    public void Post(SolicitudIncapacidad solicitudIncapacidad)
    {
        _creadorIncapacidad.Crear(solicitudIncapacidad);
    }
}
