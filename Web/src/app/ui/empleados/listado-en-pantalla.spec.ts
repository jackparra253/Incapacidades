import {
  ConsultaFallida,
  EmpleadosCargados,
  EmpleadosCargando,
} from '../../application/empleados/listado-de-empleados';
import { Empleado } from '../../domain/empleados/empleado';
import { Ley50 } from '../../domain/empleados/tipo-salario';
import { Dinero } from '../../domain/salarios/dinero';
import { SalarioDesagregado } from '../../domain/salarios/salario-desagregado';
import { ListadoEnPantalla } from './listado-en-pantalla';

function unEmpleado(): Empleado {
  const pesos = Dinero.de(3000000, 'COP');

  return new Empleado(1, 'Carolina', 'Restrepo', new SalarioDesagregado(pesos, pesos, pesos, pesos), Ley50);
}

describe('ListadoEnPantalla', () => {
  it('mientras carga lo dice y no falla', () => {
    const pantalla = ListadoEnPantalla.de(new EmpleadosCargando());

    expect(pantalla.estaCargando()).toBe(true);
    expect(pantalla.fallo()).toBe(false);
  });

  it('mientras carga no ofrece empleados para pintar', () => {
    const pantalla = ListadoEnPantalla.de(new EmpleadosCargando());

    const hay = pantalla.hayEmpleados();

    expect(hay).toBe(false);
  });

  it('con empleados los ofrece y no dice que carga', () => {
    const pantalla = ListadoEnPantalla.de(new EmpleadosCargados([unEmpleado()]));

    expect(pantalla.hayEmpleados()).toBe(true);
    expect(pantalla.estaCargando()).toBe(false);
  });

  it('cuando la Api responde sin empleados no hay nada que pintar ni error', () => {
    const pantalla = ListadoEnPantalla.de(new EmpleadosCargados([]));

    expect(pantalla.hayEmpleados()).toBe(false);
    expect(pantalla.fallo()).toBe(false);
  });

  it('cuando falla expone el motivo y no finge una lista vacia', () => {
    const pantalla = ListadoEnPantalla.de(new ConsultaFallida('Se cayó la Api'));

    expect(pantalla.fallo()).toBe(true);
    expect(pantalla.motivoDelError()).toBe('Se cayó la Api');
    expect(pantalla.estaCargando()).toBe(false);
  });
});
