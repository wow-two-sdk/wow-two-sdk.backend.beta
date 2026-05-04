namespace WoW.Two.Sdk.Backend.Beta.Errors;

/// <summary>
/// Canonical error category — opinionated mapping to HTTP status codes.
/// </summary>
public enum WowErrorCategory
{
    /// <summary>Caller sent invalid input. Maps to 400.</summary>
    Validation = 400,

    /// <summary>Caller is unauthenticated. Maps to 401.</summary>
    Unauthorized = 401,

    /// <summary>Caller is authenticated but lacks permission. Maps to 403.</summary>
    Forbidden = 403,

    /// <summary>Resource not found. Maps to 404.</summary>
    NotFound = 404,

    /// <summary>Conflicting state. Maps to 409.</summary>
    Conflict = 409,

    /// <summary>Domain rule violation (semantically valid request, business state rejects it). Maps to 422.</summary>
    BusinessRule = 422,

    /// <summary>Rate limit exceeded. Maps to 429.</summary>
    TooManyRequests = 429,

    /// <summary>Unexpected server error. Maps to 500.</summary>
    Unexpected = 500,
}

/// <summary>
/// Domain error record. Lightweight, immutable, and intentionally HTTP-aware.
/// </summary>
/// <param name="Code">Stable code (e.g. <c>orders.not_found</c>).</param>
/// <param name="Message">Human-readable message. Localized when possible.</param>
/// <param name="Category">Category controlling default HTTP mapping.</param>
/// <param name="Detail">Optional additional context.</param>
public sealed record WowError(
    string Code,
    string Message,
    WowErrorCategory Category,
    string? Detail = null)
{
    /// <summary>HTTP status code derived from <see cref="Category"/>.</summary>
    public int StatusCode => (int)Category;

    /// <summary>Convenience factory — validation error.</summary>
    public static WowError Validation(string code, string message, string? detail = null) =>
        new(code, message, WowErrorCategory.Validation, detail);

    /// <summary>Convenience factory — not-found.</summary>
    public static WowError NotFound(string code, string message, string? detail = null) =>
        new(code, message, WowErrorCategory.NotFound, detail);

    /// <summary>Convenience factory — conflict.</summary>
    public static WowError Conflict(string code, string message, string? detail = null) =>
        new(code, message, WowErrorCategory.Conflict, detail);

    /// <summary>Convenience factory — forbidden.</summary>
    public static WowError Forbidden(string code, string message, string? detail = null) =>
        new(code, message, WowErrorCategory.Forbidden, detail);

    /// <summary>Convenience factory — unauthorized.</summary>
    public static WowError Unauthorized(string code, string message, string? detail = null) =>
        new(code, message, WowErrorCategory.Unauthorized, detail);

    /// <summary>Convenience factory — business-rule violation.</summary>
    public static WowError BusinessRule(string code, string message, string? detail = null) =>
        new(code, message, WowErrorCategory.BusinessRule, detail);

    /// <summary>Convenience factory — unexpected server error.</summary>
    public static WowError Unexpected(string code, string message, string? detail = null) =>
        new(code, message, WowErrorCategory.Unexpected, detail);
}
