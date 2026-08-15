---
name: angular-architecture
description: Arquitectura Angular v22 con TDD aplicando H1-H6 — núcleo de dominio sin @angular/*, puertos y adaptadores, Vitest. Angular architecture, signals, zoneless, OnPush, signal forms, httpResource, standalone components, DTO mapping, TestBed, dónde poner la lógica de negocio en una app Angular.
---

# Arquitectura Angular v22 — heurísticas + TDD

**La premisa.** Angular es un **adaptador de entrega**, no el modelo. Sus idiomas empujan en contra de
las heurísticas: componentes que nacen vacíos (H2), DTOs anémicos (H1), `undefined` mientras carga
(H4), señales mutables (H5), templates que leen campos y deciden (H6). No pelees: **contené**.

## La forma

Cuatro capas, y **todo apunta hacia adentro**:

```
domain/<contexto>/         entes, value objects, puertos    ← cero imports de @angular/*
application/<contexto>/    casos de uso sobre los puertos
infrastructure/<contexto>/ adaptadores HTTP/storage + mapeadores de DTO
ui/<feature>/              componentes standalone, signal forms, rutas
```

Hacé cumplir la frontera con una regla de ESLint, no con disciplina.

## Las cinco decisiones que importan

| | Decisión |
| --- | --- |
| H1 | **El DTO muere en el mapeador.** Un solo lugar conoce el JSON; nada arriba ve `OrderDto`. |
| H3 | El validador de signal forms **le pregunta al ente** (`EmailAddress.isValid(...)`), no duplica la regla. |
| H4 | Los estados de `httpResource` (`isLoading`/`error`/`value`) se convierten en objetos (`Loading`/`Loaded`/`Failed`) en la frontera. |
| H5 | La señal es la única celda mutable y guarda **objetos inmutables**: `set` de uno nuevo, nunca mutar. |
| H6 | El template hace **una pregunta con nombre** — `computed(() => order().canBeCancelledBy(user()))` — no cinco lecturas de campos. |

Los puertos se nombran por el **rol de dominio** (`Orders`), no por la implementación (`OrderService`).

## TDD con Vitest, alineado a las capas

| Capa | Cómo | Costo |
| --- | --- | --- |
| `domain/` | Vitest pelado, sin `TestBed`. **La mayoría de los tests.** | ~1 ms |
| `application/` | Fakes en memoria de los puertos (`InMemoryOrders`), **no mocks**. | ~ms |
| `ui/` | `TestBed` + harnesses, sobre comportamiento observable. Pocos. | ~100 ms |

Con zoneless por defecto: `await fixture.whenStable()`. Necesitar `fakeAsync` suele indicar que esa
lógica va en una capa sin fixture. **El refactor es donde nace y se nombra el ente.**

## Las únicas excepciones legítimas

Componentes vacíos (H2), DTOs anémicos (H1), `undefined`/`null` que vienen de la API (H4), señales
mutables (H5) — cada una **solo en su capa**. Lo demás que rompa una heurística no es excepción: es
una decisión que todavía no tomaste.

## Olores

`interface Order` como tipo de dominio · reglas de negocio en un `@if` · `value() ?? []` · un servicio
con `signal<Order | null>(null)` y setters · `TestBed` en casi todos los tests · `domain/` importando
`@angular/core`.

## Ver también

- `tdd` — el ciclo; `naming` — nombrar en el refactor.
- `h1-object-per-entity`, `h3-valid-objects`, `h4-no-null`, `h5-immutable-objects`, `h6-encapsulation`.
- `why-heuristics` — en una app chica, colapsar `application/` es un trade-off legítimo.

## Texto completo

`docs/es/15-arquitectura-angular.md` · `docs/en/15-angular-architecture.md`
