// MIT License Copyright (c) Florian Grimm

#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace TUnit.Assertions.Extensions;

public sealed class MatchesAssertion : Assertion<TestLoggerSnapshot> {
    private readonly TestLoggerItem _Expected;

    public MatchesAssertion(
        AssertionContext<TestLoggerSnapshot> context,
        TestLoggerItem expected
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

        var item=value.GetCurrentItem();
        if (item == null) {
            return Task.FromResult(AssertionResult.Failed("value was null"));
        }

        var notmatched = item.IsEqualTo(this._Expected);

        if (notmatched is null) {
            return Task.FromResult(AssertionResult.Passed);
        } else {
            return Task.FromResult(AssertionResult.Failed($"'{value}' is not equal '{notmatched}' of '{this._Expected.ToStringMinimal()}'"));
        }
    }

    protected override string GetExpectation()
        => $"to contain \"{this._Expected.ToStringMinimal()}\"";
}
