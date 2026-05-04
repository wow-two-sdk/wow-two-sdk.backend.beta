# WoW.Two.Sdk.Backend.Beta.ValueObjects

> Re-exports [Vogen](https://github.com/SteveDunn/Vogen) (compile-time value-object generator) and [StronglyTypedId](https://github.com/andrewlock/StronglyTypedId) (compile-time strongly-typed ID generator).

## Install

```
dotnet add package WoW.Two.Sdk.Backend.Beta.ValueObjects
```

## Usage

### Strongly typed ID

```csharp
using StronglyTypedIds;

[StronglyTypedId(Template.Guid)]
public partial struct UserId { }

[StronglyTypedId(Template.Long)]
public partial struct OrderId { }
```

### Value object with validation

```csharp
using Vogen;

[ValueObject<string>]
public partial struct Email
{
    private static Validation Validate(string input) =>
        input.Contains('@')
            ? Validation.Ok
            : Validation.Invalid("Email must contain '@'.");
}
```

## See also

- [Vogen docs](https://github.com/SteveDunn/Vogen)
- [StronglyTypedId docs](https://github.com/andrewlock/StronglyTypedId)
