import { computed, Injectable, signal } from '@angular/core';
import { httpResource } from '@angular/common/http';

import { Empleado } from '../../domain/empleados/empleado';
import { EmpleadoSeleccionado, Seleccion, SinSeleccion } from '../../domain/empleados/seleccion';
import { EmpleadoDto } from '../../infrastructure/empleados/empleado-dto';
import { ListadoDeEmpleados } from './listado-de-empleados';

@Injectable({ providedIn: 'root' })
export class Empleados {
  private readonly consulta = httpResource<EmpleadoDto[]>(() => '/Empleado');
  private readonly elegido = signal<Seleccion>(new SinSeleccion());

  readonly listado = computed<ListadoDeEmpleados>(() => ListadoDeEmpleados.desde(this.consulta));
  readonly seleccion = this.elegido.asReadonly();

  seleccionar(empleado: Empleado): void {
    this.elegido.set(new EmpleadoSeleccionado(empleado));
  }
}
