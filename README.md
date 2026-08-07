# Incapacidades

Liquidación de incapacidades laborales según la normativa colombiana. Dada una solicitud —empleado,
tipo de incapacidad, fecha inicial y duración— reparte el pago entre la empresa, la EPS, la ARL y el
Fondo de Pensiones en períodos contiguos, y persiste la incapacidad con sus reconocimientos
económicos.

## Requisitos

- .NET 10 SDK

## Ejecutar

```bash
cd Api
dotnet run
```

Luego abrir https://localhost:5001/index.html (también responde en http://localhost:5000).

La base es SQLite y vive en `Api/Incapacidades.db`. **No está versionada** —el `.gitignore` excluye
`*.db`— pero trae los empleados sembrados y **nada la recrea al arrancar**: sin ese archivo todos los
endpoints responden 500. Las migraciones están en `Api/Migrations/`; para recrearla, `dotnet ef
database update` desde `Api`.

## Tests

```bash
dotnet test
```

Son 85 tests con xUnit y Shouldly. Cubren la creación de incapacidades extremo a extremo contra una
SQLite en memoria —para salario ordinario y para salario integral—, los cálculos de `ResponsablePago`
y `Dinero`, la validación de la solicitud en el borde y los servicios de datos.

## Endpoints

| Método | Ruta | Qué hace |
| --- | --- | --- |
| `GET` | `/Empleado` | Lista los empleados |
| `POST` | `/Incapacidad` | Crea una incapacidad a partir de una `SolicitudIncapacidad` |
| `GET` | `/IncapacidadConsulta/{idEmpleado}` | Incapacidades de un empleado, con su total a pagar |
| `GET` | `/ReconocimientoEconomico/{idEmpleado}` | Reconocimientos económicos de un empleado |
| `GET` | `/CalcularFechas?anio=&mes=&dia=&cantidadDias=` | Fecha final de un período |

## Estructura

El repo está organizado **por dominio**, no por capa técnica: cada carpeta de `Bitakora/` es un
concepto del negocio y se lleva todo lo suyo —la entidad, su servicio, la interfaz de ese servicio y
sus excepciones—, así que agregar un concepto se hace en un solo lugar. La única dependencia entre
proyectos es `Api → Bitakora`.

| Carpeta | Contenido |
| --- | --- |
| `Bitakora/Empleados` | `Empleado`, `TipoSalario`, `EmpleadoServicio`, `ConsultarEmpleados` |
| `Bitakora/Incapacidades` | `Incapacidad`, `SolicitudIncapacidad`, `CreadorIncapacidad`, `IncapacidadServicio`, `CalcularFechas` |
| `Bitakora/Liquidacion` | `ReconocimientoEconomico`, `ResponsablePago`, `ResponsablePagoServicio` |
| `Bitakora/Salarios` | `Dinero`, `ValueObject`, `SalarioMinimoServicio` |
| `Bitakora/Persistencia` | `IncapacidadesContext` — lo único que sabe de EF Core |
| `Api/` | Minimal API: los endpoints, agrupados igual que el dominio |
| `Test/` | Espeja las carpetas del dominio |

La `Api` usa **Minimal API**, con los endpoints repartidos en archivos por concepto del negocio en
vez de en controllers. `Program.cs` solo arma el pipeline y llama a los tres mapeos:

| Archivo | Endpoints |
| --- | --- |
| `Api/Empleados/EmpleadosEnHttp.cs` | `GET /Empleado` |
| `Api/Incapacidades/IncapacidadesEnHttp.cs` | `POST /Incapacidad`, `GET /IncapacidadConsulta/{id}`, `GET /CalcularFechas` |
| `Api/Liquidacion/LiquidacionEnHttp.cs` | `GET /ReconocimientoEconomico/{id}` |
| `Api/ServiciosDeBitakora.cs` | Las registraciones del contenedor |

