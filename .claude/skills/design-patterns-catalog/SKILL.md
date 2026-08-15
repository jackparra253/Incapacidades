---
name: design-patterns-catalog
description: Índice de los 23 patrones GoF con su intención. El problema suena a un patrón conocido pero no sabés cuál, comparar patrones de estructura parecida, verificar que el problema existe.
---

# Catálogo de patrones de diseño — índice

Un **patrón de diseño** es una solución general y reutilizable a un problema recurrente. **No es
código que se pega**: es un plano que adaptás.

**La pregunta para elegir no es "¿cuál conozco?", sino: ¿qué heurística está bajo presión acá y qué
patrón la protege mejor?**

## Creacionales → `creational-patterns`

- **Factory Method** — interfaz para crear objetos en una superclase, dejando a las subclases alterar el tipo.
- **Abstract Factory** — familias de objetos relacionados sin especificar clases concretas.
- **Builder** — construir objetos complejos paso a paso.
- **Prototype** — copiar objetos existentes sin depender de sus clases.
- **Singleton** — una sola instancia con punto de acceso global.

## Estructurales → `structural-patterns`

- **Adapter** — hacer colaborar interfaces incompatibles.
- **Bridge** — separar abstracción e implementación en dos jerarquías independientes.
- **Composite** — árboles que se tratan como objetos individuales.
- **Decorator** — añadir comportamiento envolviendo el objeto.
- **Facade** — interfaz simplificada sobre un conjunto complejo.
- **Flyweight** — compartir estado común para que entren más objetos en memoria.
- **Proxy** — sustituto que controla el acceso a otro objeto.

## De comportamiento → `behavioral-patterns`

- **Chain of Responsibility** — cadena de manejadores; cada uno procesa o pasa.
- **Command** — la solicitud como objeto: colas, registro, deshacer.
- **Iterator** — recorrer sin exponer la representación interna.
- **Mediator** — colaborar vía un mediador en vez de en malla.
- **Memento** — guardar y restaurar estado sin revelar la implementación.
- **Observer** — suscripción y notificación de eventos.
- **State** — cambiar de comportamiento al cambiar de estado interno.
- **Strategy** — familia de algoritmos intercambiables.
- **Template Method** — esqueleto en la superclase, pasos en las subclases.
- **Visitor** — separar algoritmos de los objetos sobre los que operan.

## La regla de oro

**Recurrí a un patrón solo cuando el problema existe.** Un patrón sin su problema es complejidad
accidental y **daña el eje descriptivo del modelo**.

## Ver también

- `what-is-a-design-pattern` · `design-principles-solid` · `why-heuristics`

## Texto completo

`design-patterns/es/00-resumen.md` · `design-patterns/en/00-overview.md`
