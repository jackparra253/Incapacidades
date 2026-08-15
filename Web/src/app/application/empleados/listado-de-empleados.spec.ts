import { signal } from '@angular/core';
import { HttpResourceRef } from '@angular/common/http';

import { Empleado } from '../../domain/empleados/empleado';
import { EmpleadoDto } from '../../infrastructure/empleados/empleado-dto';
import { ListadoDeEmpleados, VistaDeListado } from './listado-de-empleados';

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

class VistaEspia implements VistaDeListado {
  mostrados: readonly Empleado[] | undefined;
  avisaronQueCarga = false;
  motivoDelError: string | undefined;

  mostrarEmpleados(empleados: readonly Empleado[]): void {
    this.mostrados = empleados;
  }

  avisarQueCarga(): void {
    this.avisaronQueCarga = true;
  }

  avisarDelError(motivo: string): void {
    this.motivoDelError = motivo;
  }
}

function consulta(estado: {
  cargando?: boolean;
  falla?: unknown;
  valor?: EmpleadoDto[];
}): HttpResourceRef<EmpleadoDto[] | undefined> {
  return {
    isLoading: signal(estado.cargando ?? false),
    error: signal(estado.falla),
    value: signal(estado.valor),
  } as unknown as HttpResourceRef<EmpleadoDto[] | undefined>;
}

describe('ListadoDeEmpleados', () => {
  it('mientras carga avisa que carga', () => {
    const listado = ListadoDeEmpleados.desde(consulta({ cargando: true }));
    const vista = new VistaEspia();

    listado.mostrarCon(vista);

    expect(vista.avisaronQueCarga).toBe(true);
  });

  it('mientras carga no muestra una lista vacia', () => {
    const listado = ListadoDeEmpleados.desde(consulta({ cargando: true }));
    const vista = new VistaEspia();

    listado.mostrarCon(vista);

    expect(vista.mostrados).toBeUndefined();
  });

  it('cuando responde muestra los empleados ya mapeados', () => {
    const listado = ListadoDeEmpleados.desde(consulta({ valor: [carolinaDto] }));
    const vista = new VistaEspia();

    listado.mostrarCon(vista);

    expect(vista.mostrados?.map((empleado) => empleado.nombreCompleto())).toEqual([
      'Carolina Restrepo',
    ]);
  });

  it('cuando la Api responde sin empleados muestra la lista vacia', () => {
    const listado = ListadoDeEmpleados.desde(consulta({ valor: [] }));
    const vista = new VistaEspia();

    listado.mostrarCon(vista);

    expect(vista.mostrados).toEqual([]);
  });

  it('cuando la consulta falla avisa del error en vez de mostrar una lista vacia', () => {
    const listado = ListadoDeEmpleados.desde(consulta({ falla: new Error('Se cayó la Api') }));
    const vista = new VistaEspia();

    listado.mostrarCon(vista);

    expect(vista.motivoDelError).toBe('Se cayó la Api');
    expect(vista.mostrados).toBeUndefined();
  });
});
