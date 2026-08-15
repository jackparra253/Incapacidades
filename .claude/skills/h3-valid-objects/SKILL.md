---
name: h3-valid-objects
description: Hacer imposible construir objetos inválidos y fallar rápido. Dónde validar (dominio vs. controller/DTO/formulario), factory methods, validación duplicada entre REST/batch/UI, excepciones vs. códigos de retorno.
---

# H3 — Solo crear objetos válidos

**La heurística.** El modelo debe hacer **imposible construir** objetos inválidos y **fallar rápido**
cuando la creación es incorrecta. No deberías poder construir un 31 de febrero, una fracción sobre
cero, ni una tarjeta con dueño vacío.

## La jugada de mayor valor: *dónde* vive la validación

**Ponela en el modelo de dominio, no en los bordes.** Una sola regla sirve entonces a la API REST, al
proceso batch y a la UI por igual. Validar en cada borde multiplica los tests, y tarde o temprano un
borde queda desactualizado y admite un objeto que el dominio considera imposible.

## Qué hacer

- **Validá en un método de creación de instancia** —idealmente un método de clase polimórfico— en
  lugar del constructor crudo. El constructor arma; el método de creación decide si corresponde armar.
- **Señalá errores con excepciones, no con códigos de retorno.** Un código de retorno se puede
  ignorar; el objeto inválido sigue viaje.
- **Elegí la estrategia de error según el consumidor**: parar en el primer error para llamadas
  internas; **recolectar todos los errores** cuando del otro lado hay una UI y el usuario merece ver
  todo lo que está mal de una vez.

## Por qué importa

Con objetos válidos por construcción, el resto del sistema deja de defenderse. Los chequeos
defensivos, los `if` de sanidad y los tests de "¿y si viene vacío?" dejan de tener sentido porque el
caso no puede existir.

## Ver también

- `h2-complete-objects` — completo es el requisito previo de válido.
- `h4-no-null` — los chequeos defensivos por null son el síntoma de validar en el borde equivocado.
- `h6-encapsulation` — la validación vive donde vive la responsabilidad.

## Texto completo

`docs/es/09-h3-objetos-validos.md` · `docs/en/09-h3-valid-objects.md`
