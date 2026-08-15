---
name: development-as-learning
description: El desarrollo como aprendizaje iterativo; el cambio es esencial, no accidental. Requisitos poco claros, iteración vs. big design up front, deuda técnica, diseñar el ciclo de feedback.
---

# El desarrollo de software como proceso de aprendizaje

**Definición de trabajo.** Casi nunca entendés un dominio por completo de entrada. Por eso el
desarrollo **es un proceso de aprendizaje**: trabajá iterativa e incrementalmente, fundá tu
conocimiento en **hechos concretos**, y hacé explícito el **conocimiento tácito** del dominio *en el
modelo mismo*.

## El cambio es esencial, no accidental

Tres cosas cambian a la vez y ninguna se puede congelar:

- el **dominio** cambia;
- tu **comprensión** del dominio cambia;
- tu **modelado** de esa comprensión cambia.

Así que el objetivo no es acertar el diseño de entrada. Es **diseñar para que el cambio siga siendo
barato** y para **preservar tu capacidad de seguir aprendiendo**.

## El motor: el ciclo de feedback corto

El aprendizaje se acelera con **feedback inmediato** — tests rápidos, una consola viva. Eso te permite
aplicar el método científico muchas veces por hora:

**caracterizar → hipotetizar → predecir → experimentar**

Un ciclo lento no es un ciclo peor: es *otra actividad*, en la que dejás de experimentar y empezás a
adivinar.

## El modelo es dinámico

El modelo ejecuta y **su estado cambia con el tiempo**. Diseñá teniendo eso en cuenta: los estados
por los que pasa un objeto son parte de lo que estás modelando.

## Ver también

- `tdd` — la práctica que encarna este ciclo.
- `source-code-is-the-design` — testear y debuggear son diseño.
- `good-model-three-axes` — un caso nuevo debería agregarse, no parchearse.

## Texto completo

`docs/es/04-desarrollo-de-software.md` · `docs/en/04-software-development.md`
