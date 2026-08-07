import { Dinero } from '../salarios/dinero';
import { TipoSalario } from './tipo-salario';

export interface Empleado {
  id: number;
  nombres: string;
  apellidos: string;
  salario: Dinero;
  salarioDiario: Dinero;
  salarioDiarioPorPorcentajeSalario: Dinero;
  salarioDiarioPorPorcentajeCompensacion: Dinero;
  tipoSalario: TipoSalario;
}
