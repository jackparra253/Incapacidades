import { computed, Injectable, signal } from '@angular/core';
import { httpResource } from '@angular/common/http';

import { Empleado } from './empleado';
import { EmpleadoSeleccionado, Seleccion, SinSeleccion } from './seleccion';

@Injectable({ providedIn: 'root' })
export class Empleados {
  private readonly consulta = httpResource<Empleado[]>(() => '/Empleado', { defaultValue: [] });
  private readonly idSeleccionado = signal(0);

  readonly todos = this.consulta.value;

  readonly seleccion = computed<Seleccion>(() => {
    const elegido = this.todos().find((empleado) => empleado.id === this.idSeleccionado());

    return elegido ? new EmpleadoSeleccionado(elegido) : new SinSeleccion();
  });

  seleccionar(id: number): void {
    this.idSeleccionado.set(id);
  }
}
