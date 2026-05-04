# WoW.Two.Sdk.Backend.Beta.Results

> Curated set of result / discriminated-union types — bundle of `ErrorOr`, `OneOf`, and `Ardalis.Result` so consumers pick the style they prefer without juggling separate references.

## Install

```
dotnet add package WoW.Two.Sdk.Backend.Beta.Results
```

## Usage

### ErrorOr (most common)

```csharp
public ErrorOr<User> GetUser(Guid id) =>
    _repo.Find(id) is { } user
        ? user
        : Error.NotFound(description: "User not found");
```

### OneOf

```csharp
public OneOf<Success, NotFound, Conflict> Update(...) =>
    _repo.Update(...) ? new Success() : new Conflict();
```

### Ardalis.Result

```csharp
public Result<Order> Place(...) =>
    Result<Order>.Success(order);
```

## See also

- [ErrorOr](https://github.com/amantinband/error-or)
- [OneOf](https://github.com/mcintyre321/OneOf)
- [Ardalis.Result](https://github.com/ardalis/Result)
