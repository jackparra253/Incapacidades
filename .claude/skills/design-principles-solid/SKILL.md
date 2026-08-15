---
name: design-principles-solid
description: Encapsular lo que varía, programar hacia interfaces, composición sobre herencia, y SOLID. Acoplamiento, jerarquías profundas, inyección de dependencias, interfaces gordas, clases que hacen demasiado.
---

# Principios de diseño de software

La mayoría de los patrones son **aplicaciones concretas de unos pocos principios universales**.
Aprender los principios rinde más que memorizar el catálogo.

## Los tres fundamentales

1. **Encapsular lo que varía.** Identificá los aspectos que cambian y separalos de lo que permanece
   igual, para que los cambios afecten a menos código. Es el principio del que salen Strategy, State,
   Factory Method y buena parte del resto.
2. **Programar hacia una interfaz, no hacia una implementación.** Dependé de abstracciones, para que
   los colaboradores sean **intercambiables**.
3. **Favorecer la composición sobre la herencia.** Construí comportamiento **combinando objetos** en
   lugar de hacer crecer jerarquías de clases profundas. La herencia fija en tiempo de compilación lo
   que la composición deja abierto.

## SOLID

- **S — Responsabilidad Única.** Una clase, una razón para cambiar.
- **O — Abierto/Cerrado.** Abierto a la extensión, cerrado a la modificación. Es el mismo ideal que la
  **correspondencia 1:1** del eje funcional: un caso nuevo se *agrega*, no se parchea.
- **L — Sustitución de Liskov.** Un subtipo debe poder usarse donde se espera el tipo base, sin que el
  llamador se entere.
- **I — Segregación de Interfaces.** Mejor varias interfaces chicas que una grande que obliga a
  implementar lo que no usás.
- **D — Inversión de Dependencias.** Dependé de abstracciones, no de concreciones; que el detalle
  dependa de la política y no al revés.

## La advertencia

Estos "principios" **son heurísticas**: se aplican en contexto, tienen excepciones y se sopesan entre
sí. Aplicar SOLID a rajatabla produce sistemas con más abstracciones que dominio — y eso daña el eje
descriptivo.

## Ver también

- `why-heuristics` — por qué llamarlos principios invita al dogma.
- `h6-encapsulation` — responsabilidad única y encapsulamiento son la misma idea vista desde dos lados.
- `good-model-three-axes` — abierto/cerrado dicho en términos de modelado.

## Texto completo

`design-patterns/es/02-principios-de-diseno.md` · `design-patterns/en/02-design-principles.md`
