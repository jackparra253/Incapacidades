export class MonedaInvalida extends Error {
  constructor(moneda: string) {
    super(`La moneda '${moneda}' no es un código de tres letras.`);
    this.name = 'MonedaInvalida';
  }
}
