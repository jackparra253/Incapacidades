using Bitakora.Empleados;

namespace Api.Empleados;

public static class EmpleadosEnHttp
{
    public static void MapearEmpleados(this IEndpointRouteBuilder rutas)
    {
        rutas.MapGet("/Empleado", (IConsultarEmpleados consultarEmpleados) =>
                consultarEmpleados.ObtenerEmpleados())
            .WithSummary("Lista los empleados con su salario ya desagregado")
            .Produces<List<Empleado>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
