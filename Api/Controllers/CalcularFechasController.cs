using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Modelos.Entidades;
using IAplicacion;
namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CalcularFechasController : ControllerBase
    {
        public readonly ICalcularFechas _calcularFechas;


        public CalcularFechasController(ICalcularFechas calcularFechas)
        {
            _calcularFechas = calcularFechas;
        }

        [HttpGet]
        public DateTime Get(int anio, int mes, int dia, int cantidadDias)
        {
            DateTime fechaIncial = new DateTime(anio, mes, dia);

            return _calcularFechas.CalcularSiguienteFecha(fechaIncial, cantidadDias);
        }
    }
}