using Bitakora.Empleados;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class EmpleadoController : ControllerBase
{
    private readonly IConsultarEmpleados _aplicacion;

    public EmpleadoController(IConsultarEmpleados aplicacion)
    {
        _aplicacion = aplicacion;
    }

    [HttpGet]
    public List<Empleado> Get()
    {
        return _aplicacion.ObtenerEmpleados();
    }
}
