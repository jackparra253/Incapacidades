import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'empleados' },
  {
    path: 'empleados',
    loadComponent: () =>
      import('./ui/empleados/pantalla-de-empleados').then((modulo) => modulo.PantallaDeEmpleados),
  },
];
