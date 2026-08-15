import { Component, computed, inject } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatListModule } from '@angular/material/list';
import { MatProgressBarModule } from '@angular/material/progress-bar';

import { Empleados } from '../../application/empleados/empleados';
import { Empleado } from '../../domain/empleados/empleado';
import { ListadoEnPantalla } from './listado-en-pantalla';
import { SeleccionEnPantalla } from './seleccion-en-pantalla';

@Component({
  selector: 'app-empleados',
  imports: [MatCardModule, MatListModule, MatProgressBarModule],
  templateUrl: './pantalla-de-empleados.html',
  styleUrl: './pantalla-de-empleados.scss',
})
export class PantallaDeEmpleados {
  private readonly empleados = inject(Empleados);

  readonly listado = computed(() => ListadoEnPantalla.de(this.empleados.listado()));
  readonly seleccion = computed(() => SeleccionEnPantalla.de(this.empleados.seleccion()));

  elegir(empleado: Empleado): void {
    this.empleados.seleccionar(empleado);
  }
}
