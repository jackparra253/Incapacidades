export class TipoSalarioDesconocido extends Error {
  constructor(tipo: number) {
    super(`La Api informó el tipo de salario '${tipo}', que no corresponde a Ley 50 ni a Integral.`);
    this.name = 'TipoSalarioDesconocido';
  }
}
