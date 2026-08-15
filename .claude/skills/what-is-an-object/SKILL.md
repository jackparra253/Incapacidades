---
name: what-is-an-object
description: Un objeto se define por los mensajes que responde, no por sus datos. Diseñar la interfaz pública, structs anémicos, elegir entre exponer campos y ofrecer comportamiento.
---

# ¿Qué es un objeto?

**Definición de trabajo.** Un objeto es **la representación esencial de un ente del dominio** — no
"código + datos".

Un objeto se define por **los mensajes a los que responde** (su **protocolo**), no por los datos que
guarda adentro. La disciplina es capturar la **esencia** del ente y quitar lo **incidental**.

## Las dos preguntas del diseño de objetos

1. **¿Qué debo modelar?** Qué entes del dominio elegís representar, y con qué nivel de detalle.
2. **¿Cómo lo debo modelar?** Qué mensajes responde ese objeto — es decir, qué responsabilidades tiene.

## Por qué "mensajes" y no "datos"

Porque los datos son **una** forma de cumplir el protocolo, no el objeto en sí. Si definís el objeto
por sus datos, cualquier cambio interno se convierte en un cambio de interfaz, y todo el que lo use
queda acoplado a una decisión que debería haber sido privada.

Esta postura es la que hace **coherentes** a las seis heurísticas: si un objeto *es* su protocolo,
entonces ocultar sus datos (H6), controlar su construcción (H2) y rechazar estados inválidos (H3) son
todas formas de **mantener honesto ese protocolo**.

## La prueba

Escribí primero los mensajes que el objeto debería responder, sin pensar en campos. Si no podés
nombrar ninguno más allá de `getX`/`setX`, todavía no tenés un objeto: tenés una estructura de datos
con ambiciones.

## Ver también

- `h6-encapsulation` — asignar responsabilidades es el corazón del asunto.
- `h1-object-per-entity` — un objeto por ente.
- `naming` — el protocolo se lee en los nombres de los mensajes.

## Texto completo

`docs/es/05-que-es-un-objeto.md` · `docs/en/05-what-is-an-object.md`
