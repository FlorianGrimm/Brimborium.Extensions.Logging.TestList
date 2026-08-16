// MIT License Copyright (c) Florian Grimm

#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace TUnit.Assertions.Extensions;

public sealed class MatchesParameterAssertion : Assertion<TestLoggerSnapshot> {
    private readonly Dictionary<string, object?> _Expected;

    public MatchesParameterAssertion(
        AssertionContext<TestLoggerSnapshot> context,
        Dictionary<string, object?> expected
        ) : base(context) {
        this._Expected = expected;
    }


    protected override Task<AssertionResult> CheckAsync(EvaluationMetadata<TestLoggerSnapshot> metadata) {
        var value = metadata.Value;
        var exception = metadata.Exception;

        if (exception != null) {
            return Task.FromResult(AssertionResult.Failed($"threw {exception.GetType().Name}"));
        }

        if (value == null) {
            return Task.FromResult(AssertionResult.Failed("value was null"));
        }

        var item = value.GetCurrentItem();
        if (item == null) {
            return Task.FromResult(AssertionResult.Failed("value was null"));
        }

        var notmatched = TestLoggerItem.IsStateEqual(item.State, this._Expected);

        if (notmatched is null) {
            return Task.FromResult(AssertionResult.Passed);
        } else {
            object? currentItemValue = "NOT-FOUND";
            {
                _ = value.GetCurrentItem()?.State?.TryGetValue(notmatched, out currentItemValue);
                currentItemValue ??= "null";
            }

            object? expectedValue = "NOT-FOUND";
            {
                _ = this._Expected.TryGetValue(notmatched, out expectedValue);
                expectedValue ??= "null";
            }

            return Task.FromResult(AssertionResult.Failed($"{{ '{notmatched}': {currentItemValue} }} does not match {{ '{notmatched}': {expectedValue} }}'"));
        }
    }

    protected override string GetExpectation() {
        var keysExpected = string.Join(":, ", (this._Expected.Keys) ?? Enumerable.Empty<string>());
        return $"to match {{ {keysExpected} }}";
    }
}