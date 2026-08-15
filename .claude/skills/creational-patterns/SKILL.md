---
name: creational-patterns
description: Factory Method, Abstract Factory, Builder, Prototype, Singleton. Construcción compleja o condicional, constructores con muchos parámetros, `new` esparcido, familias de objetos, clonación.
---

# Patrones creacionales

Proveen **mecanismos de creación de objetos** que aumentan la flexibilidad y la reutilización. Son los
aliados naturales de **H2 (objetos completos)** y **H3 (objetos válidos)**: la creación es exactamente
donde se decide si un objeto puede nacer inválido.

## Los cinco

**Factory Method** — provee una interfaz para crear objetos en una superclase, pero permite a las
subclases alterar el tipo de objetos que se crearán.
→ *Señal*: un `switch` sobre un tipo para decidir qué instanciar.

**Abstract Factory** — permite producir **familias** de objetos relacionados sin especificar sus clases
concretas.
→ *Señal*: varios objetos que deben pertenecer al mismo "juego" y hoy se combinan mal por descuido.

**Builder** — permite construir objetos complejos **paso a paso**, produciendo distintos tipos y
representaciones con el mismo código de construcción.
→ *Señal*: un constructor con muchos parámetros, o un objeto que hoy se arma con setters —
**el Builder es la respuesta canónica de H2** al ensamblado complejo: el builder carga con las fases
y el objeto del dominio nace completo.

**Prototype** — permite copiar objetos existentes sin que el código dependa de sus clases.
→ *Señal*: clonación que hoy se hace inspeccionando el tipo desde afuera (rompe H6).

**Singleton** — asegura que una clase tenga una sola instancia, dando un punto de acceso global.
→ *Cuidado*: es el patrón que más se aplica sin tener el problema. El acceso global acopla todo con
todo y suele esconder estado mutable compartido — chocando con **H5**. Preguntá si lo que necesitás es
"una sola instancia" o simplemente "pasar la dependencia".

## Cómo elegir

**Preferí primero la opción más simple.** Muchos diseños empiezan con Factory Method y evolucionan
hacia Abstract Factory o Prototype **solo cuando aparece la presión**.

## Ver también

- `h2-complete-objects` · `h3-valid-objects` · `design-principles-solid` · `design-patterns-catalog`

## Texto completo

`design-patterns/es/03-patrones-creacionales.md` · `design-patterns/en/03-creational-patterns.md`
