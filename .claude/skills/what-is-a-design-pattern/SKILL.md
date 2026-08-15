---
name: what-is-a-design-pattern
description: Qué es y qué no es un patrón, y cómo se relaciona con H1–H6. Evaluar si introducir un patrón está justificado, distinguir intención de estructura, copiar un patrón sin su problema.
---

# ¿Qué es un patrón de diseño?

**Definición operativa.** Un patrón de diseño es una **solución general y reutilizable** a un problema
que ocurre con frecuencia en el diseño de software.

**No es un trozo de código terminado que se pega.** Es una **descripción o plano** de cómo resolver un
problema, que **adaptás** a tu programa. Dos implementaciones del mismo patrón pueden no parecerse.

## Qué contiene la descripción de un patrón

- **Intención** — el problema central que resuelve, en una o dos frases.
- **Estructura** — las clases y colaboraciones típicas.
- **Consecuencias** — qué ganás y qué pagás.

De todo eso, **lo que importa es la intención**. Dos patrones pueden compartir estructura y resolver
problemas distintos: si aprendés el diagrama y no la intención, vas a aplicar el patrón equivocado con
el diagrama correcto.

## Cómo se relacionan con las heurísticas

Trabajan juntos, en planos distintos:

- las **heurísticas** te dicen cómo construir un **modelo fiel** del dominio;
- los **patrones** te dan **estructuras nombradas y probadas** para problemas de diseño recurrentes.

En este catálogo cada patrón se interpreta contra H1–H6:

| Heurística | Qué aporta el patrón |
| --- | --- |
| H1 | evita representaciones ambiguas y responsabilidades mezcladas |
| H2/H3 | la creación o transición impide estados imposibles |
| H4 | ausencia y variación con objetos y protocolos, no sentinelas |
| H5 | compartir y comunicar con estado estable |
| H6 | "decir, no preguntar"; no exponer internals para coordinar |

## Las advertencias

- **Como las heurísticas, los patrones son herramientas, no reglas.**
- **Nunca introduzcas un patrón donde el problema no existe** — es complejidad accidental.
- **Preferí primero la opción más simple.** Muchos diseños empiezan simples y evolucionan hacia
  patrones más flexibles solo cuando aparece la presión.
- **Nombrá por el dominio**, no por el patrón: la clase representa un ente, no una entrada del catálogo.

## Ver también

- `design-patterns-catalog` · `design-principles-solid` · `why-heuristics`

## Texto completo

`design-patterns/es/01-que-es-un-patron.md` · `design-patterns/en/01-what-is-a-design-pattern.md`
