---
name: h6-encapsulation
description: Asignar responsabilidades y aplicar Tell, Don't Ask. Getters/setters indiscriminados, preguntar datos para decidir afuera, lógica repetida en los llamadores, exponer colecciones internas, feature envy, `a.getB().getC()`.
---

# H6 — No romper el encapsulamiento

**La heurística.** Encapsular es **otorgar responsabilidades a los objetos correctamente** — no
meramente ocultar campos. El *information hiding* (el control de acceso) es solo la parte que queda
cuando le sacás las responsabilidades.

## Pensalo por lo que se rompe

- **Generás acoplamiento**: quien llama pasa a depender de la estructura interna del objeto. Es peor
  en lenguajes estáticamente tipados, donde ese acoplamiento queda fijado en el tipo.
- **Le quitás responsabilidad al objeto correcto**: su lógica se filtra hacia los llamadores y
  reaparece como **código repetido** en cada lugar que preguntó.

## La cura: Tell, Don't Ask

Agregá **un mensaje por cada cosa que un colaborador necesite saber o hacer**, en vez de exponer
datos para que decida afuera:

```
card.isExpiredOn(date)        en vez de   card.month() y card.year() y comparar
card.isOwnedBy(person)        en vez de   card.owner() == person
```

Y **no agregues getters/setters indiscriminadamente**: cada uno es una responsabilidad que estás
regalando.

## Cuando tenés que devolver un colaborador

Preferí, en este orden: **objetos inmutables**, **copias**, **wrappers**. Pero tené presente que
**ninguno elimina el acoplamiento** — solo limitan el daño.

## La parte incómoda

Los lenguajes ayudan o dificultan: la privacidad **por objeto** (Smalltalk, Ruby) ayuda; la privacidad
**por clase** y los modificadores débiles dificultan. Pero al final el encapsulamiento es **una
disciplina que el usuario del objeto debe respetar**, aun cuando el lenguaje lo deje romperlo.

## Ver también

- `h3-valid-objects`, `h4-no-null`, `h5-immutable-objects` — las tres se apoyan en encapsulamiento.
- `structural-patterns` — Decorator y Proxy como wrappers al devolver internals.
- `design-principles-solid` — responsabilidad única, inversión de dependencias.

## Texto completo

`docs/es/12-h6-encapsulamiento.md` · `docs/en/12-h6-encapsulation.md`
