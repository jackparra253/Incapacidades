using System.Collections.Generic;
using IAplicacion;
using Microsoft.AspNetCore.Mvc;
using Modelos;

namespace Api.Controllers
{
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

        [HttpGet("{idEmpleado}")]
        public List<DetalleIncapacidad> ObtenerIncapacidades(int idEmpleado)
        {
            return new List<DetalleIncapacidad>
            {
                new DetalleIncapacidad(idEmpleado, "Enfermedad General","2020-06-01", "2020-06-02", 2),
                new DetalleIncapacidad(5, "Enfermedad Laboral","2020-07-13", "2020-07-03", 10),
                new DetalleIncapacidad(6, "Licencia Maternidad","2020-08-15", "2020-08-08", 12)
            };
        }
    }
}