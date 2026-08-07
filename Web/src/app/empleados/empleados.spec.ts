import { ApplicationRef } from '@angular/core';
import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';

import { Empleados } from './empleados';
import { Empleado } from './empleado';
import { Integral, Ley50 } from './tipo-salario';
import { VistaDeSeleccion } from './seleccion';

describe('Empleados', () => {
  const carolina: Empleado = {
    id: 1,
    nombres: 'Carolina',
    apellidos: 'Restrepo',
    salario: { cantidad: 3000000, moneda: 'COP' },
    salarioDiario: { cantidad: 100000, moneda: 'COP' },
    salarioDiarioPorPorcentajeSalario: { cantidad: 100000, moneda: 'COP' },
    salarioDiarioPorPorcentajeCompensacion: { cantidad: 0, moneda: 'COP' },
    tipoSalario: { tipo: Ley50, porcentajeSalario: 1, porcentajeCompensacion: 0 },
  };

  const andres: Empleado = {
    id: 2,
    nombres: 'Andrés',
    apellidos: 'Gómez',
    salario: { cantidad: 15000000, moneda: 'COP' },
    salarioDiario: { cantidad: 500000, moneda: 'COP' },
    salarioDiarioPorPorcentajeSalario: { cantidad: 350000, moneda: 'COP' },
    salarioDiarioPorPorcentajeCompensacion: { cantidad: 150000, moneda: 'COP' },
    tipoSalario: { tipo: Integral, porcentajeSalario: 0.7, porcentajeCompensacion: 0.3 },
  };

  async function empleadosRespondiendo(respuesta: Empleado[]): Promise<Empleados> {
    const empleados = TestBed.inject(Empleados);
    TestBed.tick();
    TestBed.inject(HttpTestingController).expectOne('/Empleado').flush(respuesta);
    await TestBed.inject(ApplicationRef).whenStable();

    return empleados;
  }

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [provideHttpClient(), provideHttpClientTesting()],
    });
  });

  it('expone los empleados que responde la Api', async () => {
    const empleados = TestBed.inject(Empleados);
    TestBed.tick();

    TestBed.inject(HttpTestingController).expectOne('/Empleado').flush([carolina]);
    await TestBed.inject(ApplicationRef).whenStable();

    expect(empleados.todos()).toEqual([carolina]);
  });

  it('recuerda el empleado seleccionado', async () => {
    const empleados = await empleadosRespondiendo([carolina, andres]);
    const vista = new VistaEspia();

    empleados.seleccionar(andres.id);
    empleados.seleccion().mostrarCon(vista);

    expect(vista.empleadoMostrado).toEqual(andres);
  });

  it('sin seleccionar a nadie invita a elegir', async () => {
    const empleados = await empleadosRespondiendo([carolina, andres]);
    const vista = new VistaEspia();

    empleados.seleccion().mostrarCon(vista);

    expect(vista.invitaronAElegir).toBe(true);
  });

  it('un id que no corresponde a nadie invita a elegir', async () => {
    const empleados = await empleadosRespondiendo([carolina, andres]);
    const vista = new VistaEspia();

    empleados.seleccionar(99);
    empleados.seleccion().mostrarCon(vista);

    expect(vista.invitaronAElegir).toBe(true);
  });
});

class VistaEspia implements VistaDeSeleccion {
  empleadoMostrado: Empleado | null = null;
  invitaronAElegir = false;

  mostrarEmpleado(empleado: Empleado): void {
    this.empleadoMostrado = empleado;
  }

  invitarAElegir(): void {
    this.invitaronAElegir = true;
  }
}
