# Octopath Traveler — Entrega 2 (E2)

**Integrante:** Sebastián San Martín Ibacache

Implementación de las acciones **Usar Habilidad** y **Defender**, junto con
habilidades activas y pasivas de viajeros y habilidades de bestias, el sistema
de **debilidades** y **Breaking Point**, y el **bonus E3**.

## Estructura

- `Octopath-Traveler-Controller/`
  - `Controller/`: `CombatController`, `CombatReporter`, `CombatResolver`, `ActionSelector`,
    `TurnQueue`, `TeamBuilder` y `Actions/` (`ITravelerAction` + Ataque básico / Usar habilidad / Defender / Huir).
  - `Model/`: `Units/`, `Skills/` (`Damage`, `Effects`, `Targeting`), `Combat/` y `Data/`.
- `Octopath-Traveler-View/`: `View` y `Display/` (records de presentación).
- `Octopath-Traveler.Tests/`: runner de tests (provisto por el curso, no modificado).
- `data/` — equipos, catálogos y casos de prueba (provista por el curso; no se versiona).

## Compilar y ejecutar

```
dotnet build
dotnet run --project Octopath-Traveler-Controller
```

## Cómo probar

```
dotnet test --filter "FullyQualifiedName~TestE1"   # E1: 122 tests
dotnet test --filter "FullyQualifiedName~TestE2"   # E2: 207 tests
```

## Reglas implementadas

- Ataque básico con **debilidad** (`con debilidad`, ×1.5) y **Breaking Point** (×1.5;
  debilidad + BP = ×2); el daño final se trunca con `Math.Floor`.
- **Usar habilidad**: objetivos `Single / Enemies / User / Ally / Party`; consume SP y
  muestra el menú de BP (en E2 no se evalúa el efecto del boosting).
- **Defender**: reduce un 50% el daño hasta el fin de la ronda, otorga prioridad en la
  ronda siguiente y solo anuncia `X se defiende` al recibir un golpe.
- **Debilidades**: bajan Shields solo si el ataque es de un tipo débil y hace daño; al
  llegar a 0 la bestia entra en **Breaking Point** (pierde la ronda actual y la siguiente;
  al recuperarse resetea Shields y obtiene prioridad).
- Curación, revivir y habilidades de cola de turnos (Spearhead, Leghold Trap), respetando
  el orden de anuncios de múltiples objetivos del enunciado.

## Bonus E3

- Boosting en ataque básico: 1 golpe + 1 por BP (máx. 3); quien usa boosting no recibe BP
  al final de la ronda.
- Pasivas: `Vim and Vigor`, `Second Wind`, `Patience`, `Boost Start`, `Stat Swap`.

## Arquitectura

- **Polimorfismo por composición** para habilidades: `ITargetSelector`, `ISkillEffect`,
  `IDamageBonus`, `IDamageCap`; factories con diccionario `nombre → creador`.
- **MVC** estricto: el Modelo no conoce Vista ni Controlador; el Controlador no usa
  `WriteLine/ReadLine`, la Vista no depende de Controller/Model.
- Clean Code (capítulos 2, 3, 6 y 10): nombres intencionales, funciones cortas, ≤3
  argumentos, privado por defecto y sin código duplicado.

## Supuestos y limitaciones

- El uso de BP en habilidades no se evalúa en esta entrega (sí el menú y su validación).
- `HP Thief` implementado como 2 golpes de Dagger, sin el robo de vida: no está en la
  lista de habilidades de E2 ni se ejercita en los tests.
- Los grupos E3/E4 fuera del bonus quedan fuera del alcance de esta entrega.
- `Tests.cs` y el `.csproj` del proyecto de tests **no se modificaron**.

## Uso de herramientas de IA

- La implementación y las decisiones del proyecto las tomé yo, apoyándome en una IA
  (DeepSeek v4) durante el desarrollo.
- La IA me ayudó de manera extensiva a la hora de modelar las clases (acciones del viajero,
  habilidades activas y pasivas, habilidades de bestias) y con el modelo MVC y su estructura
  (Modelo / Vista / Controlador).
- Me aconsejó modelar las habilidades con **polimorfismo** (objetivos y
  efectos como estrategias que se combinan por composición), en vez de `if`/`switch` por
  nombre de habilidad.
- También me ayudó con la redacción de este README.
- Se utilizó GitHub Copilot para la autocompletación de código durante el desarrollo.
