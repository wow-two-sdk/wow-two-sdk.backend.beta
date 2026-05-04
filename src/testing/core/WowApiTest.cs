using Microsoft.Extensions.Time.Testing;
using Xunit;

namespace WoW.Two.Sdk.Backend.Beta.Testing;

/// <summary>
/// Convenience xUnit base class for an integration test that needs a <see cref="WowTestHost{TEntryPoint}"/>.
/// Implements <see cref="IAsyncLifetime"/> so per-test setup/teardown can be overridden.
/// </summary>
/// <typeparam name="TEntryPoint">The application entry-point type (typically <c>Program</c>).</typeparam>
public abstract class WowApiTest<TEntryPoint> : IAsyncLifetime, IDisposable
    where TEntryPoint : class
{
    private readonly Lazy<WowTestHost<TEntryPoint>> _host;
    private bool _disposed;

    /// <summary>
    /// Creates a new test, lazily building the host on first access.
    /// </summary>
    protected WowApiTest()
    {
        _host = new Lazy<WowTestHost<TEntryPoint>>(BuildHost);
    }

    /// <summary>The shared test host (lazy).</summary>
    protected WowTestHost<TEntryPoint> Host => _host.Value;

    /// <summary>An <see cref="HttpClient"/> against the host.</summary>
    protected HttpClient Client => Host.CreateClient();

    /// <summary>The fake clock controlling <see cref="TimeProvider"/> in the host.</summary>
    protected FakeTimeProvider Clock => Host.Clock;

    /// <summary>Hook called per-test instance to assemble the host.</summary>
    protected virtual WowTestHost<TEntryPoint> BuildHost() => new();

    /// <inheritdoc />
    public virtual Task InitializeAsync() => Task.CompletedTask;

    /// <inheritdoc />
    public virtual Task DisposeAsync()
    {
        Dispose();
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        if (_host.IsValueCreated)
            _host.Value.Dispose();

        GC.SuppressFinalize(this);
    }
}
