// MIT License Copyright (c) Florian Grimm

#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace TUnit.Assertions.Extensions;

public sealed class FindNextAssertion : Assertion<TestLoggerSnapshot> {
    private readonly TestLoggerItem _Expected;

    public FindNextAssertion(
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

        var success = value.TryFindNext(this._Expected, out _);

        if (success) {
            return Task.FromResult(AssertionResult.Passed);
        } else {
            return Task.FromResult(AssertionResult.Failed($"'{value}' does not contain '{this._Expected.ToStringMinimal()}'"));
        }
    }

    protected override string GetExpectation()
        => $"to contain \"{this._Expected.ToStringMinimal()}\"";
}
