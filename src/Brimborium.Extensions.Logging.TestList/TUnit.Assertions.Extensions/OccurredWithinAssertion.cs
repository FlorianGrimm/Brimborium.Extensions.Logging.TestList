// MIT License Copyright (c) Florian Grimm

#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace TUnit.Assertions.Extensions;

public sealed class OccurredWithinAssertion : Assertion<TestLoggerSnapshot> {
    private readonly TimeSpan _ExpectedLow;
    private readonly TimeSpan _ExpectedHigh;

    public OccurredWithinAssertion(
        AssertionContext<TestLoggerSnapshot> context,
        TimeSpan expectedLow,
        TimeSpan expectedHigh
        ) : base(context) {
        this._ExpectedLow = expectedLow;
        this._ExpectedHigh = expectedHigh;
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

        var foundItem = value.GetCurrentItem();
        if (foundItem is null) {
            return Task.FromResult(AssertionResult.Failed("no found item"));
        }

        var previousFoundItem = value.GetPreviousItem() ?? value.GetItemByIndex(0);
        if (previousFoundItem is null) {
            return Task.FromResult(AssertionResult.Failed("no previous found item"));
        }

        var elapsed = foundItem.Timestamp.Subtract(previousFoundItem.Timestamp);

        if (elapsed < this._ExpectedLow) {
            return Task.FromResult(AssertionResult.Failed($" does occurred within '{elapsed}', but '{this._ExpectedLow}' minimum expected."));
        }

        if (this._ExpectedHigh >= elapsed) {
            return Task.FromResult(AssertionResult.Failed($" does occurred within '{elapsed}', but '{this._ExpectedHigh}' maximum expected."));
        }

        return Task.FromResult(AssertionResult.Passed);
    }

    protected override string GetExpectation()
        => $"occurre within minimum '{this._ExpectedLow}' and maximum '{this._ExpectedHigh}'";
}
