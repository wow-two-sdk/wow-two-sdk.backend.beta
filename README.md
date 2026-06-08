# wow-two-sdk.backend.beta

> Beta-forever .NET 9 backend SDK — wraps the .NET ecosystem behind opinionated, descriptively-named registration extensions (`AddJwtBearerAuthentication`, `AddOpenTelemetryTracing`, `AddPerIpSlidingWindowRateLimit`, …) so a `Program.cs` becomes 1–2 liners per concern instead of 100.

## Status

| Phase | Bundle | Status |
|---|---|---|
| P0 | Testing scaffold (parallel track) | ✅ 12 packages shipped |
| P1 — foundation | `time`, `errors`, `results`, `validation`, `serialization`, `guards`, `value-objects` | ✅ 7 packages shipped |
| P1 — observability | `logging`, `tracing`, `metrics`, `healthchecks`, `otlp`, `prometheus`, `azure-monitor`, `datadog` | ✅ 8 packages shipped |
| P1 — web | `hosting`, `openapi`, `problemdetails`, `ratelimit`, `outputcache`, `secureheaders`, `cors`, `compression`, `versioning` | ✅ 9 packages shipped |
| P2 — mediator | `mediator`, `mediator.{validation, logging, authorization, idempotency}` | ✅ 5 packages shipped |
| P2 — identity | `identity.{jwt, cookies, oidc, identity-api}`, `oauth.{google, microsoft, github, apple}`, `mfa.{totp, webauthn}`, `password-hashing.argon2` | ✅ 11 packages shipped |
| P3 | persistence + outbound | planned |
| P4 | distributed essentials | planned |
| P5 | SaaS-shaped (tenancy + AI + flags) | planned |
| P6 | heavy domain extensions | planned |

**52 packages shipped**, 130+ planned. See [`docs/conventions/package-registry.md`](./docs/conventions/package-registry.md).

## Active design work

In-progress patterns being designed/built — read these to know what's underway:

- [`docs/analysis/validation-and-result-pattern.md`](./docs/analysis/validation-and-result-pattern.md) — validation result model (Stage 1, building now) + the deferred generic `Result`/`Error` pattern.

## Quick start

```csharp
var builder = WebApplication.CreateBuilder(args);

// Foundation
builder.Host.UseSerilogConventional();
builder.Services
    .AddTimeProviders()
    .AddFluentValidatorsFromAssemblies(typeof(Program).Assembly);

// Observability
builder.Services
    .AddOpenTelemetryTracing("my-service")
    .AddOpenTelemetryMetrics("my-service")
    .AddOtlpExporters()
    .AddHealthChecksBuilder();

// Web
builder.Services
    .AddProxyAwareHosting()
    .AddOpenApiDefaults()
    .AddTraceAwareProblemDetails()
    .AddPerIpSlidingWindowRateLimit()
    .AddDefaultOutputCache()
    .AddBrotliGzipCompression()
    .AddDefaultCorsPolicy("https://example.com");

// Pipeline + auth
builder.Services
    .AddMediator(typeof(Program).Assembly)
    .AddMediatorValidationBehavior()
    .AddMediatorLoggingBehavior()
    .AddJwtBearerAuthentication(o =>
    {
        o.Issuer   = "https://issuer";
        o.Audience = "my-api";
        o.JwksUri  = new Uri("https://issuer/.well-known/openid-configuration");
    });

var app = builder.Build();
app.UseProxyAwareHosting();
app.UseOwaspSecureHeaders();
app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();
app.UseOutputCache();
app.UseResponseCompression();
app.MapOpenApiEndpoint();
app.Run();
```

## Read these first

- [`docs/analysis/philosophy/ideas.md`](./docs/analysis/philosophy/ideas.md) — encyclopedia of the .NET ecosystem (no verdicts)
- [`docs/analysis/philosophy/targets.md`](./docs/analysis/philosophy/targets.md) — verdicts (DONE / NOW / NEXT / LATER / MAYBE / SKIP / LOCKED)
- [`docs/conventions/package-layout.md`](./docs/conventions/package-layout.md) — repo + per-package shape
- [`docs/conventions/documentation.md`](./docs/conventions/documentation.md) — three-layer doc strategy

## Build

```bash
cd src
ulimit -n 65535          # macOS — avoid EMFILE
export MSBUILDDISABLENODEREUSE=1
dotnet restore WoW.Two.Sdk.Backend.Beta.slnx -m:1
dotnet build WoW.Two.Sdk.Backend.Beta.slnx --no-restore -m:1
```

## License

MIT.
