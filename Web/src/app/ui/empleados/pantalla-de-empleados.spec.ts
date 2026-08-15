import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';

import { EmpleadoDto } from '../../infrastructure/empleados/empleado-dto';
import { PantallaDeEmpleados } from './pantalla-de-empleados';

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

describe('PantallaDeEmpleados', () => {
  let fixture: ComponentFixture<PantallaDeEmpleados>;

  function texto(): string {
    return (fixture.nativeElement as HTMLElement).textContent ?? '';
  }

  async function responder(empleados: EmpleadoDto[]): Promise<void> {
    TestBed.inject(HttpTestingController).expectOne('/Empleado').flush(empleados);
    await fixture.whenStable();
  }

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [provideHttpClient(), provideHttpClientTesting()],
    });

    fixture = TestBed.createComponent(PantallaDeEmpleados);
    TestBed.tick();
  });

  it('invita a elegir mientras no hay nadie seleccionado', async () => {
    await responder([carolinaDto]);

    expect(texto()).toContain('Elegí un empleado');
  });

  it('lista los empleados que responde la Api', async () => {
    await responder([carolinaDto]);

    expect(texto()).toContain('Carolina Restrepo');
  });

  it('al elegir un empleado muestra su salario desagregado', async () => {
    await responder([carolinaDto]);

    (fixture.nativeElement as HTMLElement).querySelector('mat-list-option')?.dispatchEvent(
      new MouseEvent('click', { bubbles: true }),
    );
    await fixture.whenStable();

    expect(texto()).toContain('$ 3.000.000');
    expect(texto()).toContain('Ley 50');
  });

  it('cuando la Api falla lo dice en vez de mostrar una lista vacia', async () => {
    TestBed.inject(HttpTestingController)
      .expectOne('/Empleado')
      .flush('sin servicio', { status: 500, statusText: 'Server Error' });
    await fixture.whenStable();

    expect(texto()).toContain('No se pudo consultar los empleados');
  });
});