Son clases estáticas con métodos de extensión: no son objetos del dominio, son cableado. A
propósito **no** hay un `IEndpointDefinition` con auto-registro por reflexión — con 5 endpoints ese
patrón resolvería un problema que acá no existe, y cambiaría tres llamadas legibles por magia.

La carpeta del dinero se llama `Salarios` por una restricción del compilador, no de diseño: un
namespace `Bitakora.Dinero` que contenga la clase `Dinero` haría que cualquier `Dinero` sin calificar
dentro de `Bitakora.*` se resolviera al namespace en vez de al tipo.

## Reglas del dominio

Enfermedad general — los períodos son **contiguos**, sin huecos ni traslapes:

| Días | Responsable | % del salario |
| --- | --- | --- |
| 1–2 | Empresa | 100% |
| 3–90 | EPS | 66.66% |
| 91–180 | EPS | 50% |
| 181–540 | Fondo de Pensiones | 50% |

Licencia de maternidad: 126 días a cargo de la EPS al 100%. Licencia de paternidad: 8 días, igual.
Enfermedad y accidente laboral: hasta 180 días a cargo de la ARL al 100%.

Ninguna incapacidad se liquida a medias: si la duración se pasa del último día cubierto por su tipo,
la creación falla con `DiasSinResponsableDePago` en vez de guardar los días que sí alcanzan.

Se liquida sobre el IBC, y ningún reconocimiento diario baja del salario mínimo del año en que
empezó la incapacidad. El tipo de salario **no** afecta las fechas, solo los montos: el ordinario
paga 100% sobre el salario, mientras que el integral lo separa en 70% salario y 30% compensación,
que la empresa paga aparte al 100%.

Fuente: [MinJusticia — Cómo y quién paga el salario durante una incapacidad laboral](https://www.minjusticia.gov.co/programas-co/LegalApp/Paginas/Como-y-quien-paga-el-salario-durante-una-incapacidad-laboral.aspx)

## Errores

El dominio distingue dos clases de falla que el que llama **puede** corregir, y la Api las traduce a
HTTP en `ExcepcionesDelDominioComoHttp`. La distinción vive en el dominio (`SolicitudInvalida`,
`NoEncontrado`); el código de estado es cosa de la Api.

| Excepción hereda de | HTTP | Ejemplos |
| --- | --- | --- |
| `SolicitudInvalida` | `400` | `CantidadDiasInvalida`, `FechaInvalida`, `TipoIncapacidadInvalido`, `DiasSinResponsableDePago` |
| `NoEncontrado` | `404` | `EmpleadoNoEncontrado` |
| cualquier otra | `500` | `SalarioMinimoDesconocido`, `MonedaInvalida`, `IncapacidadSinReconocimientos` |

Las dos primeras responden `application/problem+json` con el mensaje de la excepción en `detail`, y
el front lo muestra tal cual. Las demás son fallas internas: devuelven un 500 genérico y **no** filtran
el mensaje.

Una excepción nueva no obliga a tocar la Api: alcanza con heredar de la base que corresponda.

## Limitaciones conocidas

- **Empleados y responsables de pago están hardcodeados** en `EmpleadoServicio` y
  `ResponsablePagoServicio`. Solo las incapacidades y sus reconocimientos van a la base.
- **No existe el concepto de sector.** Los días 1–2 se pagan al 100%, que es la regla del sector
  público; en el privado la fuente sostiene 66.66%. No se arregla cambiando un número: falta el ente
  que represente la distinción.
- **`SalarioMinimoServicio` solo conoce 2026 y 2020.** Cualquier otro año lanza
  `SalarioMinimoDesconocido`, a propósito, para no liquidar contra un mínimo equivocado. Hoy sale como
  500 porque es un hueco del sistema, no un error del que llama; si algún día se decide que el cliente
  debe verlo, basta con reparentarla a `SolicitudInvalida`.
- **Qué pasa después del día 540** no está definido por ninguna de las fuentes consultadas.
