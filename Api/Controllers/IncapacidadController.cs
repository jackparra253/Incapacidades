using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Modelos;
using IAplicacion;
namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IncapacidadController : ControllerBase
    {
        [HttpPost]
        public void Post([FromBody]SolicitudIncapacidad solicitudIncapacidad)
        {
            
        }
    }
}