// MIT License Copyright (c) Florian Grimm

namespace Brimborium.Extensions.Logging.TestList;

public sealed record class TestLoggerItem(
    DateTimeOffset Timestamp = default,
    LogLevel LogLevel = LogLevel.None,
    EventId EventId = default,
    string Category = "",
    string Message = "",
    string OriginalFormat = "",
    Dictionary<string, object?>? State = default
    ) {
    public string? IsEqualTo(TestLoggerItem searchFor) {
        // do not compare this.Timestamp
        if ((searchFor.LogLevel == LogLevel.None) || (searchFor.LogLevel == this.LogLevel)) { } else { return nameof(this.LogLevel); }

        if ((searchFor.EventId.Id == 0) || (searchFor.EventId.Id == this.EventId.Id)) { } else { return "EventId.Id"; }
        if ((string.IsNullOrEmpty(searchFor.EventId.Name)) || (string.Equals(searchFor.EventId.Name, this.EventId.Name, StringComparison.Ordinal))) { } else { return "EventId.Name"; }

        if ((string.IsNullOrEmpty(searchFor.Category)) || (string.Equals(searchFor.Category, this.Category, StringComparison.Ordinal))) { } else { return nameof(this.Category); }

        if ((string.IsNullOrEmpty(searchFor.Message)) || (string.Equals(searchFor.Message, this.Message, StringComparison.Ordinal))) { } else { return nameof(this.Message); }
        if ((string.IsNullOrEmpty(searchFor.OriginalFormat)) || (string.Equals(searchFor.OriginalFormat, this.OriginalFormat, StringComparison.Ordinal))) { } else { return nameof(this.OriginalFormat); }

        return (TestLoggerItem.IsStateEqual(searchFor.State, this.State));
    }

    public static string? IsStateEqual(
        Dictionary<string, object?>? searchForState,
        Dictionary<string, object?>? thisState) {
        if (searchForState is null || 0 == searchForState.Count) { return null; }
        if (thisState is null) { return nameof(thisState); }

        foreach (var (key, value) in searchForState) {
            // try get thisState of key value
            if (!thisState.TryGetValue(key, out var thisValue)) { return key; }

            // compares value thisValue
            if (ReferenceEquals(value, thisValue)) { continue; }
            if (value is null) { return key; }
            {
                if (value is string valueString) {
                    if (thisValue is string thisValueString) {
                        if (string.Equals(valueString, thisValueString, StringComparison.Ordinal)) {
                            continue;
                        }
                    }
                } else {
                    if (value.Equals(thisValue)) {
                        continue;
                    }
                }
            }
            return key;
        }
        return null;
    }

    public string ToStringMinimal() {
        System.Text.StringBuilder sb = new();
        _ = sb.Append('{');

        if (this.LogLevel == LogLevel.None) {
            // skip
        } else {
            _ = sb.Append($" LogLevel:{this.LogLevel};");
        }

        if (this.EventId.Id == 0 && string.IsNullOrEmpty(this.EventId.Name)) {
            // skip
        } else {
            _ = sb.Append($" EventId:{this.EventId};");
        }

        if (string.IsNullOrEmpty(this.Category)) {
            // skip
        } else {
            _ = sb.Append($" Category:{this.Category};");
        }

        if (string.IsNullOrEmpty(this.Message)) {
            // skip
        } else {
            _ = sb.Append($" Message:{this.Message};");
        }

        if (string.IsNullOrEmpty(this.OriginalFormat)) {
            // skip
        } else {
            _ = sb.Append($" OriginalFormat:{this.OriginalFormat};");
        }

        _ = sb.Append(" }");

        return sb.ToString();
    }
}