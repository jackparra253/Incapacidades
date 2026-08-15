---
name: what-is-software
description: El software como modelo computable de un dominio de la realidad. Qué parte del dominio modelar y con cuánto detalle, alcance y límites, código organizado alrededor del framework en vez del negocio.
---

# ¿Qué es el software?

**Definición de trabajo.** El software es **un modelo computable de un dominio de problema de la
realidad**. No es "un conjunto de instrucciones para una máquina".

Esta sola definición reorienta el trabajo: no estás escribiendo pasos, estás **construyendo un
modelo** que representa cierta porción de un dominio real (o conceptual) y que una computadora puede
ejecutar.

## Las cuatro nociones

- **Realidad** — lo que existe, con toda su ambigüedad y contexto.
- **Dominio de problema** — la porción de realidad que elegís modelar. Elegirla **es una decisión de
  diseño**: rebanás la realidad a propósito.
- **Modelo** — la representación de esa porción. **El modelo representa; no es la cosa.** Mantené el
  mapa distinto del territorio.
- **Computable** — el modelo tiene que poder ejecutarse. Por eso es un artefacto formal, no una
  descripción.

## Las dos consecuencias prácticas

1. **Especificá el *qué* e implementá el *cómo*.** El software debe ser ejecutable, así que el modelo
   es siempre formal y ejecutable — nunca un documento.
2. **El diseño es el paso deliberado de un dominio ambiguo y contextual a un modelo formal y
   ejecutable.** Esa traducción *es* el trabajo difícil y valioso.

## Cómo se usa esto al programar

Cuando el dominio y el framework piden cosas distintas, **favorecé el dominio**. Organizá el código
alrededor del negocio que modelás, no alrededor de las carpetas que propone la herramienta.

## Ver también

- `source-code-is-the-design` — dónde vive ese modelo.
- `good-model-three-axes` — cómo se juzga si el modelo es bueno.
- `what-is-an-object` — cuál es su unidad.

## Texto completo

`docs/es/01-que-es-software.md` · `docs/en/01-what-is-software.md`
