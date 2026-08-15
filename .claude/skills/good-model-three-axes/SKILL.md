---
name: good-model-three-axes
description: Juzgar un modelo por tres ejes —implementativo, descriptivo, funcional— anclando en el funcional. Code review, comparar dos diseños, correspondencia 1:1 dominio-modelo, abierto-cerrado.
---

# ¿Qué es un buen modelo? — Los tres ejes

Si el software es un modelo, **"buen software" significa "buen modelo"**. Juzgalo por tres ejes.

## Los tres ejes

1. **Implementativo — *cómo ejecuta*.** Performance, uso de recursos, solidez técnica.
2. **Descriptivo — *cuán entendible es*.** Nombres, lenguaje del dominio, habitabilidad. La pregunta
   concreta: **¿puede un humano leerlo y aprender el dominio desde él?**
3. **Funcional — *cuán fielmente representa el dominio*.** ¿El modelo se corresponde con la realidad?

Los tres importan, pero **el eje funcional es el ancla**. Un sistema rapidísimo y prolijo que
representa mal el dominio va a resistirse a cada cambio que el negocio pida.

## La prueba práctica: correspondencia 1:1

**Buscá una correspondencia 1:1 entre dominio y modelo.** Cuando la correspondencia es correcta:

- un **caso nuevo del dominio** debería **agregarse** al modelo, no parchearse;
- **un cambio en el dominio** debería mapear a **un cambio en el modelo**.

Es el ideal **abierto–cerrado** dicho en términos de modelado. Y da un test concreto para revisiones:
si agregar un caso de negocio obliga a tocar seis archivos y meter un `if` en tres lugares, la
correspondencia está rota — sin importar qué tan limpio se vea el código.

## Cómo usarlo en un code review

Preguntá en este orden: ¿representa fielmente el dominio? ¿Puedo aprender el dominio leyéndolo?
¿Ejecuta bien? Los defectos del eje funcional son los caros; los del implementativo suelen ser locales.

## Ver también

- `h1-object-per-entity` — el eje funcional hecho concreto.
- `naming` — el eje descriptivo hecho concreto.
- `development-as-learning` — por qué el modelo cambia con el tiempo.

## Texto completo

`docs/es/03-buen-modelo.md` · `docs/en/03-good-model.md`
