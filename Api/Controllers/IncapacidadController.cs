using System.Collections.Generic;
using IAplicacion;
using Microsoft.AspNetCore.Mvc;
using Modelos;
using Modelos.Entidades;
using IDatos;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IncapacidadController : ControllerBase
    {
        private readonly ICreadorIncapacidad _creadorIncapacidad;
        private readonly IIncapacidadServicio _incapacidadServicio;

        public IncapacidadController(ICreadorIncapacidad creadorIncapacidad,IIncapacidadServicio incapacidadServicio)
        {
            _creadorIncapacidad = creadorIncapacidad;
            _incapacidadServicio = incapacidadServicio;
        }
        
        [HttpPost]
        public void Post(SolicitudIncapacidad solicitudIncapacidad)
        {            
            _creadorIncapacidad.Crear(solicitudIncapacidad);
        }

        [HttpGet("{idEmpleado}")]
        public List<DetalleIncapacidad> ObtenerIncapacidades(int idEmpleado)
        {
            return _incapacidadServicio.ObtenerIncapacidadesDetalle(idEmpleado);
        }
    }
}