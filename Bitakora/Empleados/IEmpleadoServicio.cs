namespace Bitakora.Empleados;

public interface IEmpleadoServicio
{
    List<Empleado> ObtenerEmpleados();

    Empleado ObtenerEmpleado(int id);
}
