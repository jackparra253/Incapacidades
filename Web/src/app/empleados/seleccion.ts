import { Empleado } from './empleado';

export interface VistaDeSeleccion {
  mostrarEmpleado(empleado: Empleado): void;
  invitarAElegir(): void;
}

export interface Seleccion {
  mostrarCon(vista: VistaDeSeleccion): void;
}

export class EmpleadoSeleccionado implements Seleccion {
  constructor(private readonly empleado: Empleado) {}

  mostrarCon(vista: VistaDeSeleccion): void {
    vista.mostrarEmpleado(this.empleado);
  }
}

export class SinSeleccion implements Seleccion {
  mostrarCon(vista: VistaDeSeleccion): void {
    vista.invitarAElegir();
  }
}
