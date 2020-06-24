using System.Collections.Generic;
using IAplicacion;
using Microsoft.AspNetCore.Mvc;
using Modelos;
using Modelos.Entidades;
using IDatos;
using Modelos.Constantes;
using Modelos.ValueObjects;

namespace Api.Controllers
{
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
            return new List<DetalleReconocimientoEconomico>
            {
                new DetalleReconocimientoEconomico(1,"2020-06-02", "2020-06-02", new Dinero(1_000_000, Moneda.COP),"Empresa")
            };
        }
    }
}