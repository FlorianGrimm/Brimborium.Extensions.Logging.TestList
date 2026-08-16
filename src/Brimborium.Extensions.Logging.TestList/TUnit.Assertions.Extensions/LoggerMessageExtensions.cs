// MIT License Copyright (c) Florian Grimm

#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace TUnit.Assertions.Extensions;

public static class LoggerMessageExtensions {
    public static TestLoggerItem ExtractFromExpression(Expression expected) {
        if (expected is LambdaExpression lambdaExpression
            && lambdaExpression.Body is { } lambdaBody) {
        } else {
            throw new ArgumentException("lambda", nameof(expected));
        }
        if (lambdaBody.NodeType == ExpressionType.Convert
            && lambdaBody is UnaryExpression bodyConvert
            && bodyConvert.Operand.NodeType == ExpressionType.Call
            && bodyConvert.Operand is System.Linq.Expressions.MethodCallExpression convertOperand
            ) {
        } else {
            throw new ArgumentException("call", nameof(expected));
        }
        if (convertOperand.Object is System.Linq.Expressions.ConstantExpression operandObject
            && operandObject.Value is System.Reflection.MethodInfo objectValue
            && objectValue?.GetCustomAttribute<LoggerMessageAttribute>() is { } loggerMessageAttribute
            ) {
        } else {
            throw new ArgumentException("call", nameof(expected));
        }

        {
            var eventName = loggerMessageAttribute.EventName;
            if (string.IsNullOrEmpty(eventName)) {
                eventName = objectValue.Name;
            }
            var eventId = loggerMessageAttribute.EventId;
            if (eventId <= 0) {
                eventId = GetNonRandomizedHashCode(eventName);
            }
            TestLoggerItem testLoggerItem = new TestLoggerItem(
                EventId: new(eventId, eventName),
                LogLevel: loggerMessageAttribute.Level,
                OriginalFormat: loggerMessageAttribute.Message
                );
            return testLoggerItem;
        }
    }


    /// <summary>
    /// Returns a non-randomized hash code for the given string.
    /// We always return a positive value.
    /// </summary>
    public static int GetNonRandomizedHashCode(string s) {
        uint uhash = 2166136261u;
        foreach (char c in s) {
            uhash = (c ^ uhash) * 16777619;
        }

        int ihash = (int)uhash;
        var result = (ihash == int.MinValue) 
            ? 0 
            : Math.Abs(ihash); // Ensure the result is non-negative
        return result;
    }
}
