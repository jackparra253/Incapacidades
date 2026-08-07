using Microsoft.AspNetCore.Mvc;
using Modelos;
using IDatos;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ReconocimientoEconomicoController : ControllerBase
{
    private readonly IIncapacidadServicio _incapacidadServicio;

    public ReconocimientoEconomicoController(IIncapacidadServicio incapacidadServicio)
    {
        _incapacidadServicio = incapacidadServicio;
    }
    

    [HttpGet("{idEmpleado}")]
    public List<DetalleReconocimientoEconomico> ObtenerReconocimientosEconomicos(int idEmpleado)
    {
        return _incapacidadServicio.ObtenerReconocimientosEconomicosDetalle(idEmpleado);
    }
}