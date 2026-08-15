import { HttpResourceRef } from '@angular/common/http';

import { Empleado } from '../../domain/empleados/empleado';
import { EmpleadoDto, empleadoDesde } from '../../infrastructure/empleados/empleado-dto';

export interface VistaDeListado {
  mostrarEmpleados(empleados: readonly Empleado[]): void;
  avisarQueCarga(): void;
  avisarDelError(motivo: string): void;
}

export abstract class ListadoDeEmpleados {
  static desde(consulta: HttpResourceRef<EmpleadoDto[] | undefined>): ListadoDeEmpleados {
    if (consulta.isLoading()) return new EmpleadosCargando();

    const falla = consulta.error();
    if (falla) return new ConsultaFallida(motivoDe(falla));

    return new EmpleadosCargados((consulta.value() ?? []).map(empleadoDesde));
  }

  abstract mostrarCon(vista: VistaDeListado): void;
}

export class EmpleadosCargando extends ListadoDeEmpleados {
  mostrarCon(vista: VistaDeListado): void {
    vista.avisarQueCarga();
  }
}

export class EmpleadosCargados extends ListadoDeEmpleados {
  constructor(private readonly empleados: readonly Empleado[]) {
    super();
  }

  mostrarCon(vista: VistaDeListado): void {
    vista.mostrarEmpleados(this.empleados);
  }
}

export class ConsultaFallida extends ListadoDeEmpleados {
  constructor(private readonly motivo: string) {
    super();
  }

  mostrarCon(vista: VistaDeListado): void {
    vista.avisarDelError(this.motivo);
  }
}

function motivoDe(falla: unknown): string {
  return falla instanceof Error ? falla.message : 'No se pudo consultar los empleados.';
}
