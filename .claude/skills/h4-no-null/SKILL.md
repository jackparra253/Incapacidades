---
name: h4-no-null
description: Modelar la ausencia como objeto. null, nil, None, nullptr, `if x == null`, safe navigation (`?.`, `&.`), NullPointerException, campos opcionales, punteros nilables, Optional/Maybe.
---

# H4 — No usar null

**La heurística.** Si los objetos son completos (H2) y válidos (H3), ¿debería una referencia ser
alguna vez `null`? **No.**

`null` es la violación canónica de H1: un solo símbolo sobrecargado significa a la vez "variable no
inicializada", "el cliente no tiene dirección" y "nada". No representa fielmente a ninguno de los
tres — y se filtra al mundo real como los bugs que todos vimos (`null95` en una pantalla, "(null)
could not be found").

## Qué hacer

- **Modelá la ausencia como un objeto.** Reemplazá `if x.nil?` con **polimorfismo** vía un
  **Null Object**, y **nombralo por el dominio**: `NotProvidedAddress`, no `NullAddress`. El nombre
  debe decir *qué significa esa ausencia aquí*, que rara vez es "nada".
- **En lenguajes estáticamente tipados**, hacé explícito el "quizás ausente" con **Optional/Maybe**,
  para que el tipo diga la verdad.
- **Mantené la ausencia adentro del objeto.** Preferí un *Explicit Absent Message* antes que un
  safe-navigation operator (`?.`, `&.`): el operador de navegación segura filtra hacia afuera la
  decisión que le corresponde al objeto.
- **No escribas chequeos defensivos por null** sobre parámetros que un objeto válido nunca te pasaría.

## Cuándo no aplica tal cual

En los bordes con sistemas que sí devuelven null (drivers, JSON, SDKs de terceros) el null existe y
no lo controlás. Convertilo a un objeto del dominio **en el borde**, una sola vez, y que no entre al
modelo.

## Ver también

- `h1-object-per-entity` — null rompe H1 por definición: un símbolo, muchos significados.
- `h3-valid-objects` — un objeto válido no necesita que lo defiendan de null.
- `h6-encapsulation` — el safe navigation es romper encapsulamiento con azúcar sintáctico.
- `behavioral-patterns` / `structural-patterns` — Null Object, Strategy, State.

## Texto completo

`docs/es/10-h4-no-usar-null.md` · `docs/en/10-h4-no-null.md`
