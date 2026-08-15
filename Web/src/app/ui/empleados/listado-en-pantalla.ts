import { Empleado } from '../../domain/empleados/empleado';
import { ListadoDeEmpleados, VistaDeListado } from '../../application/empleados/listado-de-empleados';

export class ListadoEnPantalla implements VistaDeListado {
  static de(listado: ListadoDeEmpleados): ListadoEnPantalla {
    const pantalla = new ListadoEnPantalla();
    listado.mostrarCon(pantalla);

    return pantalla;
  }

  private cargando = false;
  private motivo = '';
  private empleados: readonly Empleado[] = [];

  estaCargando(): boolean {
    return this.cargando;
  }

  fallo(): boolean {
    return this.motivo !== '';
  }

  motivoDelError(): string {
    return this.motivo;
  }

  hayEmpleados(): boolean {
    return this.empleados.length > 0;
  }

  empleadosAMostrar(): readonly Empleado[] {
    return this.empleados;
  }

  mostrarEmpleados(empleados: readonly Empleado[]): void {
    this.empleados = empleados;
  }

  avisarQueCarga(): void {
    this.cargando = true;
  }

  avisarDelError(motivo: string): void {
    this.motivo = motivo;
  }
}
