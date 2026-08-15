---
name: naming
description: Nombrar por lo que las cosas son en el dominio. Nombres genéricos (data, info, manager, helper, utils, temp), abreviaturas, nombres que describen el tipo o la implementación, objetos sin nombre.
---

# Nombramiento — Variables y clases

*Material adicional: es una práctica, no una de las seis heurísticas.*

**Programar es el arte de nombrar.** El nombre es la mitad del modelo: es lo que hace que otro humano
—o vos en seis meses— pueda leer el código y **aprender el dominio desde él**. Sirve directamente al
**eje descriptivo** y es inseparable de H1: nombrar el ente por lo que es *es* representarlo fielmente.

## Evitá

- **Nombres genéricos y de relleno**: `data`, `info`, `temp`, `item`, `value`, `obj`, `result`.
- **Sufijos que nombran la implementación, no el ente**: `Manager`, `Helper`, `Processor`, `Utils`,
  `Handler` — suelen delatar una clase que no representa nada del dominio.
- **Abreviaturas** que ahorran teclas y cuestan lecturas.
- Nombres que describen el **tipo** (`stringList`, `userMap`) en vez del **ente**.

## Preferí

- El **término del dominio**, tal como lo dice quien conoce el negocio.
- Nombres que digan **qué es**, no cómo está hecho.
- Para los mensajes: lo que el objeto **hace o responde**, no cómo lo calcula.

## No dejes objetos sin nombre

Un mapa anónimo, una tupla o un diccionario que todos saben interpretar "por convención" es un ente
del dominio **sin objeto que lo represente** (H1). Dale una clase y un nombre.

## Hacelo una práctica de equipo

Los nombres se discuten. Un nombre que solo entiende quien lo escribió no cumple su función.

## Ver también

- `h1-object-per-entity` — nombrar el ente por lo que es es parte de representarlo.
- `tdd` — el refactor es donde aparecen los nombres.
- `good-model-three-axes` — el eje descriptivo.

## Texto completo

`docs/es/14-nombramiento.md` · `docs/en/14-naming.md`
