// MIT License Copyright (c) Florian Grimm

#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace TUnit.Assertions.Extensions;

public sealed class NextAssertion : Assertion<TestLoggerSnapshot> {
    private readonly int _Expected;

    public NextAssertion(
        AssertionContext<TestLoggerSnapshot> context,
        int expected=1
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
        
        var success = value.SeekRelative(this._Expected);

        if (success) {
            return Task.FromResult(AssertionResult.Passed);
        } else {
            return Task.FromResult(AssertionResult.Failed($"cannot seek '{this._Expected}'."));
        }
    }

    protected override string GetExpectation()
        => $"seek \"{this._Expected}\"";
}
