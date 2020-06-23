using Microsoft.AspNetCore.Mvc;
using Modelos;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IncapacidadController : ControllerBase
    {
        [HttpPost]
        public SolicitudIncapacidad Post(SolicitudIncapacidad solicitudIncapacidad)
        {
            return solicitudIncapacidad;
        }
    }
}