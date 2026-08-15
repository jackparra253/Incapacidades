---
name: h5-immutable-objects
description: Inmutable por defecto; mutable solo si el ente cambia. Setters, estado compartido mutado, copias defensivas, aliasing, claves de hash o diccionario, concurrencia.
---

# H5 — Favorecer objetos inmutables

**La heurística.** La mutabilidad es una **decisión de modelado, no técnica**: un objeto debe ser
mutable **si y solo si el ente que representa es mutable**.

La mayoría de las cosas que pensás como objetos mutables en realidad no lo son — números, fechas,
facturas, contratos, strings. Así que **por defecto, inmutable**.

## Modelá el cambio como eventos

Muchas veces el cambio se modela mejor como una **secuencia de eventos que producen objetos nuevos**:
una llamada en curso no "se modifica", se convierte en una *nueva* llamada terminada. Una factura no
cambia: se emite una nota de crédito.

## Lo que te compra

- Dejás de preocuparte por **el paso del tiempo**: el objeto que leíste es el que sigue ahí.
- Dejás de preocuparte por **las consecuencias de entregarlo**: sin copias defensivas, sin bugs de
  aliasing.
- Seguro como **clave de hash**, y **thread-safe** sin esfuerzo.

## Cuando el ente sí cambia

Modelá la mutación deliberadamente:

1. **Evitá setters** — expresá el cambio con un mensaje del dominio, no con `setX`.
2. **Que los cambios sean atómicos y válidos** (H3): nunca un estado intermedio observable.
3. **Que el owner controle la mutación** — no cualquiera que tenga una referencia.
4. **Entregá copias o vistas inmutables** en lugar de internals mutables.
5. **Protegé identidad, igualdad y hash**: si el objeto muta, no puede ser su propia clave.

## Ver también

- `h3-valid-objects` — un cambio es una construcción: debe dejar el objeto válido.
- `h6-encapsulation` — entregar internals mutables es romper encapsulamiento.
- `h2-complete-objects` — nacer completo es lo que permite no cambiar después.

## Texto completo

`docs/es/11-h5-objetos-inmutables.md` · `docs/en/11-h5-immutable-objects.md`
