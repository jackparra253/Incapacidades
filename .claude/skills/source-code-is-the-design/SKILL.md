---
name: source-code-is-the-design
description: El código fuente es el diseño; programar es diseñar. Diagramas y documentación como fuente de verdad, dónde está el costo al estimar, justificar refactor o tests, diseño como fase previa.
---

# ¿Cuál es el modelo? — El código fuente es el diseño

**Definición de trabajo.** El modelo vive **en el código fuente**. El código es la única fuente de
verdad del modelo; los diagramas y documentos solo lo **visualizan**. Es la idea clásica de que
**programar es diseñar**.

## El corolario que cambia las estimaciones

**El build es gratis; el diseño es el costo.** La compilación —el paso de "manufactura"— es
esencialmente gratuita y automática. Todo el esfuerzo, la habilidad y la estimación van a producir el
código, es decir, **el diseño**.

Por eso no tiene sentido tratar "diseño" y "programación" como fases separadas con presupuestos
distintos: son la misma actividad.

## Testear y debuggear son diseño

Si el diseño es el código, entonces **testear y debuggear son actividades de diseño** — validación y
refinamiento del modelo — no una fase pegada al final ni un impuesto sobre el trabajo "real".

## Los diagramas no son el diseño

Un diagrama es una **vista** del modelo, útil para comunicar. Pero si el diagrama y el código
discrepan, el que está bien es el código: es el que ejecuta.

## El modelo es dinámico

El modelo **ejecuta y su estado cambia con el tiempo**. Diseñá teniendo en cuenta el paso del tiempo:
un objeto no es solo su forma, es también su historia de estados válidos.

## Ver también

- `what-is-software` — qué es ese modelo.
- `tdd` — testear como actividad de diseño.
- `h2-complete-objects` — el tiempo transcurre en el modelo; por eso no hay estados "a medio construir".

## Texto completo

`docs/es/02-cual-es-el-modelo.md` · `docs/en/02-where-is-the-model.md`
