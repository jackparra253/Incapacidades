import { Dinero } from './dinero';
import { MonedaInvalida } from './moneda-invalida';

describe('Dinero', () => {
  it('se muestra con el formato del pais', () => {
    const salario = Dinero.de(3000000, 'COP');

    const texto = salario.comoTexto();

    expect(texto).toBe('$ 3.000.000');
  });

  it('es igual a otro con la misma cantidad y moneda', () => {
    const unSalario = Dinero.de(3000000, 'COP');
    const otroSalario = Dinero.de(3000000, 'COP');

    const iguales = unSalario.esIgualA(otroSalario);

    expect(iguales).toBe(true);
  });

  it('no es igual a otro en distinta moneda', () => {
    const enPesos = Dinero.de(3000000, 'COP');
    const enDolares = Dinero.de(3000000, 'USD');

    const iguales = enPesos.esIgualA(enDolares);

    expect(iguales).toBe(false);
  });

  it('ignora las diferencias por debajo del centavo', () => {
    const unSalario = Dinero.de(100000.001, 'COP');
    const otroSalario = Dinero.de(100000.002, 'COP');

    const iguales = unSalario.esIgualA(otroSalario);

    expect(iguales).toBe(true);
  });

  it('no se puede crear con una moneda que no es un codigo de tres letras', () => {
    const crear = () => Dinero.de(3000000, 'PESOS');

    expect(crear).toThrow(MonedaInvalida);
  });

  it('acepta la moneda en minusculas y la normaliza', () => {
    const enMinusculas = Dinero.de(3000000, 'cop');
    const enMayusculas = Dinero.de(3000000, 'COP');

    const iguales = enMinusculas.esIgualA(enMayusculas);

    expect(iguales).toBe(true);
  });
});
