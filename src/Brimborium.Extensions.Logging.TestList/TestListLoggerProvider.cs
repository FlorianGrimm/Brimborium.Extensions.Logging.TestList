// MIT License Copyright (c) Florian Grimm

namespace Brimborium.Extensions.Logging.TestList;

/// <summary>
/// The provider for the <see cref="TestListLogger"/>.
/// </summary>
[ProviderAlias("TestList")]
public sealed class TestListLoggerProvider : ILoggerProvider {
    private readonly TestLoggerBuffer _TestLoggerBuffer;

    public TestListLoggerProvider(TestLoggerBuffer testLoggerBuffer) {
        this._TestLoggerBuffer = testLoggerBuffer;
    }

    /// <inheritdoc />
    public ILogger CreateLogger(string name) {
        return new TestListLogger(this._TestLoggerBuffer, name);
    }

    /// <inheritdoc />
    public void Dispose() {
    }
}
