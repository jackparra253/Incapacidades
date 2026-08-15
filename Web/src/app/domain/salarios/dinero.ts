import { MonedaInvalida } from './moneda-invalida';

const ESPACIOS_DUROS = /[\u00a0\u202f]/g;

export class Dinero {
  static de(cantidad: number, moneda: string): Dinero {
    if (!Dinero.esMonedaValida(moneda)) throw new MonedaInvalida(moneda);

    return new Dinero(cantidad, moneda.toUpperCase());
  }

  private constructor(
    private readonly cantidad: number,
    private readonly moneda: string,
  ) {}

  esIgualA(otro: Dinero): boolean {
    return this.redondeada() === otro.redondeada() && this.moneda === otro.moneda;
  }

  comoTexto(): string {
    return new Intl.NumberFormat('es-CO', {
      style: 'currency',
      currency: this.moneda,
      maximumFractionDigits: 0,
    })
      .format(this.cantidad)
      .replace(ESPACIOS_DUROS, ' ');
  }

  private redondeada(): number {
    return Math.round(this.cantidad * 100) / 100;
  }

  private static esMonedaValida(moneda: string): boolean {
    return /^[a-zA-Z]{3}$/.test(moneda);
  }
}
