import { Empleado } from '../../domain/empleados/empleado';
import { Integral, Ley50, TipoSalario } from '../../domain/empleados/tipo-salario';
import { Dinero } from '../../domain/salarios/dinero';
import { SalarioDesagregado } from '../../domain/salarios/salario-desagregado';
import { TipoSalarioDesconocido } from './tipo-salario-desconocido';

export interface DineroDto {
  cantidad: number;
  moneda: string;
}

export interface TipoSalarioDto {
  tipo: number;
}

export interface EmpleadoDto {
  id: number;
  nombres: string;
  apellidos: string;
  salario: DineroDto;
  salarioDiario: DineroDto;
  salarioDiarioPorPorcentajeSalario: DineroDto;
  salarioDiarioPorPorcentajeCompensacion: DineroDto;
  tipoSalario: TipoSalarioDto;
}

export function empleadoDesde(dto: EmpleadoDto): Empleado {
  return new Empleado(
    dto.id,
    dto.nombres,
    dto.apellidos,
    salarioDesde(dto),
    tipoSalarioDesde(dto.tipoSalario.tipo),
  );
}

function salarioDesde(dto: EmpleadoDto): SalarioDesagregado {
  return new SalarioDesagregado(
    dineroDesde(dto.salario),
    dineroDesde(dto.salarioDiario),
    dineroDesde(dto.salarioDiarioPorPorcentajeSalario),
    dineroDesde(dto.salarioDiarioPorPorcentajeCompensacion),
  );
}

function dineroDesde(dto: DineroDto): Dinero {
  return Dinero.de(dto.cantidad, dto.moneda);
}

function tipoSalarioDesde(tipo: number): TipoSalario {
  if (tipo === 1) return Ley50;
  if (tipo === 2) return Integral;

  throw new TipoSalarioDesconocido(tipo);
}
