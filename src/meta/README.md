# WoW.Two.Sdk.Backend.Beta

> Meta-package — installs the curated WoW Two backend SDK bundle.

## Install

```
dotnet add package WoW.Two.Sdk.Backend.Beta
```

## What you get

This package depends on every "default" wrapper in P1–P5. Installing it pulls in the entire baseline: foundation, observability, web, mediator, identity, data, caching, http, messaging, jobs, comms, tenancy, AI, and feature-flags.

Testing tools (`WoW.Two.Sdk.Backend.Beta.Testing` family) are a **separate** meta — they ship in test projects, not production assemblies.

## Quick start

```csharp
using WoW.Two.Sdk.Backend.Beta;

var builder = WebApplication.CreateBuilder(args);

// One call wires the curated baseline (foundation + observability + web + mediator + identity).
builder.AddBackendBeta();

var app = builder.Build();
app.Run();
```

> The meta-level `AddBackendBeta()` ships once the per-area defaults stabilize. Until then, compose the per-area extensions directly (see the root [README.md](../../README.md) Quick start).

## See also

- [Phase mapping](../../docs/analysis/philosophy/targets.md#6-phase-mapping)
- [Package registry](../../docs/conventions/package-registry.md)
- Per-area README in `src/<area>/`
