---
name: designing-with-objects
description: Punto de entrada al catálogo: enruta a la heurística o patrón que corresponde. Preguntas amplias de diseño, revisar un modelo, o cuando no sabés qué heurística aplica.
---

# Diseñar software con objetos — índice

**La idea central.** El software es **un modelo computable de un dominio de problema de la
realidad** — no "un conjunto de instrucciones". De ahí se sigue todo lo demás: buen software significa
**buen modelo**, y el modelo **es el código fuente**.

## Cómo elegir dónde mirar

| Si el problema es… | Consultá |
| --- | --- |
| El modelo no se parece al dominio; primitivas por todos lados | `h1-object-per-entity` |
| Objetos que se arman por partes, setters, orden oculto de llamadas | `h2-complete-objects` |
| Dónde validar; se pueden construir objetos imposibles | `h3-valid-objects` |
| `null`, ausencia, campos opcionales | `h4-no-null` |
| Estado que cambia, concurrencia, aliasing | `h5-immutable-objects` |
| Getters/setters, lógica que se fugó a los llamadores | `h6-encapsulation` |
| Cómo nombrar lo que acabo de extraer | `naming` |
| Por dónde empiezo a escribir esto | `tdd` |
| Un problema de diseño recurrente y conocido | `design-patterns-catalog` |
| Dónde poner la lógica de negocio en la app Angular | `angular-architecture` |

## El argumento, de arriba a abajo

1. El software es un **modelo computable de la realidad** → `what-is-software`
2. Ese modelo **es el código fuente** → `source-code-is-the-design`
3. Se juzga por tres ejes, anclando en el **funcional** → `good-model-three-axes`
4. Se refina **aprendiendo**, con feedback corto → `development-as-learning`
5. Su unidad es el **objeto**, definido por sus **mensajes** → `what-is-an-object`
6. Todo el consejo son **heurísticas**, no reglas → `why-heuristics`

Y las seis heurísticas mantienen honesto el eje funcional a lo largo de la vida del sistema:
representá cada ente **fielmente y 1:1** (H1), traé los objetos a la existencia **completos** (H2) y
**válidos** (H3), rechazá la gran mentira representacional de **null** (H4), dejá que los objetos
cambien solo como cambian sus entes (H5), y protegé las responsabilidades **no rompiendo el
encapsulamiento** (H6).

## Texto completo

`docs/es/00-resumen.md` · `docs/en/00-overview.md`
