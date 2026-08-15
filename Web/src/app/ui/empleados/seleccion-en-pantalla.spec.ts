import { Empleado } from '../../domain/empleados/empleado';
import { EmpleadoSeleccionado, SinSeleccion } from '../../domain/empleados/seleccion';
import { Integral, Ley50, TipoSalario } from '../../domain/empleados/tipo-salario';
import { Dinero } from '../../domain/salarios/dinero';
import { SalarioDesagregado } from '../../domain/salarios/salario-desagregado';
import { SeleccionEnPantalla } from './seleccion-en-pantalla';

function empleado(id: number, nombres: string, tipo: TipoSalario): Empleado {
  return new Empleado(
    id,
    nombres,
    'Restrepo',
    new SalarioDesagregado(
      Dinero.de(3000000, 'COP'),
      Dinero.de(100000, 'COP'),
      Dinero.de(100000, 'COP'),
      Dinero.de(0, 'COP'),
    ),
    tipo,
  );
}

describe('SeleccionEnPantalla', () => {
  it('con un empleado elegido muestra su nombre y sus salarios', () => {
    const carolina = empleado(1, 'Carolina', Ley50);

    const pantalla = SeleccionEnPantalla.de(new EmpleadoSeleccionado(carolina));

    expect(pantalla.hayEmpleadoElegido()).toBe(true);
    expect(pantalla.nombre()).toBe('Carolina Restrepo');
    expect(pantalla.salarioMensual()).toBe('$ 3.000.000');
    expect(pantalla.salarioDiario()).toBe('$ 100.000');
  });

  it('con un empleado elegido muestra el nombre de su tipo de salario', () => {
    const andres = empleado(2, 'Andrés', Integral);

    const pantalla = SeleccionEnPantalla.de(new EmpleadoSeleccionado(andres));

    expect(pantalla.tipoDeSalario()).toBe('Integral');
  });

  it('reconoce al elegido entre los de la lista', () => {
    const carolina = empleado(1, 'Carolina', Ley50);
    const andres = empleado(2, 'Andrés', Integral);

    const pantalla = SeleccionEnPantalla.de(new EmpleadoSeleccionado(carolina));

    expect(pantalla.esElElegido(carolina)).toBe(true);
    expect(pantalla.esElElegido(andres)).toBe(false);
  });

  it('sin seleccion no hay elegido ni nadie a quien resaltar', () => {
    const carolina = empleado(1, 'Carolina', Ley50);

    const pantalla = SeleccionEnPantalla.de(new SinSeleccion());

    expect(pantalla.hayEmpleadoElegido()).toBe(false);
    expect(pantalla.esElElegido(carolina)).toBe(false);
  });
});
