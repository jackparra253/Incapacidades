namespace Bitakora.Empleados;

public class EmpleadoNoEncontrado : NoEncontrado
{
    public int IdEmpleado { get; }

    public EmpleadoNoEncontrado(int idEmpleado)
        : base($"No existe un empleado con el id {idEmpleado}.")
    {
        IdEmpleado = idEmpleado;
    }
}
