using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Modelos;
using IDatos;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IncapacidadConsultaController : ControllerBase
    {

        private readonly IIncapacidadServicio _incapacidadServicio;

        public IncapacidadConsultaController(IIncapacidadServicio incapacidadServicio)
        {
            _incapacidadServicio = incapacidadServicio;
        }

        [HttpGet("{idEmpleado}")]
        public List<DetalleIncapacidad> ObtenerIncapacidades(int idEmpleado)
        {
            return _incapacidadServicio.ObtenerIncapacidadesDetalle(idEmpleado);
        }

    }
}