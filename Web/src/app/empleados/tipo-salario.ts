export const Ley50 = 1;
export const Integral = 2;

export interface TipoSalario {
  tipo: typeof Ley50 | typeof Integral;
  porcentajeSalario: number;
  porcentajeCompensacion: number;
}
