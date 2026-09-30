# Octopath Traveler — Entrega 1 (E1)

**Integrante:** Sebastián San Martín Ibacache

Implementación del flujo principal de combate de Octopath Traveler: ataques
básicos, huir, cancelar acciones, orden de turnos por Speed, validación de
equipos, lectura de unidades desde JSON y cálculo de daño.

## Requisitos

- .NET SDK 8 (ver `global.json`).

## Estructura

- `Octopath-Traveler-Controller/` — lógica del juego (Game, Battle, Model, Data, Combat).
- `Octopath-Traveler-View/` — capa de input/output (provista por el curso).
- `Octopath-Traveler.Tests/` — tests (provistos por el curso, no modificados).
- `data/` — equipos, unidades y casos de prueba (provista por el curso; no se versiona).

## Compilar y ejecutar

```
dotnet build
dotnet run --project Octopath-Traveler-Controller
```

El `Program` permite replicar un test case o ejecutar la partida en consola manual.

## Cómo probar

```
dotnet test --filter "FullyQualifiedName~TestE1"
```

Grupos: `E1-BasicCombat`, `E1-InvalidTeams`, `E1-RandomBasicCombat` (122 tests).
Se debe colocar la carpeta `data/` antes de ejecutar los tests.

## Uso de herramientas de IA

- Se utilizó una IA (DeepSeek v4) para ayudarme y aconsejarme sobre el diseño de la arquitectura de
  clases en C# (separación Modelo / Datos / Combate / Vista).
- Se utilizó GitHub Copilot para la autocompletación de código durante el desarrollo.

## Clean Code

El código aplica los principios de Clean Code vistos en clase (capítulos 2 y 3):
nombres intencionales y descriptivos, funciones cortas que hacen una sola cosa,
pocos argumentos, constantes encapsuladas y convenciones de nombres de C#.
