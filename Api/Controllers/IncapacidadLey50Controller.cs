using IAplicacion;
using Microsoft.AspNetCore.Mvc;
using Modelos;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class IncapacidadLey50Controller : ControllerBase
{
    private readonly ICreadorIncapacidad _creadorIncapacidad;

    public IncapacidadLey50Controller(ICreadorIncapacidad creadorIncapacidad)
    {
        _creadorIncapacidad = creadorIncapacidad;
    }
        
    [HttpPost]
    public void Post(SolicitudIncapacidad solicitudIncapacidad)
    {            
        _creadorIncapacidad.Crear(solicitudIncapacidad);
    }

}