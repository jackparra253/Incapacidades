import { EmpleadoDto, empleadoDesde } from './empleado-dto';
import { TipoSalarioDesconocido } from './tipo-salario-desconocido';

const carolinaDto: EmpleadoDto = {
  id: 1,
  nombres: 'Carolina',
  apellidos: 'Restrepo',
  salario: { cantidad: 3000000, moneda: 'COP' },
  salarioDiario: { cantidad: 100000, moneda: 'COP' },
  salarioDiarioPorPorcentajeSalario: { cantidad: 100000, moneda: 'COP' },
  salarioDiarioPorPorcentajeCompensacion: { cantidad: 0, moneda: 'COP' },
  tipoSalario: { tipo: 1 },
};

describe('empleadoDesde', () => {
  it('arma el empleado con su nombre completo', () => {
    const carolina = empleadoDesde(carolinaDto);

    const nombre = carolina.nombreCompleto();

    expect(nombre).toBe('Carolina Restrepo');
  });

  it('arma el salario desagregado que informa la Api', () => {
    const carolina = empleadoDesde(carolinaDto);

    const diario = carolina.salario().diario();

    expect(diario.comoTexto()).toBe('$ 100.000');
  });

  it('traduce el uno de la Api a Ley 50', () => {
    const carolina = empleadoDesde(carolinaDto);

    const tipo = carolina.tipoSalario().nombre();

    expect(tipo).toBe('Ley 50');
  });

  it('traduce el dos de la Api a Integral', () => {
    const andres = empleadoDesde({ ...carolinaDto, tipoSalario: { tipo: 2 } });

    const tipo = andres.tipoSalario().nombre();

    expect(tipo).toBe('Integral');
  });

  it('rechaza un tipo de salario que la Api no deberia informar', () => {
    const mapear = () => empleadoDesde({ ...carolinaDto, tipoSalario: { tipo: 7 } });

    expect(mapear).toThrow(TipoSalarioDesconocido);
  });
});
