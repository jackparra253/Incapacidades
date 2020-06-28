using IAplicacion;
using Microsoft.AspNetCore.Mvc;
using Modelos;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IncapacidadEnfermedadGeneralSalarioLey50Controller : ControllerBase
    {
        private readonly ICreadorIncapacidadEnfermedadGeneralSalarioLey50 _creadorIncapacidad;

        public IncapacidadEnfermedadGeneralSalarioLey50Controller(ICreadorIncapacidadEnfermedadGeneralSalarioLey50 creadorIncapacidad)
        {
            _creadorIncapacidad = creadorIncapacidad;
        }
        
        [HttpPost]
        public void Post(SolicitudIncapacidad solicitudIncapacidad)
        {            
            _creadorIncapacidad.Crear(solicitudIncapacidad);
        }

    }
}