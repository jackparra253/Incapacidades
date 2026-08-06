# Incapacidades

Liquidación de incapacidades laborales según la normativa colombiana. Dada una solicitud —empleado,
tipo de incapacidad, fecha inicial y duración— reparte el pago entre la empresa, la EPS y la ARL en
períodos contiguos, y persiste la incapacidad con sus reconocimientos económicos.

## Requisitos

- .NET 10 SDK

## Ejecutar

```bash
cd Api
dotnet run
```

Luego abrir https://localhost:5001/index.html (también responde en http://localhost:5000).

La base es SQLite y viene versionada en el repo, en `Api/Incapacidades.db`. Las migraciones están en
`Api/Migrations/`, pero nada las aplica al arrancar: si necesitás recrear la base, corré
`dotnet ef database update` desde `Api`.

## Tests

```bash
dotnet test
```

Son 32 tests con xUnit y Shouldly. Cubren los dos creadores de incapacidad extremo a extremo contra
una SQLite en memoria, los cálculos de `ResponsablePago` y los servicios de datos.

## Endpoints

| Método | Ruta | Qué hace |
| --- | --- | --- |
| `GET` | `/Empleado` | Lista los empleados |
| `POST` | `/IncapacidadLey50` | Crea una incapacidad a partir de una `SolicitudIncapacidad` |
| `GET` | `/IncapacidadConsulta/{idEmpleado}` | Incapacidades de un empleado |
| `GET` | `/ReconocimientoEconomico/{idEmpleado}` | Reconocimientos económicos de un empleado |
| `GET` | `/CalcularFechas?anio=&mes=&dia=&cantidadDias=` | Fecha final de un período |

## Estructura

El repo está organizado por capa técnica, con las interfaces de cada capa en su propio proyecto
(`I*`). Las dependencias apuntan hacia adentro: el dominio no conoce EF Core ni ASP.NET.

| Proyecto | Contenido |
| --- | --- |
| `Modelos/Entidades`, `Modelos/ValueObjects` | **El dominio.** `Empleado`, `Incapacidad`, `ReconocimientoEconomico`, `ResponsablePago`, `TipoSalario`, `Dinero` |
| `Modelos/` (raíz) | DTOs del borde: `SolicitudIncapacidad`, `DetalleIncapacidad`, `DetalleReconocimientoEconomico` |
| `Aplicacion/` · `IAplicacion/` | Casos de uso: `CreadorIncapacidadLey50`, `CreadorIncapacidadSalarioIntegral`, `CalcularFechas`, `ConsultarEmpleados` |
| `Datos/` · `IDatos/` | EF Core sobre SQLite: los `*Servicio` y `IncapacidadesContext` |
| `Api/` | Controllers, `Startup`, migraciones y el front estático en `wwwroot/` |
| `Dominio/` · `IDominio/` | Vacíos (ver limitaciones) |
| `Test/` | Tests unitarios y de integración |

## Reglas del dominio

Enfermedad general — los períodos son **contiguos**, sin huecos ni traslapes:

| Días | Responsable | % del salario |
| --- | --- | --- |
| 1–2 | Empresa | 100% |
| 3–90 | EPS | 66.66% |
| 91–180 | EPS | 50% |
| 181+ | Fondo de Pensiones | *no implementado* |

Licencia de maternidad: 126 días a cargo de la EPS al 100%. Licencia de paternidad: 8 días, igual.
Enfermedad y accidente laboral: hasta 180 días a cargo de la ARL al 100%.

El tipo de salario **no** afecta las fechas, solo los montos: Ley 50 paga 100% sobre el salario,
mientras que salario integral lo separa en 70% salario y 30% compensación.

Fuente: [MinJusticia — Cómo y quién paga el salario durante una incapacidad laboral](https://www.minjusticia.gov.co/programas-co/LegalApp/Paginas/Como-y-quien-paga-el-salario-durante-una-incapacidad-laboral.aspx)

## Limitaciones conocidas

Cosas que hoy no funcionan como el dominio pediría. Ninguna hace fallar la app: todas producen datos
incompletos en silencio.

- **Solo Ley 50 está expuesta por la API.** `CreadorIncapacidadSalarioIntegral` está registrado en
  `Startup` y cubierto por tests, pero ningún controller lo llama.
- **No existe el Fondo de Pensiones.** Una enfermedad general de más de 180 días deja los días
  restantes sin responsable y sin reconocimiento.
- **Falta el piso de 1 SMLMV** por incapacidad que exige la normativa.
- **Un `tipoIncapacidad` inválido se persiste igual**, sin reconocimientos económicos y sin error.
- **Empleados y responsables de pago están hardcodeados** en `EmpleadoServicio` y
  `ResponsablePagoServicio`. Solo las incapacidades y sus reconocimientos van a la base.
- **El porcentaje de la EPS está como `0.6667`** y la normativa citada dice 66.66%. Falta decidir
  cuál es la cifra oficial.
- **`Api/Incapacidades.db` está versionada** pese a que el `.gitignore` excluye `*.db`; quedó
  trackeada desde antes de que existiera la regla.
- **`Dominio/` e `IDominio/` quedaron vacíos** tras borrar código muerto, pero siguen en el `.sln` y
  referenciados desde `Api`, `Aplicacion` y `Test`.
