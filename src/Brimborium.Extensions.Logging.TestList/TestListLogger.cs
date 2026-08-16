// MIT License Copyright (c) Florian Grimm

namespace Brimborium.Extensions.Logging.TestList;

/// <summary>
/// A logger that writes messages in the debug output window only when a debugger is attached.
/// </summary>
public sealed partial class TestListLogger : ILogger {
    private readonly TestLoggerBuffer _TestLoggerBuffer;
    private readonly string _Name;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestListLogger"/> class.
    /// </summary>
    /// <param name="name">The name of the logger.</param>
    public TestListLogger(TestLoggerBuffer testLoggerBuffer, string name) {
        this._TestLoggerBuffer = testLoggerBuffer;
        this._Name = name;
    }

    /// <inheritdoc />
    public IDisposable BeginScope<TState>(TState state) where TState : notnull {
        return NullScope.Instance;
    }

    /// <inheritdoc />
    public bool IsEnabled(LogLevel logLevel) {
        // Everything is enabled unless the debugger is not attached
        return logLevel != LogLevel.None;
    }

    /// <inheritdoc />
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) {
        if (!this.IsEnabled(logLevel)) {
            return;
        }

        ArgumentNullException.ThrowIfNull(formatter);

        string formatted = formatter(state, exception);

        string originalFormat = string.Empty;
        Dictionary<string, object?> dictState = new();
        if (state is IReadOnlyList<KeyValuePair<string, object?>> list) {
            foreach (var (key, value) in list) {
                if (string.Equals("{OriginalFormat}", key, StringComparison.Ordinal)) {
                    if (value is string valueOriginalFormat) { 
                        originalFormat = valueOriginalFormat;
                    }
                } else { 
                    dictState[key] = value;
                }
            }
        }

        if (string.IsNullOrEmpty(formatted) && exception == null) {
            // With no formatted message or exception, there's nothing to print.
            return;
        }

        string message;
        if (string.IsNullOrEmpty(formatted)) {
            System.Diagnostics.Debug.Assert(exception != null);
            message = $"{exception}";
        } else if (exception == null) {
            message = formatted;
        } else {
            message = $"{formatted}{Environment.NewLine}{Environment.NewLine}{exception}";
        }

        this._TestLoggerBuffer.Add(
            timestamp: DateTimeOffset.UtcNow,
            logLevel: logLevel,
            eventId: eventId,
            category: this._Name,
            message: message,
            originalFormat: originalFormat,
            state: dictState);
    }
}
