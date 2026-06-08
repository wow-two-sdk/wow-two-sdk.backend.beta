# Validation & Result pattern

*Last updated: 2026-06-03 · Status: **Stage 2 (generic Result) built**; Stage 3 (validation engine) next*

Kit-owned validation + result pattern. Built bottom-up; the whole concern lands as one commit.
This doc is the durable design memory — any session can resume from it.

## Goal
- A **validation model** + a kit-owned **generic Result** for operation outcomes.
- **Pure validators** — no I/O, no exceptions. "Does state exist / is it unique?" is a *handler* concern.
- **FluentValidation behind our own `IValidator<T>` wrapper** — swappable; the public contract is our types.

## House DU style
`Result` / `Result<T>` / `ValidationResult` nest `Success` / `Failure` (matches vault + Haven, the mental
model). `CA1034` (nested public types) is suppressed in `src/.editorconfig` — the nesting is the intended
DU shape, not an accident.

## Validation model — Stage 1 (done)
`src/Foundation/Validation/`
```
ValidationResult (abstract)
 ├─ Success
 └─ Failure(IReadOnlyList<ValidationError> Errors)
ValidationError(string Property, string Message, string Code)
```
Separate success/failure; simple success (no warnings / severity / category — deferred).

## Generic Result — Stage 2 (built)
`src/Foundation/Results/`
```
Result (abstract)          Result<T> (abstract)
 ├─ Success                 ├─ Success(T Value)
 └─ Failure(DomainError)    └─ Failure(DomainError)
```
`IsSuccess`, `Ok` / `Fail` factories, `Match`, `Map`. **Failure carries a single `DomainError`** (the kit's
existing error record). No multi-error, no error union.

## Validation engine — Stage 3 (next)
- `IValidator<T>` (our wrapper): `ValidationResult Validate(T)` (no throw) + `void ValidateAndThrow(T)`
  (throws `ValidationException`). Names mirror FluentValidation.
- `ValidationException(IReadOnlyList<ValidationError>)`.
- FV adapter implements `IValidator<T>`, mapping FV output → our model.
- Refactor existing `Mediator/Validation/ValidationBehavior` to call our `ValidateAndThrow` + throw our exception.
- **Sync only.** Async (IO validators) → a *separate* `IAsyncValidator<T>` when needed — never two methods on one interface.

## Error model — collapsed (was a 3-variant hierarchy)
Earlier sketch: `Error = DomainError | ValidationError | ExceptionalError`. **Dropped** — the union forced
every failure check to branch on error-kind. Simpler rule:
- **Expected, nameable failure** → `Result.Failure(DomainError)` (single error).
- **Validation failure** → stays in `ValidationResult` / `ValidationException`; never enters a `Result`.
- **Truly unexpected** → `throw`; the boundary (ProblemDetails middleware) maps to 500.

Validation and operation Result are **separate channels** — they don't convert into each other.

## Decisions
**Locked**
- Pure validators; IO / existence / uniqueness checks live in handlers.
- FV behind our `IValidator<T>` wrapper (swappable).
- `Validate` (return) + `ValidateAndThrow` (throw); sync only now.
- `Result.Failure` carries one `DomainError`; no `ExceptionalError`, no error union.
- Nested DU style; `CA1034` suppressed.

**Deferred**
- Warnings-on-success, severity, error-category polish.
- Async validators (separate `IAsyncValidator<T>`).
- DB/config-aware rules *inside* validators — needs its own brainstorm.
- `<example>` tags on result types (convention wants them; one-liner summaries for now).
- Message localization.

## Value objects (Vogen) — scope narrowed
We will **not** use Vogen's in-type `Validate` — its hook is static (no DI/config), and our validation is
config-driven and case-specific, so it lives **externally** in FV validators (which are DI-resolved and can
inject config). Residual Vogen value: **strongly-typed IDs** + primitive-obsession / type-distinction only.

## Reference models surveyed
- **Vault** — `Result` / `Result<T>` (class, nested Success/Failure, enum + message, Match/Map).
- **Kit** — `DomainError` (Code + Category=HTTP + Detail).
- **Haven** — `ApplicationResult<TSuccess, TFailure>` (record; both sides typed; success/failure context channels).
