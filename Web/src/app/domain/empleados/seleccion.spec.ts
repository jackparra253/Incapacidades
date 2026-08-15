import { Dinero } from '../salarios/dinero';
import { SalarioDesagregado } from '../salarios/salario-desagregado';
import { Empleado } from './empleado';
import { EmpleadoSeleccionado, SinSeleccion, VistaDeSeleccion } from './seleccion';
import { Ley50 } from './tipo-salario';

class VistaEspia implements VistaDeSeleccion {
  readonly mostrados: Empleado[] = [];
  invitaronAElegir = false;

  mostrarEmpleado(empleado: Empleado): void {
    this.mostrados.push(empleado);
  }

  invitarAElegir(): void {
    this.invitaronAElegir = true;
  }
}

function unEmpleado(): Empleado {
  const pesos = Dinero.de(3000000, 'COP');

  return new Empleado(
    1,
    'Carolina',
    'Restrepo',
    new SalarioDesagregado(pesos, pesos, pesos, pesos),
    Ley50,
  );
}

describe('Seleccion', () => {
  it('un empleado seleccionado se muestra en la vista', () => {
    const carolina = unEmpleado();
    const vista = new VistaEspia();

    new EmpleadoSeleccionado(carolina).mostrarCon(vista);

    expect(vista.mostrados).toEqual([carolina]);
  });

  it('un empleado seleccionado no invita a elegir', () => {
    const vista = new VistaEspia();

    new EmpleadoSeleccionado(unEmpleado()).mostrarCon(vista);

    expect(vista.invitaronAElegir).toBe(false);
  });

  it('sin seleccion se invita a elegir', () => {
    const vista = new VistaEspia();

    new SinSeleccion().mostrarCon(vista);

    expect(vista.invitaronAElegir).toBe(true);
  });

  it('sin seleccion no se muestra ningun empleado', () => {
    const vista = new VistaEspia();

    new SinSeleccion().mostrarCon(vista);

    expect(vista.mostrados).toEqual([]);
  });
});
