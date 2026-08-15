import { Dinero } from './dinero';

export class SalarioDesagregado {
  constructor(
    private readonly mensualidad: Dinero,
    private readonly diaria: Dinero,
    private readonly diariaPorPorcentajeSalario: Dinero,
    private readonly diariaPorPorcentajeCompensacion: Dinero,
  ) {}

  mensual(): Dinero {
    return this.mensualidad;
  }

  diario(): Dinero {
    return this.diaria;
  }

  diarioPorPorcentajeSalario(): Dinero {
    return this.diariaPorPorcentajeSalario;
  }

  diarioPorPorcentajeCompensacion(): Dinero {
    return this.diariaPorPorcentajeCompensacion;
  }
}
