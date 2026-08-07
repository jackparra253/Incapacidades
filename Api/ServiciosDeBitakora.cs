using Bitakora.Empleados;
using Bitakora.Incapacidades;
using Bitakora.Liquidacion;
using Bitakora.Persistencia;
using Bitakora.Salarios;
using Microsoft.EntityFrameworkCore;

namespace Api;

public static class ServiciosDeBitakora
{
    public static IServiceCollection AgregarBitakora(this IServiceCollection servicios, IConfiguration configuracion)
    {
        servicios.AddScoped<IConsultarEmpleados, ConsultarEmpleados>();
        servicios.AddScoped<ICalcularFechas, CalcularFechas>();
        servicios.AddScoped<ICreadorIncapacidad, CreadorIncapacidad>();

        servicios.AddScoped<IIncapacidadServicio, IncapacidadServicio>();
        servicios.AddScoped<IEmpleadoServicio, EmpleadoServicio>();
        servicios.AddScoped<IResponsablePagoServicio, ResponsablePagoServicio>();
        servicios.AddScoped<ISalarioMinimoServicio, SalarioMinimoServicio>();

        servicios.AddProblemDetails();
        servicios.AddExceptionHandler<ExcepcionesDelDominioComoHttp>();

        servicios.ConfigureHttpJsonOptions(opciones => opciones.SerializerOptions.WriteIndented = true);

        servicios.AddDbContext<IncapacidadesContext>(opciones =>
            opciones.UseSqlite(configuracion.GetConnectionString("IncapacidadesContext"),
                sqlite => sqlite.MigrationsAssembly("Api")));

        return servicios;
    }
}
