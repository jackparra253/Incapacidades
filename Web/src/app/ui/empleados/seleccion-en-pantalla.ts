import { Empleado } from '../../domain/empleados/empleado';
import { Seleccion, VistaDeSeleccion } from '../../domain/empleados/seleccion';

export abstract class SeleccionEnPantalla {
  static de(seleccion: Seleccion): SeleccionEnPantalla {
    const recolector = new Recolector();
    seleccion.mostrarCon(recolector);

    return recolector.enPantalla();
  }

  abstract hayEmpleadoElegido(): boolean;
  abstract nombre(): string;
  abstract salarioMensual(): string;
  abstract salarioDiario(): string;
  abstract tipoDeSalario(): string;
  abstract esElElegido(empleado: Empleado): boolean;
}

class ElegidoEnPantalla extends SeleccionEnPantalla {
  constructor(private readonly empleado: Empleado) {
    super();
  }

  hayEmpleadoElegido(): boolean {
    return true;
  }

  nombre(): string {
    return this.empleado.nombreCompleto();
  }

  salarioMensual(): string {
    return this.empleado.salario().mensual().comoTexto();
  }

  salarioDiario(): string {
    return this.empleado.salario().diario().comoTexto();
  }

  tipoDeSalario(): string {
    return this.empleado.tipoSalario().nombre();
  }

  esElElegido(empleado: Empleado): boolean {
    return this.empleado.esElMismoQue(empleado);
  }
}

class NadieEnPantalla extends SeleccionEnPantalla {
  hayEmpleadoElegido(): boolean {
    return false;
  }

  nombre(): string {
    return '';
  }

  salarioMensual(): string {
    return '';
  }

  salarioDiario(): string {
    return '';
  }

  tipoDeSalario(): string {
    return '';
  }

  esElElegido(): boolean {
    return false;
  }
}

class Recolector implements VistaDeSeleccion {
  private recolectada: SeleccionEnPantalla = new NadieEnPantalla();

  enPantalla(): SeleccionEnPantalla {
    return this.recolectada;
  }

  mostrarEmpleado(empleado: Empleado): void {
    this.recolectada = new ElegidoEnPantalla(empleado);
  }

  invitarAElegir(): void {
    this.recolectada = new NadieEnPantalla();
  }
}
