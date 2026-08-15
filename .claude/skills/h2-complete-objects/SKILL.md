---
name: h2-complete-objects
description: Objetos completos desde su creación. Setters post-construcción, constructores vacíos, campos sin inicializar, acoplamiento temporal, init/setUp/configure separados del `new`.
---

# H2 — Crear objetos completos

**La heurística.** Un objeto debe ser válido y fiel **desde el momento en que existe**. Como el
modelo es dinámico y el tiempo transcurre en él, no hay un estado legítimo "a medio construir".

## Qué hacer

- **Pedí en el constructor todo lo que el objeto necesita** para ser él mismo.
- **Evitá los setters post-construcción** que terminan el trabajo después de crear el objeto.
- Para un ensamblado genuinamente complejo, usá un **Builder**: el builder carga con la complejidad
  del armado y el objeto del dominio nace completo.

## El problema que evita

- **Acoplamiento temporal**: los mensajes deben enviarse en un orden oculto que el código no expresa
  y que nadie te obliga a respetar.
- **Estados inválidos transitorios**: entre el `new` y el último setter, el objeto existe mintiendo
  sobre el ente que representa.

## La señal reveladora

**Las variables sin inicializar suelen indicar que estás modelando dos entes como uno.** Si un campo
está vacío durante media vida del objeto, casi siempre hay dos entes distintos: una llamada *en
curso* y una llamada *terminada*; un pedido *abierto* y un pedido *despachado*. Partilos en dos
objetos, cada uno completo.

## Por qué importa

Un objeto completo **enseña**: su constructor documenta qué necesita el ente para existir. Un objeto
que se arma por partes no le enseña nada a quien lo lee.

## Ver también

- `h3-valid-objects` — completo no alcanza; además debe ser válido.
- `h5-immutable-objects` — si nace completo y no cambia, desaparece toda una clase de bugs.
- `creational-patterns` — Builder, Factory Method.

## Texto completo

`docs/es/08-h2-objetos-completos.md` · `docs/en/08-h2-complete-objects.md`
