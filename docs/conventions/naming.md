# Naming conventions

*Last updated: 2026-05-04*

## Package IDs

`WoW.Two.Sdk.Backend.Beta.<Area>[.<SubArea>]`

Examples:

- `WoW.Two.Sdk.Backend.Beta` — meta-package
- `WoW.Two.Sdk.Backend.Beta.Time`
- `WoW.Two.Sdk.Backend.Beta.Caching`
- `WoW.Two.Sdk.Backend.Beta.Caching.Redis`
- `WoW.Two.Sdk.Backend.Beta.Identity.OAuth.Google`
- `WoW.Two.Sdk.Backend.Beta.Testing`
- `WoW.Two.Sdk.Backend.Beta.Testing.Containers.Postgres`

PascalCase. Dotted. No abbreviations except established ones (e.g., `OAuth`, `Sql`, `Http`).

## Folder names (filesystem)

`kebab-case`. Match the area portion of the package id, lowercased and hyphenated.

- `src/foundation/time/` → `WoW.Two.Sdk.Backend.Beta.Time`
- `src/web/output-cache/` → `WoW.Two.Sdk.Backend.Beta.Web.OutputCache`
- `src/identity/oauth-google/` → `WoW.Two.Sdk.Backend.Beta.Identity.OAuth.Google`

## Namespaces

Match the package id 1:1. File-scoped:

```csharp
namespace WoW.Two.Sdk.Backend.Beta.Time;
```

## Public extension classes

`<Area>ServiceCollectionExtensions` — matches Microsoft's pattern.

```csharp
public static class TimeServiceCollectionExtensions
{
    public static IServiceCollection AddWowTwoTime(this IServiceCollection services, ...) { ... }
}
```

For non-`IServiceCollection` extensions, name `<Area><Target>Extensions`:

- `TimeProviderExtensions` (extends `TimeProvider`)
- `LoggerBuilderExtensions` (extends `ILoggingBuilder`)
- `OpenTelemetryBuilderExtensions` (extends `IOpenTelemetryBuilder`)
- `EndpointConventionBuilderExtensions` (extends `IEndpointConventionBuilder`)

## Registration method names

Always **`AddWowTwo<Area>`**. The `WowTwo` infix avoids collisions with built-in `Add<Area>` and makes it grep-friendly.

```csharp
services.AddWowTwoTime();
services.AddWowTwoCaching();
services.AddWowTwoCaching(opts => opts.RedisConnection = "localhost");
```

For sub-areas:

```csharp
services.AddWowTwoCachingRedis();
services.AddWowTwoTestingContainersPostgres();
```

For `WebApplicationBuilder`-friendly registration that wires both services and middleware patterns at once:

```csharp
builder.AddWowTwoBackendBeta();   // meta — does everything
builder.AddWowTwoWeb();           // P1 web bundle
```

## Options types

`<Area>Options` — record, init-only.

```csharp
public sealed record TimeOptions
{
    public string TimeZone { get; init; } = "UTC";
}
```

For sub-areas:

```csharp
public sealed record CachingRedisOptions { ... }
```

## Internal types

Plain PascalCase. No leading underscore. Live in an `Internal/` folder when the namespace becomes crowded.

```csharp
internal sealed class TimeProviderRegistrar { ... }
```

## Test files

`<Module>.tests.cs` next to the file under test. Class name `<Module>Tests`.

```
TimeModule.cs
TimeModule.tests.cs   // class TimeModuleTests
```

## Spec / standard files

- `<Module>.standard.md` — RFC 2119 contract
- `<Module>.spec.md` — API surface + usage

## Symbols banned in our code

- `var` for non-obvious types (let analyzers complain)
- Hungarian notation (`m_`, `s_`, `_`)
- Suffix `Helper`, `Util`, `Manager` for *public* types — internal helpers ok
- `dynamic` (use generics or polymorphism)
- `BinaryFormatter` (security)

## Acronyms

Pascal-case all acronyms longer than 2 letters: `OpenApi`, `JsonSchema`, `OAuth` (special — established).

Two-letter acronyms stay all-caps: `IO`, `UI`, `ID` (but `Id` when used as a member name like `UserId`).

## Database / table names (when our packages create schema)

Snake_case (Postgres convention via `EFCore.NamingConventions`). Schema named `wow_two_<area>` (e.g., `wow_two_outbox.outbox_messages`).
