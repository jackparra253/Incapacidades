import { Dinero } from '../salarios/dinero';
import { SalarioDesagregado } from '../salarios/salario-desagregado';
import { Empleado } from './empleado';
import { Integral, Ley50 } from './tipo-salario';

function salarioDe(mensual: number): SalarioDesagregado {
  return new SalarioDesagregado(
    Dinero.de(mensual, 'COP'),
    Dinero.de(mensual / 30, 'COP'),
    Dinero.de(mensual / 30, 'COP'),
    Dinero.de(0, 'COP'),
  );
}

describe('Empleado', () => {
  it('junta nombres y apellidos en el nombre completo', () => {
    const carolina = new Empleado(1, 'Carolina', 'Restrepo', salarioDe(3000000), Ley50);

    const nombre = carolina.nombreCompleto();

    expect(nombre).toBe('Carolina Restrepo');
  });

  it('es el mismo empleado cuando comparte el id', () => {
    const carolina = new Empleado(1, 'Carolina', 'Restrepo', salarioDe(3000000), Ley50);
    const laMismaCarolina = new Empleado(1, 'Carolina', 'Restrepo', salarioDe(4000000), Integral);

    const esElMismo = carolina.esElMismoQue(laMismaCarolina);

    expect(esElMismo).toBe(true);
  });

  it('no es el mismo empleado que otro con distinto id', () => {
    const carolina = new Empleado(1, 'Carolina', 'Restrepo', salarioDe(3000000), Ley50);
    const andres = new Empleado(2, 'Andrés', 'Gómez', salarioDe(15000000), Integral);

    const esElMismo = carolina.esElMismoQue(andres);

    expect(esElMismo).toBe(false);
  });

  it('conoce el nombre de su tipo de salario', () => {
    const andres = new Empleado(2, 'Andrés', 'Gómez', salarioDe(15000000), Integral);

    const tipo = andres.tipoSalario().nombre();

    expect(tipo).toBe('Integral');
  });
});
