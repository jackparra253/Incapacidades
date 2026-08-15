export abstract class TipoSalario {
  abstract nombre(): string;
}

class SalarioLey50 extends TipoSalario {
  nombre(): string {
    return 'Ley 50';
  }
}

class SalarioIntegral extends TipoSalario {
  nombre(): string {
    return 'Integral';
  }
}

export const Ley50: TipoSalario = new SalarioLey50();
export const Integral: TipoSalario = new SalarioIntegral();
