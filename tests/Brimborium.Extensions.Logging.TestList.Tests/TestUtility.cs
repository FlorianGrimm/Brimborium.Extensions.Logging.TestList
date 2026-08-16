namespace Brimborium.Extensions.Logging.TestList;

public sealed class TestUtility {
    public static (ServiceProvider appServiceProvider, TestLoggerSnapshot snapshot) CreateANNA() {
        ServiceProvider appServiceProvider;
        {
            Microsoft.Extensions.DependencyInjection.ServiceCollection services = new();
            _ = services.AddLogging((loggingBuilder) => {
                loggingBuilder.AddTestList();
            });
            appServiceProvider = services.BuildServiceProvider();
        }

        // Act

        var logger = appServiceProvider.GetRequiredService<ILogger<TestUtility>>();
        logger.Log(LogLevel.Information, "A");
        logger.Log(LogLevel.Information, "N");
        logger.Log(LogLevel.Information, "N");
        logger.Log(LogLevel.Information, "A");

        var testLoggerBuffer = appServiceProvider.GetTestLoggerBuffer();
        var snapshot = testLoggerBuffer.GetSnapshot();

        return (appServiceProvider, snapshot);
    }
}
