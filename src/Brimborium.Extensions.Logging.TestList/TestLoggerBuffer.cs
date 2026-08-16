// MIT License Copyright (c) Florian Grimm

namespace Brimborium.Extensions.Logging.TestList;

/// <summary>
/// a list with an Lock
/// </summary>
public sealed class TestLoggerBuffer {
    private readonly Lock _Lock = new();
    private readonly List<TestLoggerItem> _ListItem;

    public TestLoggerBuffer() {
        this._ListItem = new List<TestLoggerItem>();
    }

    public TestLoggerItem[] ListItem {
        get {
            using (this._Lock.EnterScope()) {
                return [.. this._ListItem];
            }
        }
    }

    public TestLoggerSnapshot GetSnapshot() {
        using (this._Lock.EnterScope()) {
            return new TestLoggerSnapshot([.. this._ListItem]);
        }
    }

    public void Add(TestLoggerItem item) {
        if (item.Timestamp == default(DateTimeOffset)) {
            item = item with {
                Timestamp = DateTimeOffset.UtcNow
            };
        }
        using (this._Lock.EnterScope()) {
            this._ListItem.Add(item);
        }
    }

    public void Add(
        DateTimeOffset timestamp,
        LogLevel logLevel = default,
        EventId eventId = default,
        string category = "",
        string message = "",
        string originalFormat = "",
        Dictionary<string, object?>? state = default
        ) {
        if (timestamp == default(DateTimeOffset)) {
            timestamp = DateTimeOffset.UtcNow;
        }
        using (this._Lock.EnterScope()) {
            var item = new TestLoggerItem(
                Timestamp: timestamp,
                LogLevel: logLevel,
                EventId: eventId,
                Category: category,
                Message: message,
                OriginalFormat: originalFormat,
                State: state);
            this._ListItem.Add(item);
        }
    }

    public void Clear() {
        using (this._Lock.EnterScope()) {
            this._ListItem.Clear();
        }
    }
}
