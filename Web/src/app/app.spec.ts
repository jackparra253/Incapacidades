import { TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';

import { App } from './app';

describe('App', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [App],
      providers: [provideRouter([])],
    }).compileComponents();
  });

  it('muestra el nombre de la aplicacion en la barra', async () => {
    const fixture = TestBed.createComponent(App);
    await fixture.whenStable();

    const barra = (fixture.nativeElement as HTMLElement).querySelector('mat-toolbar');

    expect(barra?.textContent).toContain('Incapacidades');
  });
});
