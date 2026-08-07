# Incapacidades

Liquidación de incapacidades laborales según la normativa colombiana. Dada una solicitud —empleado,
tipo de incapacidad, fecha inicial y duración— reparte el pago entre la empresa, la EPS, la ARL y el
Fondo de Pensiones en períodos contiguos, y persiste la incapacidad con sus reconocimientos
económicos.

## Requisitos

- .NET 10 SDK

## Correr y verificar en local

### 1. Antes de arrancar: la base tiene que existir

La base es SQLite y vive en `Api/Incapacidades.db`. **No está versionada** —el `.gitignore` excluye
`*.db`— pero trae los empleados sembrados y **nada la recrea al arrancar**: sin ese archivo todos
los endpoints responden 500.

```bash
ls Api/Incapacidades.db || git show b765f90:Api/Incapacidades.db > Api/Incapacidades.db
```

Las migraciones están en `Api/Migrations/`; para recrearla desde cero, `dotnet ef database update`
desde `Api`.

### 2. Levantar

```bash
cd Api
dotnet run
```

Responde en `https://localhost:5001` y en `http://localhost:5000`. El pipeline tiene
`UseHttpsRedirection`, así que **el puerto 5000 redirige con 307 al 5001**: en el navegador es
transparente, pero con `curl` hay que usar `https` o agregar `-L`.

La primera vez conviene confiar el certificado de desarrollo, o el navegador va a advertir:

```bash
dotnet dev-certs https --trust
```

| Abrir | Para |
| --- | --- |
| https://localhost:5001/index.html | El front: cargar una incapacidad y ver su liquidación |
| https://localhost:5001/scalar/v1 | Explorar y **probar** los endpoints desde el navegador |
| https://localhost:5001/openapi/v1.json | El documento crudo |

### 3. Verificar de punta a punta

El camino feliz, que es lo que conviene mirar cuando se toca algo. El `-k` es porque el certificado
de desarrollo no está confiado; si corriste `dev-certs --trust`, sobra:

```bash
API=https://localhost:5001

curl -sk $API/Empleado                      # 200, Alan y Richard
curl -sk "$API/CalcularFechas/?anio=2020&mes=6&dia=3&cantidadDias=4"   # "2020-06-06T00:00:00"

SOLICITUD='{"idEmpleado":2,"tipoIncapacidad":1,"anio":2020,"mes":6,"dia":3,"cantidadDias":4,"observaciones":"x"}'
curl -sk -X POST $API/Incapacidad -H 'Content-Type: application/json' -d "$SOLICITUD"

curl -sk $API/IncapacidadConsulta/2         # la incapacidad con su totalAPagar
curl -sk $API/ReconocimientoEconomico/2     # los tramos: EMPRESA los días 1-2, EPS del 3 en adelante
```

Y que los errores sigan saliendo con su código y su mensaje:

```bash
post() { curl -sk -o /dev/null -w "%{http_code}\n" -X POST $API/Incapacidad \
           -H 'Content-Type: application/json' -d "$1"; }

post '{"idEmpleado":2,"tipoIncapacidad":1,"anio":2020,"mes":6,"dia":3,"cantidadDias":0,"observaciones":"x"}'
# 400 — "Una incapacidad dura al menos un día, y se pidieron 0."

post '{"idEmpleado":999,"tipoIncapacidad":1,"anio":2020,"mes":6,"dia":3,"cantidadDias":4,"observaciones":"x"}'
# 404 — "No existe un empleado con el id 999."

post '{"idEmpleado":2,"tipoIncapacidad":1,"anio":2020,"mes":6,"dia":3,"cantidadDias":600,"observaciones":"x"}'
# 400 — se pasa del día 540, no se liquida a medias
```

Desde `/scalar/v1` se puede hacer lo mismo sin salir del navegador, con el botón de probar de cada
endpoint.

## Tests

```bash
dotnet test
```

Son **117 tests** con xUnit y Shouldly, en tres niveles:

- **Dominio** — `Dinero`, `ResponsablePago`, `Incapacidad`, la validación de `SolicitudIncapacidad`.
- **Servicios** — contra una SQLite en memoria, no contra la base de disco.
- **Api** — los 5 endpoints por HTTP real con `WebApplicationFactory`, el mapeo excepción→HTTP, y el
  documento OpenAPI contra las rutas que la Api expone de verdad.

Los de Api son los que atrapan una ruta rota o un contrato cambiado: renombrar `/Empleado` hace caer
tres.

## Endpoints

| Método | Ruta | Qué hace |
| --- | --- | --- |
| `GET` | `/Empleado` | Lista los empleados |
| `POST` | `/Incapacidad` | Crea una incapacidad a partir de una `SolicitudIncapacidad` |
| `GET` | `/IncapacidadConsulta/{idEmpleado}` | Incapacidades de un empleado, con su total a pagar |
| `GET` | `/ReconocimientoEconomico/{idEmpleado}` | Reconocimientos económicos de un empleado |
| `GET` | `/CalcularFechas?anio=&mes=&dia=&cantidadDias=` | Fecha final de un período |
| `GET` | `/openapi/v1.json` | El documento OpenAPI 3.1 de la Api |
| `GET` | `/scalar/v1` | UI para explorar y probar la Api |

## OpenAPI

`Microsoft.AspNetCore.OpenApi` genera el documento a partir de los endpoints reales, así que el
contrato se deriva del código en vez de describirse en prosa. **Scalar** lo muestra en `/scalar/v1`
y sirve su propio bundle desde la app: no depende de ninguna CDN, igual que el front de `wwwroot`.

El documento además se emite **en cada compilación** a `Api/Api.json`, que está versionado. Un
cambio de contrato —una ruta, un campo, un status code— aparece como diff en la PR en vez de pasar
desapercibido. La copia versionada no lleva la sección `servers`, que solo existe en tiempo de
ejecución; eso la hace independiente del host.

Los códigos de error se declaran endpoint por endpoint, no con un transformer uniforme: **solo
`POST /Incapacidad` documenta un 404**, porque es el único que resuelve un empleado. Hay un test
que verifica que los otros cuatro *no* lo declaren.

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
