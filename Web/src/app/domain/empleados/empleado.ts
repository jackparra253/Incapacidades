import { SalarioDesagregado } from '../salarios/salario-desagregado';
import { TipoSalario } from './tipo-salario';

export class Empleado {
  constructor(
    private readonly id: number,
    private readonly nombres: string,
    private readonly apellidos: string,
    private readonly salarioDesagregado: SalarioDesagregado,
    private readonly tipo: TipoSalario,
  ) {}

  nombreCompleto(): string {
    return `${this.nombres} ${this.apellidos}`;
  }

  salario(): SalarioDesagregado {
    return this.salarioDesagregado;
  }

  tipoSalario(): TipoSalario {
    return this.tipo;
  }

  esElMismoQue(otro: Empleado): boolean {
    return this.id === otro.id;
  }
}
