---
name: bitakora
description: Contexto del proyecto Incapacidades (dominio Bitákora, nómina colombiana): en qué carpeta del dominio va cada clase, la regla de dependencias, y el glosario del negocio — PILA, EPS, AFP, ARL, recobro, devengado, deducciones, IBC, SMLMV.
---

# Bitákora — el proyecto Incapacidades

**Liquida incapacidades laborales bajo la normativa colombiana**: dado un empleado, un tipo de
incapacidad y una duración, reparte el pago entre empresa, EPS y ARL en períodos contiguos.

Este skill es el contexto **específico del proyecto**. Las heurísticas genéricas viven en
`designing-with-objects` y en `h1-object-per-entity` … `h6-encapsulation`; no se repiten acá.

## La regla de oro

> `Bitakora` **no** conoce la Api. La Api **no** sabe de reglas de negocio.

Solo quedan tres proyectos, y la única dependencia entre ellos es `Api → Bitakora` (más `Test`, que
ve a los dos). Al agregar una clase la pregunta ya no es *en qué capa va* sino **de qué ente del
negocio habla**, y esa respuesta es la carpeta.

## Dónde vive cada cosa (reorganizado por dominio el 2026-08-06)

Hasta ese día el repo tenía 7 proyectos partidos por capa técnica (`Modelos`, `Aplicacion`,
`IAplicacion`, `Datos`, `IDatos`…), lo que obligaba a tocar cuatro proyectos para agregar un
concepto. Ahora cada carpeta de `Bitakora/` es un concepto del negocio y se lleva **todo** lo suyo:
la entidad, su servicio, la interfaz de ese servicio y sus excepciones.

| Carpeta | Namespace | Qué tiene |
| --- | --- | --- |
| `Bitakora/Empleados` | `Bitakora.Empleados` | Empleado, TipoSalario, Tipo, EmpleadoServicio, ConsultarEmpleados, EmpleadoNoEncontrado |
| `Bitakora/Incapacidades` | `Bitakora.Incapacidades` | Incapacidad, SolicitudIncapacidad, TipoIncapacidad, DetalleIncapacidad, CreadorIncapacidad, IncapacidadServicio, CalcularFechas + sus excepciones |
| `Bitakora/Liquidacion` | `Bitakora.Liquidacion` | ReconocimientoEconomico, ResponsablePago, Entidad, DetalleReconocimientoEconomico, ResponsablePagoServicio, DiasSinResponsableDePago |
| `Bitakora/Salarios` | `Bitakora.Salarios` | Dinero, ValueObject, Moneda, SalarioMinimoServicio + sus excepciones |
| `Bitakora/Persistencia` | `Bitakora.Persistencia` | IncapacidadesContext (lo único que sabe de EF) |
| `Api/` | `Api.*` | Minimal API: `Empleados/`, `Incapacidades/`, `Liquidacion/`, más `Program`, `ServiciosDeBitakora` y las migraciones |
| `Test/` | `Test.*` | Espeja las carpetas del dominio |

La `Api` **no tiene controllers ni `Startup`**: son endpoints de Minimal API declarados en clases
estáticas con métodos de extensión (`MapearEmpleados`, `MapearIncapacidades`, `MapearLiquidacion`),
una por concepto del negocio, que `Program.cs` llama en tres líneas. Al agregar un endpoint va en el
archivo del concepto al que pertenece, no en uno nuevo por ruta.

**OpenAPI.** El documento sale de los endpoints reales (`/openapi/v1.json`, OpenAPI 3.1) y se emite
además en cada build a `Api/Api.json`, versionado. Scalar lo muestra en `/scalar/v1` sirviendo su
bundle desde la app, sin CDN.

Al agregar un endpoint hay que declararle sus errores con `.ProducesProblem(...)`, y las
descripciones van como `.WithSummary(...)` — **no** como comentarios XML `///`, que es la vía que
ofrece .NET 10 pero choca con la regla de no llevar comentarios. `.WithSummary` no es un comentario:
es un dato que la Api sirve a sus clientes.

