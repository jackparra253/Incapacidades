> **Nota (2026-08-05).** Este es el `CLAUDE-Bitakora.md` original, preservado íntegro al
> convertirlo en el skill `bitakora`. Se conserva como texto de referencia, pero **describe
> en parte un proyecto aspiracional**: la estructura de carpetas, varias entidades (Recobro,
> Nomina, PeriodoIncapacidad) y la máquina de estados no existen en el código. Para el estado
> real verificado, ver el `SKILL.md` que acompaña a este archivo.

---

# Heurísticas de Diseño de Software con Objetos - Bitákora

> **Basado en el curso de Hernán Wilkinson (10Pines)**  
> Adaptado para desarrollo en C# con Arquitectura Onion  
> Sistema de Gestión de Nómina - Bitákora (https://bitakora.co/)

---

## Tabla de Contenidos

1. [Filosofía Fundamental](#filosofía-fundamental)
2. [Arquitectura Onion](#arquitectura-onion)
3. [El Código Fuente ES el Diseño](#el-código-fuente-es-el-diseño)
4. [Los Tres Ejes de un Buen Modelo](#los-tres-ejes-de-un-buen-modelo)
5. [¿Qué es un Objeto?](#qué-es-un-objeto)
6. [Heurísticas de Diseño](#heurísticas-de-diseño)
   - [H1: Un Objeto por Ente](#h1-un-objeto-por-ente)
   - [H2: Objetos Completos](#h2-objetos-completos)
   - [H4: No Usar Null](#h4-no-usar-null)
   - [H5: Favorecer Objetos Inmutables](#h5-favorecer-objetos-inmutables)
   - [H6: No Romper el Encapsulamiento](#h6-no-romper-el-encapsulamiento)
7. [Dominio Bitákora](#dominio-bitákora)
8. [Checklist para Code Reviews](#checklist-para-code-reviews)
9. [Instrucciones para Claude](#instrucciones-para-claude)

---

# Filosofía Fundamental

## Definición de Software

El software es:

```
(Conocimiento expresado en un)
Modelo Computable
de un Dominio de Problema
de la Realidad
```

**Desglose de la definición:**

| Componente | Significado | Ejemplo Bitákora |
|------------|-------------|------------------|
| **Conocimiento** | El software captura y representa lo que sabemos sobre un problema | Reglas de liquidación de nómina colombiana |
| **Modelo** | Es una representación simplificada, no la realidad misma | `Incapacidad` representa una licencia médica real |
| **Computable** | Puede ejecutarse, implica paso del tiempo | Cálculo de días de incapacidad, recobro a EPS |
| **Dominio de Problema** | El área específica del negocio/realidad que abordamos | Gestión de nómina, incapacidades, seguridad social |
| **Realidad** | Aquello que existe independientemente de nuestra percepción | Normativa laboral colombiana, UGPP, DIAN |

### Implicación Clave

Si el software es un modelo, entonces:
- **"Buen Software" == "Buen Modelo"**
- La calidad del software se mide por la calidad del modelo que representa
- Debemos organizar según el **Dominio de Problema** (Nómina, Incapacidades, Empleados), no según capas técnicas

---

# Arquitectura Onion

## POR QUÉ usar Arquitectura Onion

La Arquitectura Onion (también llamada "Arquitectura Centrica" o "Puertos y Adaptadores") invierte la dependencia tradicional: **el dominio está en el centro y no depende de nada externo**.

### El Problema con la Arquitectura Tradicional

```
┌─────────────────────────────────────┐
│           Presentación              │ → Depende de...
├─────────────────────────────────────┤
│              Negocio                │ → Depende de...
├─────────────────────────────────────┤
│          Acceso a Datos             │ → Depende de...
├─────────────────────────────────────┤
│           Base de Datos             │
└─────────────────────────────────────┘
```

**Problemas:**
- El dominio depende de la infraestructura
- Cambiar la base de datos afecta todo
- Difícil de testear sin base de datos real
- Acoplamiento fuerte a frameworks

### La Solución: Arquitectura Onion

```
┌─────────────────────────────────────────────────────────────────┐
│                     Infraestructura                             │
│  ┌───────────────────────────────────────────────────────────┐  │
│  │                     Aplicación                            │  │
│  │  ┌─────────────────────────────────────────────────────┐  │  │
│  │  │                    Dominio                          │  │  │
│  │  │  ┌───────────────────────────────────────────────┐  │  │  │
│  │  │  │              Entidades                        │  │  │  │
│  │  │  │         (Incapacidad, Empleado,               │  │  │  │
│  │  │  │          Nomina, Recobro)                     │  │  │  │
│  │  │  └───────────────────────────────────────────────┘  │  │  │
│  │  └─────────────────────────────────────────────────────┘  │  │
│  └───────────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────────┘
```

## PARA QUÉ sirve esta arquitectura

1. **Independencia del Dominio**: Las reglas de negocio de Bitákora no dependen de SQL Server, Entity Framework, ni ASP.NET
2. **Testeabilidad**: Puedes testear la lógica de liquidación de incapacidades sin base de datos
3. **Flexibilidad**: Cambiar de SQL Server a PostgreSQL no afecta el dominio
4. **Claridad**: El código del dominio solo habla de nómina, no de tecnología

## CÓMO se estructura en el Proyecto Bitákora

### Estructura de Carpetas (Proyecto Incapacidades)

```
/Incapacidades
├── /Dominio                    # Capa más interna - Entidades y lógica de negocio
│   ├── Incapacidad.cs
│   ├── Empleado.cs
│   ├── PeriodoIncapacidad.cs
│   └── TipoIncapacidad.cs
│
├── /IDominio                   # Interfaces del dominio (abstracciones)
│   ├── IIncapacidadRepository.cs
│   └── IEmpleadoRepository.cs
│
├── /Aplicacion                 # Casos de uso / Servicios de aplicación
│   ├── RegistrarIncapacidadUseCase.cs
│   ├── CalcularRecobroUseCase.cs
│   └── LiquidarNominaUseCase.cs
│
├── /IAplicacion                # Interfaces de aplicación
│   ├── IRegistrarIncapacidad.cs
│   └── ICalcularRecobro.cs
│
├── /Datos                      # Implementaciones de infraestructura
│   ├── IncapacidadRepository.cs
│   ├── EmpleadoRepository.cs
│   └── BitakoraDbContext.cs
│
├── /IDatos                     # Interfaces de datos
│   └── IBitakoraDbContext.cs
│
├── /Modelos                    # DTOs para transferencia
│   ├── IncapacidadDto.cs
│   └── EmpleadoDto.cs
│
├── /Api                        # Capa externa - Controladores
│   └── Controllers/
│       ├── IncapacidadesController.cs
│       └── EmpleadosController.cs
│
└── /Test                       # Tests unitarios
    ├── Dominio/
    └── Aplicacion/
```

### Regla de Dependencias

```
Api → Aplicacion → Dominio ← IDominio
 ↓         ↓                    ↑
Datos → IDatos                  │
   └────────────────────────────┘
```

**La Regla de Oro:**

> **El Dominio NO conoce la existencia de la Api ni de los Datos.**
> **La Api NO sabe de reglas de negocio de Bitákora.**

---

# El Código Fuente ES el Diseño

## La Tesis Central (Jack W. Reeves, 1992)

El código fuente no es solo implementación, **ES el diseño del software**. La compilación es solo el proceso de "manufactura" automatizado.

### Contraste con la Visión Tradicional

| Visión Tradicional | Visión del Curso |
|-------------------|------------------|
| Diseño = Diagramas UML, documentos | Diseño = Código fuente |
| Código = Construcción/Implementación | Código = Expresión del diseño |
| Diseño se hace "antes" de codificar | Diseño emerge mientras se codifica |

### Consecuencias Prácticas para Bitákora

1. **No es fácil estimar el "diseño"**: Estimar cuánto toma diseñar el modelo de Incapacidades es difícil
2. **La "construcción" no cuesta nada**: Compilar el proyecto es instantáneo
3. **Lo costoso es el diseño**: El tiempo de desarrollo ES tiempo de diseño
4. **Los diagramas visualizan, no son**: Un diagrama de clases de `Incapacidad` visualiza el diseño, pero el diseño real está en `Incapacidad.cs`

---

# Los Tres Ejes de un Buen Modelo

Un buen modelo se evalúa en tres dimensiones ortogonales:

## Eje 1: Implementativo (NFRs)

**Pregunta**: ¿Ejecuta correctamente con los recursos disponibles?

| Criterio | Pregunta para Bitákora |
|----------|------------------------|
| Performance | ¿El cálculo de nómina se ejecuta en tiempo razonable para 1000 empleados? |
| Escalabilidad | ¿Puede manejar empresas con 3000+ empleados? |
| Disponibilidad | ¿El sistema está disponible durante el cierre de nómina? |

## Eje 2: Descriptivo (Legibilidad)

**Pregunta**: ¿Se puede entender y modificar fácilmente?

| Criterio | Ejemplo Bitákora |
|----------|------------------|
| Buenos nombres | `RegistrarIncapacidad()` en lugar de `Process()` |
| Lenguaje del dominio | `Empleado`, `Incapacidad`, `Recobro` — no `Entity1`, `Data` |
| Código "habitable" | Un desarrollador nuevo puede entender el flujo de liquidación |

## Eje 3: Funcional (Isomorfismo)

**Pregunta**: ¿Representa fielmente el dominio de Bitákora?

| Criterio | Aplicación |
|----------|------------|
| Algo nuevo en el dominio → algo nuevo en el modelo | Nueva regulación de incapacidades → nuevo objeto/método |
| Cambio en el dominio → cambio solo en su representación | Cambio en % de recobro → cambio solo en `Recobro` |
| 1:1 entre entes del dominio y objetos | Un `Empleado` real = un objeto `Empleado` |

### Ejemplo de Evaluación

```csharp
// ❌ Viola Eje Funcional: Un empleado tiene muchos strings sueltos
public class Empleado
{
    public string Cedula { get; set; }
    public string NombreEps { get; set; }
    public string CodigoEps { get; set; }
    public decimal SalarioBase { get; set; }
}

// ✅ Cumple los 3 ejes
public class Empleado
{
    private readonly NumeroIdentificacion _identificacion;
    private readonly EPS _eps;
    private readonly Salario _salarioBase;
    
    public Empleado(
        NumeroIdentificacion identificacion,
        EPS eps,
        Salario salarioBase)
    {
        _identificacion = identificacion;
        _eps = eps;
        _salarioBase = salarioBase;
    }
    
    public bool EstaAfiliadoA(EPS eps) => _eps.Equals(eps);
    public Dinero SalarioDiario() => _salarioBase.CalcularDiario();
}
```

---

# ¿Qué es un Objeto?

## Definición Correcta

> **Un objeto es la representación (en el modelo) de un ente (del dominio de problema).**

No es "datos + comportamiento". Es la representación de algo que existe en el dominio de Bitákora.

## La Esencia (Inspirado en Saint-Exupéry)

> "La perfección se alcanza, no cuando no hay nada más que añadir, sino cuando no hay nada más que quitar."

### Preguntas Guía para Bitákora

Para cada objeto, preguntarse:

1. **¿Qué ente del dominio representa?**
   - `Incapacidad` → Una licencia médica real de un empleado
   - `Recobro` → El proceso de recuperar dinero de la EPS

2. **¿Cuál es su esencia?**
   - `Incapacidad`: Período de tiempo en que un empleado no puede trabajar por razones médicas
   - `Empleado`: Persona vinculada laboralmente a una empresa

3. **¿Qué lo hace ser lo que es y no otra cosa?**
   - Una `Incapacidad` tiene: empleado, diagnóstico, período, tipo (común, laboral, maternidad)
   - Un `Empleado` tiene: identificación, contrato, salario, EPS

---

# Heurísticas de Diseño

## ¿Por qué Heurísticas y no Principios?

> **Principio**: Afirmación siempre verdadera, sin excepciones  
> **Heurística**: Guía útil en la mayoría de los casos, pero con excepciones

Las "heurísticas" requieren **pensamiento crítico**: no se aplican ciegamente, se evalúan en contexto.

---

# H1: Un Objeto por Ente

## Enunciado

> **Cada ente del dominio debe estar representado por exactamente un objeto.**

## POR QUÉ existe esta heurística

### Problema 1: Primitive Obsession

```csharp
// ❌ MAL: "Obsesión por primitivos"
public class Incapacidad
{
    public int DiasIncapacidad { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public string TipoIncapacidad { get; set; }  // "COMUN", "LABORAL", "MATERNIDAD"
    public string CedulaEmpleado { get; set; }
    public string NombreEps { get; set; }
    public string CodigoEps { get; set; }
}
```

**Problemas:**
- La lógica de validar tipo de incapacidad está dispersa
- El cálculo de días se repite en múltiples lugares
- No hay garantía de que `TipoIncapacidad` tenga un valor válido

### Problema 2: Entidad Fragmentada

```csharp
// ❌ MAL: EPS fragmentada en múltiples strings
public class Empleado
{
    public string NombreEps { get; set; }
    public string CodigoEps { get; set; }
    public string NitEps { get; set; }
}
```

## PARA QUÉ sirve esta heurística

1. **Centraliza la lógica**: Todo lo relacionado con EPS está en la clase `EPS`
2. **Evita duplicación**: El cálculo de días de incapacidad existe en un solo lugar
3. **Facilita el cambio**: Si cambia la regulación de tipos de incapacidad, solo se modifica `TipoIncapacidad`

## CÓMO aplicarla en Bitákora

### Value Objects del Dominio

```csharp
// ✅ BIEN: Cada ente tiene su objeto

// Tipo de Incapacidad como objeto con comportamiento
public abstract class TipoIncapacidad
{
    public abstract string Codigo { get; }
    public abstract string Nombre { get; }
    public abstract int DiasACargoEmpleador { get; }
    public abstract decimal PorcentajeRecobro { get; }
    
    public static TipoIncapacidad Comun => new IncapacidadComun();
    public static TipoIncapacidad Laboral => new IncapacidadLaboral();
    public static TipoIncapacidad Maternidad => new IncapacidadMaternidad();
}

public class IncapacidadComun : TipoIncapacidad
{
    public override string Codigo => "COMUN";
    public override string Nombre => "Enfermedad General";
    public override int DiasACargoEmpleador => 2;  // Ley colombiana
    public override decimal PorcentajeRecobro => 0.6667m;  // 66.67%
}

// Período de Incapacidad como Value Object
public sealed record PeriodoIncapacidad
{
    public DateOnly FechaInicio { get; }
    public DateOnly FechaFin { get; }
    
    public PeriodoIncapacidad(DateOnly fechaInicio, DateOnly fechaFin)
    {
        if (fechaFin < fechaInicio)
            throw new PeriodoInvalidoException(fechaInicio, fechaFin);
        
        FechaInicio = fechaInicio;
        FechaFin = fechaFin;
    }
    
    public int TotalDias => FechaFin.DayNumber - FechaInicio.DayNumber + 1;
    
    public bool Contiene(DateOnly fecha) => 
        fecha >= FechaInicio && fecha <= FechaFin;
    
    public bool SeSolapaCon(PeriodoIncapacidad otro) =>
        Contiene(otro.FechaInicio) || Contiene(otro.FechaFin) ||
        otro.Contiene(FechaInicio) || otro.Contiene(FechaFin);
}

// EPS como entidad con identidad
public sealed class EPS
{
    public Nit Nit { get; }
    public string RazonSocial { get; }
    public string CodigoMinisterio { get; }
    
    public EPS(Nit nit, string razonSocial, string codigoMinisterio)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(razonSocial);
        ArgumentException.ThrowIfNullOrWhiteSpace(codigoMinisterio);
        
        Nit = nit;
        RazonSocial = razonSocial;
        CodigoMinisterio = codigoMinisterio;
    }
}

// Número de Identificación (Cédula, CE, Pasaporte)
public abstract class NumeroIdentificacion
{
    public abstract string TipoDocumento { get; }
    public abstract string Numero { get; }
    
    public static NumeroIdentificacion Cedula(string numero) => 
        new CedulaCiudadania(numero);
    
    public static NumeroIdentificacion CedulaExtranjeria(string numero) => 
        new CedulaExtranjeria(numero);
}

public sealed class CedulaCiudadania : NumeroIdentificacion
{
    public override string TipoDocumento => "CC";
    public override string Numero { get; }
    
    public CedulaCiudadania(string numero)
    {
        if (string.IsNullOrWhiteSpace(numero) || !numero.All(char.IsDigit))
            throw new CedulaInvalidaException(numero);
        
        Numero = numero;
    }
}

// Dinero como Value Object
public sealed record Dinero
{
    public decimal Monto { get; }
    public Moneda Moneda { get; }
    
    private Dinero(decimal monto, Moneda moneda)
    {
        if (monto < 0)
            throw new MontoNegativoException(monto);
        
        Monto = monto;
        Moneda = moneda;
    }
    
    public static Dinero Pesos(decimal monto) => new(monto, Moneda.COP);
    public static Dinero Cero => new(0, Moneda.COP);
    
    public Dinero Sumar(Dinero otro)
    {
        if (Moneda != otro.Moneda)
            throw new MonedasDiferentesException(Moneda, otro.Moneda);
        
        return new Dinero(Monto + otro.Monto, Moneda);
    }
    
    public Dinero Multiplicar(decimal factor) => 
        new(Monto * factor, Moneda);
}
```

### La Incapacidad como Entidad Completa

```csharp
// ✅ BIEN: Incapacidad como entidad rica del dominio
public sealed class Incapacidad
{
    private readonly Empleado _empleado;
    private readonly TipoIncapacidad _tipo;
    private readonly PeriodoIncapacidad _periodo;
    private readonly Diagnostico _diagnostico;
    private readonly EPS _epsQueExpide;
    private EstadoIncapacidad _estado;
    
    public Incapacidad(
        Empleado empleado,
        TipoIncapacidad tipo,
        PeriodoIncapacidad periodo,
        Diagnostico diagnostico,
        EPS epsQueExpide)
    {
        _empleado = empleado ?? throw new ArgumentNullException(nameof(empleado));
        _tipo = tipo ?? throw new ArgumentNullException(nameof(tipo));
        _periodo = periodo ?? throw new ArgumentNullException(nameof(periodo));
        _diagnostico = diagnostico ?? throw new ArgumentNullException(nameof(diagnostico));
        _epsQueExpide = epsQueExpide ?? throw new ArgumentNullException(nameof(epsQueExpide));
        _estado = EstadoIncapacidad.Registrada;
    }
    
    public int DiasACargoEmpleador => 
        Math.Min(_tipo.DiasACargoEmpleador, _periodo.TotalDias);
    
    public int DiasACargoEPS => 
        Math.Max(0, _periodo.TotalDias - _tipo.DiasACargoEmpleador);
    
    public Dinero MontoARecobar()
    {
        var salarioDiario = _empleado.SalarioDiario();
        return salarioDiario
            .Multiplicar(DiasACargoEPS)
            .Multiplicar(_tipo.PorcentajeRecobro);
    }
    
    public bool PuedeGenerarRecobro() => 
        DiasACargoEPS > 0 && _estado == EstadoIncapacidad.Validada;
    
    public Recobro GenerarRecobro()
    {
        if (!PuedeGenerarRecobro())
            throw new NoSePuedeGenerarRecobroException(this);
        
        return new Recobro(this, MontoARecobar(), _epsQueExpide);
    }
    
    public void Validar()
    {
        if (_estado != EstadoIncapacidad.Registrada)
            throw new EstadoIncapacidadInvalidoException(_estado);
        
        _estado = EstadoIncapacidad.Validada;
    }
}
```

---

# H2: Objetos Completos

## Enunciado

> **Un objeto debe nacer completo y válido, representando fielmente su ente desde el momento de su creación.**

## POR QUÉ existe esta heurística

### El Problema

```csharp
// ❌ MAL: Incapacidad que nace incompleta
var incapacidad = new Incapacidad();
incapacidad.EmpleadoId = 123;
// ... ¿y si olvido setear el tipo?
// ... ¿y si olvido setear el período?
servicioRecobro.Calcular(incapacidad);  // 💥 NullReferenceException
```

**Problemas:**
- El objeto existe en estado inválido
- Otros objetos deben validar si está completo
- `NullReferenceException` en tiempo de ejecución

## PARA QUÉ sirve esta heurística

1. **Elimina estados inválidos**: No puede existir una `Incapacidad` sin `Empleado`
2. **Simplifica el código cliente**: No hay que verificar si el objeto está completo
3. **Centraliza la validación**: La validación está en el constructor, no dispersa

## CÓMO aplicarla en Bitákora

```csharp
// ✅ BIEN: Empleado nace completo
public sealed class Empleado
{
    private readonly NumeroIdentificacion _identificacion;
    private readonly NombreCompleto _nombre;
    private readonly FechaContratacion _fechaContratacion;
    private readonly Salario _salario;
    private readonly EPS _eps;
    private readonly AFP _afp;
    
    public Empleado(
        NumeroIdentificacion identificacion,
        NombreCompleto nombre,
        FechaContratacion fechaContratacion,
        Salario salario,
        EPS eps,
        AFP afp)
    {
        _identificacion = identificacion ?? throw new ArgumentNullException(nameof(identificacion));
        _nombre = nombre ?? throw new ArgumentNullException(nameof(nombre));
        _fechaContratacion = fechaContratacion ?? throw new ArgumentNullException(nameof(fechaContratacion));
        _salario = salario ?? throw new ArgumentNullException(nameof(salario));
        _eps = eps ?? throw new ArgumentNullException(nameof(eps));
        _afp = afp ?? throw new ArgumentNullException(nameof(afp));
    }
    
    public Dinero SalarioDiario() => _salario.CalcularDiario();
    
    public bool TieneDerechoAIncapacidad() => 
        _fechaContratacion.TieneMasDe(dias: 30);
    
    public bool EstaAfiliadoA(EPS eps) => _eps.Equals(eps);
}

// ✅ BIEN: Salario como Value Object completo
public sealed record Salario
{
    public Dinero Base { get; }
    public TipoSalario Tipo { get; }
    
    public Salario(Dinero baseValue, TipoSalario tipo)
    {
        if (baseValue.Monto < SalarioMinimo.Vigente.Monto)
            throw new SalarioPorDebajoDelMinimoException(baseValue);
        
        Base = baseValue;
        Tipo = tipo;
    }
    
    public Dinero CalcularDiario() => Tipo switch
    {
        TipoSalario.Ordinario => Base.Multiplicar(1m / 30m),
        TipoSalario.Integral => Base.Multiplicar(0.7m / 30m),  // 70% del integral
        _ => throw new TipoSalarioNoSoportadoException(Tipo)
    };
}

// ✅ BIEN: Recobro nace completo
public sealed class Recobro
{
    private readonly Incapacidad _incapacidad;
    private readonly Dinero _monto;
    private readonly EPS _epsARecobrar;
    private readonly FechaRecobro _fechaGeneracion;
    private EstadoRecobro _estado;
    
    public Recobro(Incapacidad incapacidad, Dinero monto, EPS epsARecobrar)
    {
        _incapacidad = incapacidad ?? throw new ArgumentNullException(nameof(incapacidad));
        _monto = monto ?? throw new ArgumentNullException(nameof(monto));
        _epsARecobrar = epsARecobrar ?? throw new ArgumentNullException(nameof(epsARecobrar));
        _fechaGeneracion = FechaRecobro.Hoy();
        _estado = EstadoRecobro.Pendiente;
    }
    
    public void MarcarComoRadicado(NumeroRadicado numeroRadicado)
    {
        if (_estado != EstadoRecobro.Pendiente)
            throw new EstadoRecobroInvalidoException(_estado);
        
        _estado = EstadoRecobro.Radicado;
        // Guardar número de radicado...
    }
    
    public void RegistrarPago(Dinero montoPagado, FechaPago fechaPago)
    {
        if (_estado != EstadoRecobro.Radicado)
            throw new EstadoRecobroInvalidoException(_estado);
        
        _estado = EstadoRecobro.Pagado;
        // Registrar el pago...
    }
}
```

---

# H4: No Usar Null

## Enunciado

> **Evitar el uso de `null` como valor válido. Usar alternativas que hagan explícita la ausencia.**

## POR QUÉ existe esta heurística

Tony Hoare (quien introdujo `null` en ALGOL W en 1965) lo llamó su **"billion dollar mistake"**.

### El Problema de null

`null` es **polisémico**: representa múltiples cosas:
- Variable no inicializada
- Ausencia de valor en el dominio (empleado sin cónyuge)
- Error no manejado
- "Todavía no lo sé"

```csharp
// ❌ MAL: ¿Qué significa null aquí?
public class Incapacidad
{
    public Recobro? Recobro { get; set; }  // ¿No tiene? ¿No se ha calculado? ¿Error?
}
```

## CÓMO aplicarla en Bitákora

### Solución 1: Null Object Pattern

```csharp
// ✅ BIEN: DireccionResidencia con Null Object
public abstract class DireccionResidencia
{
    public abstract string DireccionCompleta { get; }
    public abstract bool EsProporcionada { get; }
    public abstract Ciudad Ciudad { get; }
    
    public static DireccionResidencia NoProporcionada => new DireccionNoProporcionada();
}

public sealed class DireccionProporcionada : DireccionResidencia
{
    private readonly string _direccion;
    private readonly Ciudad _ciudad;
    
    public DireccionProporcionada(string direccion, Ciudad ciudad)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(direccion);
        _direccion = direccion;
        _ciudad = ciudad ?? throw new ArgumentNullException(nameof(ciudad));
    }
    
    public override string DireccionCompleta => $"{_direccion}, {_ciudad.Nombre}";
    public override bool EsProporcionada => true;
    public override Ciudad Ciudad => _ciudad;
}

public sealed class DireccionNoProporcionada : DireccionResidencia
{
    public override string DireccionCompleta => "Sin dirección registrada";
    public override bool EsProporcionada => false;
    public override Ciudad Ciudad => Ciudad.Desconocida;
}

// Uso
public class Empleado
{
    private readonly DireccionResidencia _direccion;
    
    public Empleado(..., DireccionResidencia direccion)
    {
        _direccion = direccion;  // Nunca null, puede ser NoProporcionada
    }
    
    // El código cliente no pregunta por null
    public string ObtenerDireccionParaEnvio() => _direccion.DireccionCompleta;
}
```

### Solución 2: Excepciones

```csharp
// ✅ BIEN: Repositorio que lanza excepción si no encuentra
public interface IEmpleadoRepository
{
    Empleado ObtenerPorIdentificacion(NumeroIdentificacion identificacion);
    bool Existe(NumeroIdentificacion identificacion);
}

public class EmpleadoRepository : IEmpleadoRepository
{
    public Empleado ObtenerPorIdentificacion(NumeroIdentificacion identificacion)
    {
        var empleado = _context.Empleados
            .FirstOrDefault(e => e.Identificacion == identificacion.Numero);
        
        return empleado ?? throw new EmpleadoNoEncontradoException(identificacion);
    }
    
    public bool Existe(NumeroIdentificacion identificacion)
    {
        return _context.Empleados.Any(e => e.Identificacion == identificacion.Numero);
    }
}
```

### Solución 3: Nullable Reference Types (C# 8+)

```csharp
// ✅ Hacer explícito lo que puede ser null con NRT
#nullable enable

public class RegistrarIncapacidadUseCase
{
    public Incapacidad? BuscarIncapacidadActiva(NumeroIdentificacion empleadoId)
    {
        // El '?' indica explícitamente que puede no existir
        return _repository.BuscarActiva(empleadoId);
    }
    
    public void Registrar(IncapacidadDto dto)
    {
        var existente = BuscarIncapacidadActiva(dto.EmpleadoId);
        
        // El compilador fuerza a verificar null
        if (existente is not null && existente.Periodo.SeSolapaCon(dto.Periodo))
        {
            throw new IncapacidadSolapadaException(existente);
        }
        
        // ...
    }
}
```

---

# H5: Favorecer Objetos Inmutables

## Enunciado

> **Favorecer la creación de objetos inmutables sobre objetos mutables.**

## POR QUÉ existe esta heurística

### El Debate Filosófico

- **Heráclito**: "Todo fluye, nada permanece"
- **Parménides**: "El ser es, el no-ser no es"

En software: los **Value Objects** representan valores que no cambian (como un monto de dinero, una fecha), mientras que las **Entidades** pueden cambiar de estado.

### Beneficios de la Inmutabilidad

1. **Thread-safety**: Objetos inmutables son inherentemente seguros en entornos concurrentes
2. **Facilidad de razonamiento**: Si no cambia, es más fácil entender qué hace
3. **Evita efectos secundarios**: `dinero.Sumar(otro)` devuelve un nuevo `Dinero`, no modifica el original

## CÓMO aplicarla en Bitákora

### Value Objects Inmutables con Records

```csharp
// ✅ BIEN: Value Objects inmutables usando records
public sealed record Dinero(decimal Monto, Moneda Moneda)
{
    public Dinero Sumar(Dinero otro)
    {
        if (Moneda != otro.Moneda)
            throw new MonedasDiferentesException(Moneda, otro.Moneda);
        
        return this with { Monto = Monto + otro.Monto };  // Devuelve nuevo objeto
    }
    
    public Dinero Multiplicar(decimal factor) =>
        this with { Monto = Monto * factor };
    
    public static Dinero Pesos(decimal monto) => new(monto, Moneda.COP);
}

public sealed record PeriodoIncapacidad(DateOnly FechaInicio, DateOnly FechaFin)
{
    public int TotalDias => FechaFin.DayNumber - FechaInicio.DayNumber + 1;
    
    public PeriodoIncapacidad Extender(int dias) =>
        this with { FechaFin = FechaFin.AddDays(dias) };
}

public sealed record NumeroRadicado(string Valor)
{
    public NumeroRadicado(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new NumeroRadicadoInvalidoException(valor);
        
        Valor = valor;
    }
}

public sealed record CodigoDiagnostico(string Codigo, string Descripcion)
{
    public CodigoDiagnostico(string codigo, string descripcion)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(codigo);
        ArgumentException.ThrowIfNullOrWhiteSpace(descripcion);
        
        Codigo = codigo;
        Descripcion = descripcion;
    }
}
```

### Entidades con Estado Controlado

```csharp
// ✅ BIEN: Entidad con campos inmutables y estado controlado
public sealed class Incapacidad
{
    // Campos inmutables (nunca cambian después de crear)
    private readonly Empleado _empleado;
    private readonly TipoIncapacidad _tipo;
    private readonly PeriodoIncapacidad _periodo;
    private readonly CodigoDiagnostico _diagnostico;
    
    // Estado mutable controlado
    private EstadoIncapacidad _estado;
    
    public Incapacidad(
        Empleado empleado,
        TipoIncapacidad tipo,
        PeriodoIncapacidad periodo,
        CodigoDiagnostico diagnostico)
    {
        _empleado = empleado;
        _tipo = tipo;
        _periodo = periodo;
        _diagnostico = diagnostico;
        _estado = EstadoIncapacidad.Registrada;
    }
    
    // Transiciones de estado controladas
    public void Validar()
    {
        if (_estado != EstadoIncapacidad.Registrada)
            throw new TransicionEstadoInvalidaException(_estado, EstadoIncapacidad.Validada);
        
        _estado = EstadoIncapacidad.Validada;
    }
    
    public void Rechazar(MotivoRechazo motivo)
    {
        if (_estado != EstadoIncapacidad.Registrada)
            throw new TransicionEstadoInvalidaException(_estado, EstadoIncapacidad.Rechazada);
        
        _estado = EstadoIncapacidad.Rechazada;
    }
}
```

---

# H6: No Romper el Encapsulamiento

## Enunciado

> **No acceder ni depender de la estructura interna de un objeto. Interactuar solo a través de sus mensajes públicos.**

## POR QUÉ existe esta heurística

### La Idea Central

> **Encapsulamiento = asignar responsabilidades a objetos correctamente**

No es solo "esconder datos". Es garantizar que cada objeto es responsable de su propio comportamiento.

### Qué se Rompe cuando Falla el Encapsulamiento

1. **Acoplamiento**: Otros objetos dependen de la estructura interna
2. **Duplicación**: La misma lógica aparece en múltiples lugares
3. **Fragilidad**: Cambiar la estructura interna rompe código externo

## CÓMO aplicarla en Bitákora

### ❌ MAL: Romper Encapsulamiento

```csharp
// ❌ MAL: Propiedades públicas con setters
public class Incapacidad
{
    public int DiasIncapacidad { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public string Estado { get; set; }
}

// Código cliente que "mete la mano" en la estructura interna
public class RecobroService
{
    public Dinero CalcularRecobro(Incapacidad inc)
    {
        // 🔴 Rompe encapsulamiento: pregunta y luego actúa
        if (inc.DiasIncapacidad > 2)
        {
            var diasARecobrar = inc.DiasIncapacidad - 2;
            // ... cálculo duplicado en otro lugar del sistema
        }
    }
}
```

### ❌ MAL: Private Setters (sigue siendo problemático)

```csharp
// ❌ TAMBIÉN MAL: Private setters permiten mutación incontrolada
public class Incapacidad
{
    public int DiasIncapacidad { get; private set; }  // ⚠️ Sigue siendo problemático
    public string Estado { get; private set; }
    
    public void CambiarDias(int dias)
    {
        DiasIncapacidad = dias;  // Sin validación de negocio
    }
    
    public void CambiarEstado(string estado)
    {
        Estado = estado;  // Sin transiciones válidas
    }
}
```

### ✅ BIEN: Encapsulamiento Correcto

```csharp
// ✅ BIEN: Campos privados + métodos semánticos
public sealed class Incapacidad
{
    private readonly Empleado _empleado;
    private readonly TipoIncapacidad _tipo;
    private readonly PeriodoIncapacidad _periodo;
    private EstadoIncapacidad _estado;
    
    public Incapacidad(
        Empleado empleado,
        TipoIncapacidad tipo,
        PeriodoIncapacidad periodo,
        CodigoDiagnostico diagnostico)
    {
        _empleado = empleado ?? throw new ArgumentNullException(nameof(empleado));
        _tipo = tipo ?? throw new ArgumentNullException(nameof(tipo));
        _periodo = periodo ?? throw new ArgumentNullException(nameof(periodo));
        _estado = EstadoIncapacidad.Registrada;
    }
    
    // ✅ El objeto sabe calcular sus propios días
    public int DiasACargoEmpleador => 
        Math.Min(_tipo.DiasACargoEmpleador, _periodo.TotalDias);
    
    public int DiasACargoEPS => 
        Math.Max(0, _periodo.TotalDias - _tipo.DiasACargoEmpleador);
    
    // ✅ El objeto sabe si puede generar recobro
    public bool PuedeGenerarRecobro() => 
        DiasACargoEPS > 0 && _estado == EstadoIncapacidad.Validada;
    
    // ✅ El objeto calcula su propio monto a recobrar
    public Dinero MontoARecobrar()
    {
        var salarioDiario = _empleado.SalarioDiario();
        return salarioDiario
            .Multiplicar(DiasACargoEPS)
            .Multiplicar(_tipo.PorcentajeRecobro);
    }
    
    // ✅ El objeto genera su propio recobro
    public Recobro GenerarRecobro()
    {
        if (!PuedeGenerarRecobro())
            throw new NoSePuedeGenerarRecobroException(this);
        
        return new Recobro(this, MontoARecobrar(), _epsQueExpide);
    }
    
    // ✅ Transiciones de estado con nombre semántico del dominio
    public void Validar()
    {
        if (_estado != EstadoIncapacidad.Registrada)
            throw new TransicionEstadoInvalidaException(_estado, EstadoIncapacidad.Validada);
        
        _estado = EstadoIncapacidad.Validada;
    }
}
```

### Principio "Tell, Don't Ask"

```csharp
// ❌ MAL: Ask - Preguntar y luego decidir afuera
if (incapacidad.Estado == "Validada" && incapacidad.DiasACargoEPS > 0)
{
    var recobro = new Recobro(...);
}

// ✅ BIEN: Tell - Decirle al objeto qué hacer
var recobro = incapacidad.GenerarRecobro();  // El objeto decide internamente
```

---

# Dominio Bitákora

## Entidades Principales

| Entidad | Descripción | Responsabilidades |
|---------|-------------|-------------------|
| `Empleado` | Persona vinculada laboralmente | Calcular salario diario, verificar afiliaciones |
| `Incapacidad` | Licencia médica de un empleado | Calcular días, generar recobro, validar estado |
| `Recobro` | Proceso de recuperar dinero de EPS | Radicar, registrar pago |
| `Nomina` | Liquidación de un período | Calcular devengados, deducciones, neto a pagar |
| `EPS` | Entidad Promotora de Salud | Validar afiliación, procesar recobros |
| `AFP` | Administradora de Fondos de Pensiones | Calcular aportes pensionales |

## Value Objects

| Value Object | Descripción | Ejemplo |
|--------------|-------------|---------|
| `Dinero` | Monto con moneda | `Dinero.Pesos(1_500_000)` |
| `NumeroIdentificacion` | Cédula, CE, Pasaporte | `NumeroIdentificacion.Cedula("1234567890")` |
| `PeriodoIncapacidad` | Rango de fechas | `new PeriodoIncapacidad(inicio, fin)` |
| `Salario` | Base + tipo | `new Salario(Dinero.Pesos(2_000_000), TipoSalario.Ordinario)` |
| `CodigoDiagnostico` | CIE-10 | `new CodigoDiagnostico("J11", "Influenza")` |
| `Nit` | NIT de empresa/EPS | `new Nit("900123456-1")` |

## Tipos de Incapacidad (Polimorfismo)

```csharp
TipoIncapacidad
├── IncapacidadComun        // 2 días empleador, 66.67% recobro
├── IncapacidadLaboral      // ARL, 100% recobro
└── IncapacidadMaternidad   // 18 semanas, 100% recobro EPS
```

## Estados de Incapacidad

```
Registrada → Validada → RecobroGenerado → RecobradoPendiente → RecobroPagado
         ↘ Rechazada
```

---

# Checklist para Code Reviews

## Tres Ejes de Evaluación

| Eje | Pregunta | Señal de Problema |
|-----|----------|-------------------|
| **Implementativo** | ¿Cumple con los NFRs? | Performance degradada, timeouts |
| **Descriptivo** | ¿Puedo entender el negocio leyendo el código? | Nombres genéricos como `Process()`, `Data` |
| **Funcional** | ¿Cada concepto de Bitákora tiene su objeto? | Strings sueltos para EPS, tipos de incapacidad |

## Heurísticas

| Heurística | Pregunta de Verificación | Señal de Problema |
|------------|-------------------------|-------------------|
| **H1** | ¿Hay un objeto por ente? | `decimal monto` + `string moneda` separados |
| **H2** | ¿Los objetos nacen completos? | Constructores vacíos, setters públicos |
| **H4** | ¿Se evita null? | `null` como valor de retorno |
| **H5** | ¿Se favorece inmutabilidad? | Setters en Value Objects |
| **H6** | ¿Se respeta encapsulamiento? | `{ get; set; }` público, lógica fuera del objeto |

## Arquitectura Onion

| Verificación | Cumple |
|--------------|--------|
| ¿El Dominio tiene cero referencias a Datos o Api? | ☐ |
| ¿Los casos de uso están en Aplicación, no en Api? | ☐ |
| ¿Las interfaces están en las capas internas? | ☐ |
| ¿La inversión de dependencias está correcta? | ☐ |

---

# Instrucciones para Claude

Cuando trabajes en este proyecto (dominio Bitákora):

## Heurísticas de Diseño (aplicar por defecto, evaluar en contexto)

1. **Prioriza el modelado del dominio Bitákora** sobre soluciones técnicas genéricas
2. **Sugiere crear Value Objects** para: `Dinero`, `NumeroIdentificacion`, `PeriodoIncapacidad`, `Nit`, `CodigoDiagnostico`
3. **Propón polimorfismo** para tipos de incapacidad (Común, Laboral, Maternidad) en lugar de switches
4. **Evita sugerir null** — usa Null Object Pattern o excepciones
5. **Nombra según Bitákora**: `Incapacidad`, `Recobro`, `Empleado`, no `Entity`, `DataProcessor`
6. **Cuestiona los Services** — ¿esa lógica debería estar en `Incapacidad` o `Empleado`?
7. **Define objetos por sus mensajes** — `PuedeGenerarRecobro()`, `MontoARecobrar()`, no por datos
8. **Favorece inmutabilidad** — especialmente para Value Objects (`Dinero`, `Periodo`)
9. **Protege el encapsulamiento** — campos privados, métodos con semántica del dominio
10. **Respeta la Arquitectura Onion** — el Dominio no conoce la Api ni los Datos

## Patrones C# Preferidos

```csharp
// ✅ Usar records para Value Objects
public sealed record Dinero(decimal Monto, Moneda Moneda);

// ✅ Usar constructores con validación
public Empleado(NumeroIdentificacion id, ...) { ... }

// ✅ Usar excepciones de dominio específicas
throw new EmpleadoNoEncontradoException(identificacion);

// ✅ Usar Nullable Reference Types
public Incapacidad? BuscarActiva(NumeroIdentificacion id);

// ✅ Usar métodos semánticos
public bool PuedeGenerarRecobro() => ...
public Recobro GenerarRecobro() => ...

// ❌ Evitar
public string Estado { get; set; }  // Setter público
public object? Data { get; set; }   // Tipo genérico
if (obj == null) ...               // null explícito sin NRT
```

## Anti-Patrones a Detectar

| Anti-Patrón | Señal | Sugerencia |
|-------------|-------|------------|
| Primitive Obsession | `string tipoIncapacidad` | Crear `TipoIncapacidad` |
| Anemic Domain Model | Entidades solo con getters/setters | Agregar comportamiento |
| God Service | `IncapacidadService` con toda la lógica | Mover lógica a las entidades |
| Null como estado válido | `if (recobro == null)` | Null Object o excepción |
| Setters públicos | `{ get; set; }` | Campos privados + métodos semánticos |

## Dominio Específico Bitákora

Términos del dominio colombiano de nómina:
- **PILA**: Planilla Integrada de Liquidación de Aportes
- **EPS**: Entidad Promotora de Salud
- **AFP**: Administradora de Fondos de Pensiones
- **ARL**: Administradora de Riesgos Laborales
- **Recobro**: Proceso de recuperar dinero de la EPS por incapacidades
- **Devengado**: Lo que gana el empleado
- **Deducciones**: Lo que se descuenta (aportes, retenciones)
- **IBC**: Ingreso Base de Cotización

---

# Referencias

- **Curso**: Heurísticas de Diseño de Software con Objetos - Hernán Wilkinson
- **Empresa**: [10Pines](https://www.10pines.com/)
- **Artículo**: "What is Software Design?" - Jack W. Reeves (1992)
- **Charla**: "Real Software Engineering" - Glenn Vanderburg
- **Dominio**: [Bitákora Software de Nómina](https://bitakora.co/)
- **Repositorio Base**: https://github.com/jackparra253/Incapacidades
