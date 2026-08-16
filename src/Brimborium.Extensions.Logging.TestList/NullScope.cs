// MIT License Copyright (c) Florian Grimm

namespace Brimborium.Extensions.Logging.TestList;

/// <summary>
/// An empty scope without any logic
/// </summary>
internal sealed class NullScope : IDisposable {
    public static NullScope Instance { get; } = new NullScope();

    private NullScope() {
    }

    /// <inheritdoc />
    public void Dispose() {
    }
}