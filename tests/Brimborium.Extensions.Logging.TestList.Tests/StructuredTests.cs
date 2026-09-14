namespace Brimborium.Extensions.Logging.TestList;

public class StructuredTests {
    [Test]
    public async Task FindNextTest001() {
        ServiceProvider appServiceProvider;
        {
            Microsoft.Extensions.DependencyInjection.ServiceCollection services = new();
            _ = services.AddLogging((loggingBuilder) => {
                loggingBuilder.AddTestList();
            });
            appServiceProvider = services.BuildServiceProvider();
        }

        // Act
        var testLoggerBuffer = appServiceProvider.GetTestLoggerBuffer();
        testLoggerBuffer.Clear();

        var logger = appServiceProvider.GetRequiredService<ILogger<StructuredTests>>();

        for (int number = 0; number < 10; number++) {
            logger.LogTestOne(number);
        }

        var snapshot = testLoggerBuffer.GetSnapshot();
        _ = await Assert.That(snapshot).FindNext(new(EventId: new EventId(1)));
        _ = await Assert.That(snapshot).Matches(new(State: new Dictionary<string, object?> { { "number", 0 } }));

        _ = await Assert.That(snapshot).FindNext(new(State: new Dictionary<string, object?> { { "number", 9 } }));
    }

    [Test]
    public async Task FindNextLoggerMessageTest001() {
        ServiceProvider appServiceProvider;
        {
            Microsoft.Extensions.DependencyInjection.ServiceCollection services = new();
            _ = services.AddLogging((loggingBuilder) => {
                loggingBuilder.AddTestList();
            });
            appServiceProvider = services.BuildServiceProvider();
        }

        // Act
        var testLoggerBuffer = appServiceProvider.GetTestLoggerBuffer();
        testLoggerBuffer.Clear();

        var logger = appServiceProvider.GetRequiredService<ILogger<StructuredTests>>();

        logger.LogTestA(43);

        var snapshot = testLoggerBuffer.GetSnapshot();

        _ = await Assert.That(snapshot).FindNextLoggerMessage(() => LoggerExtension.LogTestA);
    }

    [Test]
    public async Task FindNextLoggerMessageTest002() {
        ServiceProvider appServiceProvider;
        {
            Microsoft.Extensions.DependencyInjection.ServiceCollection services = new();
            _ = services.AddLogging((loggingBuilder) => {
                loggingBuilder.AddTestList();
            });
            appServiceProvider = services.BuildServiceProvider();
        }

        // Act
        var testLoggerBuffer = appServiceProvider.GetTestLoggerBuffer();
        testLoggerBuffer.Clear();

        var logger = appServiceProvider.GetRequiredService<ILogger<StructuredTests>>();

        logger.LogTestOne(42);
        logger.LogTestA(43);
        logger.LogTestB(44);
        logger.LogTestB(45);
        logger.LogTestA(46);
        logger.LogTestOne(47);

        var snapshot = testLoggerBuffer.GetSnapshot();

        _ = await Assert.That(snapshot).FindNextLoggerMessage(() => LoggerExtension.LogTestOne)
            .And.MatchesParameter(new Dictionary<string, object?> { { "number", 42 } });
        _ = await Assert.That(snapshot).FindNextLoggerMessage(() => LoggerExtension.LogTestA);
        _ = await Assert.That(snapshot).FindNextLoggerMessage(() => LoggerExtension.LogTestB);
        _ = await Assert.That(snapshot).FindNextLoggerMessage(() => LoggerExtension.LogTestB);
        _ = await Assert.That(snapshot).FindNextLoggerMessage(() => LoggerExtension.LogTestA);
        _ = await Assert.That(snapshot).FindNextLoggerMessage(() => LoggerExtension.LogTestOne);
    }

    [Test]
    public async Task FindNextLoggerMessageDelegateTest003() {
        ServiceProvider appServiceProvider;
        {
            Microsoft.Extensions.DependencyInjection.ServiceCollection services = new();
            _ = services.AddLogging((loggingBuilder) => {
                loggingBuilder.AddTestList();
            });
            appServiceProvider = services.BuildServiceProvider();
        }

        // Act
        var testLoggerBuffer = appServiceProvider.GetTestLoggerBuffer();
        testLoggerBuffer.Clear();

        var logger = appServiceProvider.GetRequiredService<ILogger<StructuredTests>>();

        logger.LogTestOne(42);
        logger.LogTestA(43);
        logger.LogTestB(44);
        logger.LogTestB(45);
        logger.LogTestA(46);
        logger.LogTestOne(47);

        var snapshot = testLoggerBuffer.GetSnapshot();
        _ = await Assert.That(snapshot).FindNextLoggerMessage(LoggerExtension.LogTestOne);
        //_ = await Assert.That(snapshot).FindNextLoggerMessage(() => LoggerExtension.LogTestOne)
        //    .And.MatchesParameter(new Dictionary<string, object?> { { "number", 42 } });
        //_ = await Assert.That(snapshot).FindNextLoggerMessage(() => LoggerExtension.LogTestA);
        //_ = await Assert.That(snapshot).FindNextLoggerMessage(() => LoggerExtension.LogTestB);
        //_ = await Assert.That(snapshot).FindNextLoggerMessage(() => LoggerExtension.LogTestB);
        //_ = await Assert.That(snapshot).FindNextLoggerMessage(() => LoggerExtension.LogTestA);
        //_ = await Assert.That(snapshot).FindNextLoggerMessage(() => LoggerExtension.LogTestOne);
    }

    [Test]
    public async Task MatchesParameter_failed_Test001() {
        ServiceProvider appServiceProvider;
        {
            Microsoft.Extensions.DependencyInjection.ServiceCollection services = new();
            _ = services.AddLogging((loggingBuilder) => {
                loggingBuilder.AddTestList();
            });
            appServiceProvider = services.BuildServiceProvider();
        }

        // Act
        var testLoggerBuffer = appServiceProvider.GetTestLoggerBuffer();
        testLoggerBuffer.Clear();

        var logger = appServiceProvider.GetRequiredService<ILogger<StructuredTests>>();

        logger.LogTestOne(42);

        var snapshot = testLoggerBuffer.GetSnapshot();
        try {
            _ = await Assert.That(snapshot).FindNextLoggerMessage(() => LoggerExtension.LogTestOne)
                .And.MatchesParameter(new Dictionary<string, object?> { { "number", 1 } });
            throw new Exception("Failed");
        } catch (TUnit.Assertions.Exceptions.AssertionException ex) {
            _ = await Assert.That(ex.Message).Contains("and to match { number }");
            _ = await Assert.That(ex.Message).Contains("but { 'number': 42 } does not match { 'number': 1 }'");
        }
    }
}

public static partial class LoggerExtension {
    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Information,
        Message = "TestOne {number}"
        )]
    public static partial void LogTestOne(this ILogger<StructuredTests> logger, int number);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "TestA {number}"
        )]
    public static partial void LogTestA(this ILogger<StructuredTests> logger, int number);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "TestB {number}"
        )]
    public static partial void LogTestB(this ILogger<StructuredTests> logger, int number);
}