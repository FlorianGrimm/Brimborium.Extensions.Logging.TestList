// MIT License Copyright (c) Florian Grimm

#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace TUnit.Assertions.Extensions;

public static class TestLogggerItemAssertionExtensions {
    public static FindNextAssertion FindNext(
        this IAssertionSource<TestLoggerSnapshot> source,
        TestLoggerItem expected,
        [CallerArgumentExpression(nameof(expected))] string? expression = null
        ) {
        _ = source.Context.ExpressionBuilder.Append($".FindNext({expression})");
        return new FindNextAssertion(source.Context, expected);
    }

    public static FindNextAssertion FindNextLoggerMessage(
       this IAssertionSource<TestLoggerSnapshot> source,
       Expression expected,
       [CallerArgumentExpression(nameof(expected))] string? expression = null
       ) {
        _ = source.Context.ExpressionBuilder.Append($".FindNext({expression})");
        TestLoggerItem testLoggerItemExpected = LoggerMessageExtensions.ExtractFromExpression(expected);
        return new FindNextAssertion(source.Context, testLoggerItemExpected);
    }

    public static FindNextAssertion FindNextLoggerMessage<T>(
       this IAssertionSource<TestLoggerSnapshot> source,
       T expected,
       [CallerArgumentExpression(nameof(expected))] string? expression = null
       ) where T : System.Delegate {
        _ = source.Context.ExpressionBuilder.Append($".FindNextLoggerMessage({expression})");
        TestLoggerItem testLoggerItemExpected = LoggerMessageExtensions.ExtractFromDelegate(expected);
        return new FindNextAssertion(source.Context, testLoggerItemExpected);
    }

    public static OccurredWithinAssertion OccurredWithin(
        this IAssertionSource<TestLoggerSnapshot> source,
        TimeSpan expectedHigh,
        [CallerArgumentExpression(nameof(expectedHigh))] string? expressionHigh = null
        ) {
        _ = source.Context.ExpressionBuilder.Append($".OccurredWithin({expressionHigh})");
        return new OccurredWithinAssertion(source.Context, TimeSpan.Zero, expectedHigh);
    }

    public static OccurredWithinAssertion OccurredWithin(
        this IAssertionSource<TestLoggerSnapshot> source,
        TimeSpan expectedLow,
        TimeSpan expectedHigh,
        [CallerArgumentExpression(nameof(expectedLow))] string? expressionLow = null,
        [CallerArgumentExpression(nameof(expectedHigh))] string? expressionHigh = null
        ) {
        _ = source.Context.ExpressionBuilder.Append($".OccurredWithin({expressionLow}, {expressionHigh})");
        return new OccurredWithinAssertion(source.Context, expectedLow, expectedHigh);
    }

    public static NextAssertion Next(
        this IAssertionSource<TestLoggerSnapshot> source,
        int expected = 1,
        [CallerArgumentExpression(nameof(expected))] string? expression = null
        ) {
        _ = source.Context.ExpressionBuilder.Append($".Next({expression})");
        return new NextAssertion(source.Context, expected);
    }

    public static MatchesAssertion Matches(
        this IAssertionSource<TestLoggerSnapshot> source,
        TestLoggerItem expected,
        [CallerArgumentExpression(nameof(expected))] string? expression = null
        ) {
        _ = source.Context.ExpressionBuilder.Append($".Matches({expression})");
        return new MatchesAssertion(source.Context, expected);
    }

    public static MatchesAssertion MatchesLoggerMessage(
        this IAssertionSource<TestLoggerSnapshot> source,
        Expression expected,
        [CallerArgumentExpression(nameof(expected))] string? expression = null
        ) {
        _ = source.Context.ExpressionBuilder.Append($".Matches({expression})");
        TestLoggerItem testLoggerItemExpected = LoggerMessageExtensions.ExtractFromExpression(expected);
        return new MatchesAssertion(source.Context, testLoggerItemExpected);
    }

    public static MatchesParameterAssertion MatchesParameter(
        this IAssertionSource<TestLoggerSnapshot> source,
        Dictionary<string, object?> expected,
        [CallerArgumentExpression(nameof(expected))] string? expression = null
        ) {
        _ = source.Context.ExpressionBuilder.Append($".Matches({expression})");
        return new MatchesParameterAssertion(source.Context, expected);
    }
}
