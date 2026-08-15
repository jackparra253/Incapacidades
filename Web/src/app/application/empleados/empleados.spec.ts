import { ApplicationRef } from '@angular/core';
import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';

import { Empleado } from '../../domain/empleados/empleado';
import { VistaDeSeleccion } from '../../domain/empleados/seleccion';
import { EmpleadoDto } from '../../infrastructure/empleados/empleado-dto';
import { VistaDeListado } from './listado-de-empleados';
import { Empleados } from './empleados';

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

class VistaEspia implements VistaDeListado, VistaDeSeleccion {
  mostrados: readonly Empleado[] = [];
  empleadoMostrado: Empleado | undefined;
  avisaronQueCarga = false;
  invitaronAElegir = false;
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

  mostrarEmpleado(empleado: Empleado): void {
    this.empleadoMostrado = empleado;
  }

  invitarAElegir(): void {
    this.invitaronAElegir = true;
  }
}

describe('Empleados', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [provideHttpClient(), provideHttpClientTesting()],
    });
  });

  async function empleadosRespondiendo(respuesta: EmpleadoDto[]): Promise<Empleados> {
    const empleados = TestBed.inject(Empleados);
    TestBed.tick();
    TestBed.inject(HttpTestingController).expectOne('/Empleado').flush(respuesta);
    await TestBed.inject(ApplicationRef).whenStable();

    return empleados;
  }

  it('consulta la Api y muestra los empleados que responde', async () => {
    const empleados = await empleadosRespondiendo([carolinaDto]);
    const vista = new VistaEspia();

    empleados.listado().mostrarCon(vista);

    expect(vista.mostrados.map((empleado) => empleado.nombreCompleto())).toEqual([
      'Carolina Restrepo',
    ]);
  });

  it('empieza sin nadie seleccionado', () => {
    const empleados = TestBed.inject(Empleados);
    const vista = new VistaEspia();

    empleados.seleccion().mostrarCon(vista);

    expect(vista.invitaronAElegir).toBe(true);
  });

  it('recuerda el empleado seleccionado', async () => {
    const empleados = await empleadosRespondiendo([carolinaDto]);
    const vista = new VistaEspia();

    empleados.listado().mostrarCon(vista);
    empleados.seleccionar(vista.mostrados[0]);
    empleados.seleccion().mostrarCon(vista);

    expect(vista.empleadoMostrado?.nombreCompleto()).toBe('Carolina Restrepo');
  });
});
