---
name: structural-patterns
description: Adapter, Bridge, Composite, Decorator, Facade, Flyweight, Proxy. Interfaces incompatibles, envolver un objeto para añadir comportamiento, estructuras de árbol, controlar el acceso.
---

# Patrones estructurales

Explican **cómo ensamblar objetos y clases en estructuras mayores** manteniéndolas flexibles y
eficientes. Son los aliados de **H6**: casi todos existen para que un objeto colabore con otro **sin
exponer internals**.

## Los siete

**Adapter** — permite que objetos con interfaces incompatibles colaboren.
→ *Señal*: una biblioteca externa cuyo protocolo no es el de tu dominio. El adapter es también el
lugar donde convertir el `null` de terceros en un objeto del dominio (**H4**).

**Bridge** — divide una clase grande (o un conjunto de clases relacionadas) en dos jerarquías
—abstracción e implementación— desarrolladas de forma independiente.
→ *Señal*: una jerarquía que se multiplica al combinar dos dimensiones de variación.

**Composite** — compone objetos en estructuras de árbol y permite trabajarlas **como si fueran objetos
individuales**.
→ *Señal*: código que pregunta "¿esto es una hoja o un nodo?" antes de actuar (rompe H6).

**Decorator** — añade comportamientos nuevos colocando el objeto **dentro de objetos envoltorio**.
→ *Señal*: variantes de comportamiento que hoy se resuelven con flags o subclases combinatorias. Es
uno de los **wrappers** que recomienda H6 al devolver un colaborador.

**Facade** — provee una interfaz simplificada a una biblioteca, framework o conjunto complejo de clases.
→ *Cuidado*: una facade sobre un diseño malo lo esconde, no lo arregla.

**Flyweight** — permite meter más objetos en memoria compartiendo partes comunes de estado.
→ *Requisito*: el estado compartido tiene que ser **inmutable** (**H5**), o el patrón es un generador
de bugs de aliasing.

**Proxy** — provee un sustituto o marcador de posición que **controla el acceso** a otro objeto.
→ *Señal*: carga perezosa, control de acceso, caché, logging alrededor de un colaborador.

## Decorator vs. Proxy

Misma estructura, **intención distinta**: el Decorator **añade** comportamiento; el Proxy **controla el
acceso**. Es el ejemplo clásico de por qué se aprende la intención y no el diagrama.

## Ver también

- `h6-encapsulation` · `h4-no-null` · `h5-immutable-objects` · `design-patterns-catalog`

## Texto completo

`design-patterns/es/04-patrones-estructurales.md` · `design-patterns/en/04-structural-patterns.md`
