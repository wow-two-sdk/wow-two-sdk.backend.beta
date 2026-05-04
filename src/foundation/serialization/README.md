# WoW.Two.Sdk.Backend.Beta.Serialization

> Pre-built `JsonSerializerOptions` for `System.Text.Json` — camelCase, NodaTime-aware, lenient input parsing, AOT-friendly.

## Install

```
dotnet add package WoW.Two.Sdk.Backend.Beta.Serialization
```

## Usage

```csharp
using WoW.Two.Sdk.Backend.Beta.Serialization;

// Wire as ASP.NET Core's default
builder.Services.ConfigureHttpJsonOptions(o =>
{
    var defaults = WowJsonOptions.Default;
    foreach (var c in defaults.Converters) o.SerializerOptions.Converters.Add(c);
    o.SerializerOptions.PropertyNamingPolicy = defaults.PropertyNamingPolicy;
    // (or copy other properties)
});

// Or use directly
var json = JsonSerializer.Serialize(model, WowJsonOptions.Default);
var pretty = JsonSerializer.Serialize(model, WowJsonOptions.Indented);
```

## Defaults

- camelCase property + dictionary keys
- Ignore null on writes
- Numbers can be read from strings, NaN/Infinity allowed
- Trailing commas allowed, comments skipped
- NodaTime types serialized via `NodaTime.Serialization.SystemTextJson`
- Relaxed JS escaping for cleaner output (XSS-safe per `JavaScriptEncoder.UnsafeRelaxedJsonEscaping` semantics)