Ojo con `Microsoft.OpenApi`: `Microsoft.AspNetCore.OpenApi` 10.0.10 arrastra la 2.0.0, que tiene una
vulnerabilidad alta (GHSA-v5pm-xwqc-g5wc, afecta `<= 2.7.4`). Está fijada en 2.7.5 como
`PackageReference` directo en `Api.csproj`; no la saques sin verificar con
`dotnet list package --vulnerable`.

Dos cosas del pipeline que no se tocan sin entender por qué están:
`UseDeveloperExceptionPage` va **antes** de `UseExceptionHandler` (al revés se come el mapeo de
excepciones), y `ConfigureHttpJsonOptions` reemplaza al `AddJsonOptions` de MVC — Minimal API no
lee los `JsonOptions` de MVC, así que ponerlo mal cambia el formato de todas las respuestas.

La carpeta del dinero se llama `Salarios` y no `Dinero` por una razón del compilador, no de diseño:
un namespace `Bitakora.Dinero` que contenga la clase `Dinero` hace que cualquier `Dinero` sin
calificar dentro de `Bitakora.*` se resuelva al **namespace** y no al tipo.

Las interfaces (`IEmpleadoServicio`, `ICreadorIncapacidad`, …) viven **al lado de su
implementación**, no en un proyecto aparte.

## Excepciones: la categoría la decide el dominio, el status code la Api

En la raíz de `Bitakora` hay dos bases abstractas que clasifican la falla **sin hablar de HTTP**:

- `SolicitudInvalida` — el que llamó mandó algo que no sirve y lo puede corregir.
- `NoEncontrado` — se pidió algo que no existe.

`Api/ExcepcionesDelDominioComoHttp` (un `IExceptionHandler`) las traduce a 400 y 404 con
`application/problem+json`. Cualquier otra excepción **no la maneja**: cae en el 500 genérico y no
filtra su mensaje, porque significa que se rompió un invariante nuestro, no que el cliente se
equivocó.

Al agregar una excepción, heredá de la base que corresponda y no toques la Api. Si es un invariante
interno —como `MonedaInvalida` o `IncapacidadSinReconocimientos`—, dejala heredando de `Exception`.

## Cuidado: la doc original describía otro proyecto

`docs/bitakora.md` es el documento fundacional, y buena parte es **aspiracional**. Antes de guiarte
por él, sabé que en el código **no existen**: `Recobro`, `Nomina`, `PeriodoIncapacidad`,
`NumeroIdentificacion`, `Nit`, `CodigoDiagnostico`, ni la máquina de estados
Registrada→Validada→RecobroGenerado. Tampoco hay repositorios (son `*Servicio`) ni SQL Server (es
SQLite).

Cuando la doc y el código discrepan, manda el código — `source-code-is-the-design`.

## Glosario del negocio

**PILA** Planilla Integrada de Liquidación de Aportes · **EPS** Entidad Promotora de Salud ·
**AFP** Administradora de Fondos de Pensiones · **ARL** Administradora de Riesgos Laborales ·
**Recobro** recuperar de la EPS lo pagado por incapacidades · **Devengado** lo que gana el empleado ·
**Deducciones** aportes y retenciones · **IBC** Ingreso Base de Cotización ·
**SMLMV** Salario Mínimo Legal Mensual Vigente.

Reglas de pago por enfermedad general: empresa días 1–2 al 100%, EPS días 3–90 al 66.66%,
EPS días 91–180 al 50%, Fondo de Pensiones días 181–540 al 50%. Los cuatro tramos están
implementados en `ResponsablePagoServicio`, y pasarse del último día cubierto de cualquier tipo
lanza `DiasSinResponsableDePago` en vez de liquidar de a pedazos.

## Ver también

`designing-with-objects` — el índice de las heurísticas
`h1-object-per-entity` — para los value objects que el dominio todavía no tiene
`source-code-is-the-design` — por qué manda el código cuando discrepa de la doc

## Texto completo

`docs/bitakora.md`
