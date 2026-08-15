---
name: behavioral-patterns
description: Chain of Responsibility, Command, Iterator, Mediator, Memento, Observer, State, Strategy, Template Method, Visitor. Condicionales sobre tipo o estado, algoritmos intercambiables, deshacer, máquinas de estado.
---

# Patrones de comportamiento

Se ocupan de los **algoritmos y de la asignación de responsabilidades entre objetos**. Es la familia
que más directamente sirve a **H6** (dar la responsabilidad al objeto correcto) y a **H4** (reemplazar
condicionales por polimorfismo).

## Los diez

**Chain of Responsibility** — pasa solicitudes por una cadena de manejadores; cada uno decide
procesarla o pasarla.
→ *Señal*: un `if/else if` largo que prueba condiciones en orden.

**Command** — convierte una solicitud en un objeto autónomo, habilitando colas, registro y deshacer.
→ *Señal*: necesitás deshacer, reintentar o auditar acciones.

**Iterator** — recorre los elementos de una colección **sin exponer su representación interna**.
→ Es H6 aplicado a colecciones: recorrer sin entregar la estructura.

**Mediator** — reduce dependencias caóticas forzando a los objetos a colaborar **vía un mediador**.
→ *Cuidado*: un mediador que crece sin límite se vuelve el objeto-dios que querías evitar.

**Memento** — guarda y restaura el estado previo de un objeto **sin revelar su implementación**.
→ La alternativa preferida cuando "guardar el estado" tentaría a exponer los campos (H6).

**Observer** — define un mecanismo de suscripción para notificar a varios objetos sobre eventos.
→ *Señal*: acoplamiento en una dirección que debería ser una notificación.

**State** — permite que un objeto altere su comportamiento cuando cambia su estado interno.
→ *Señal*: métodos que empiezan con `switch (estado)`. Se combina con **H2**: muchas veces "dos
estados" son en realidad **dos objetos completos** distintos, y conviene revisar eso antes.

**Strategy** — define una familia de algoritmos intercambiables, cada uno en su propia clase.
→ Es la forma canónica de **encapsular lo que varía**.

**Template Method** — define el esqueleto de un algoritmo en una superclase, dejando a las subclases
redefinir pasos concretos.
→ *Señal*: varias funciones con la misma secuencia y dos líneas distintas.
→ *Cuidado*: acopla por herencia; evaluá Strategy (composición) primero.

**Visitor** — separa los algoritmos de los objetos sobre los que operan.
→ *Cuidado*: es el que más tensiona **H6** — suele necesitar exponer estructura. Justificalo bien.

## State vs. Strategy

Misma estructura, **intención distinta**: en Strategy el cliente elige el algoritmo; en State el objeto
cambia solo de comportamiento al cambiar su estado.

## Ver también

- `h4-no-null` (Null Object, polimorfismo en vez de condicionales) · `h6-encapsulation` ·
  `design-principles-solid` · `design-patterns-catalog`

## Texto completo

`design-patterns/es/05-patrones-de-comportamiento.md` · `design-patterns/en/05-behavioral-patterns.md`
