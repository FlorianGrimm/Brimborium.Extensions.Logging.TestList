namespace Brimborium.Extensions.Logging.TestList;

public class OccurredWithinAssertionTests {
    [Test]
    public async Task OccurredWithinTests() {

        // Arange

        ServiceProvider appServiceProvider;
        {
            Microsoft.Extensions.DependencyInjection.ServiceCollection services = new();
            _ = services.AddLogging((loggingBuilder) => {
                loggingBuilder.AddTestList();
            });
            appServiceProvider = services.BuildServiceProvider();
        }

        // Act

        {
            var logger = appServiceProvider.GetRequiredService<ILogger<FindNextAssertionTests>>();
            logger.Log(LogLevel.Information, "A");
            await Task.Delay(TimeSpan.FromMilliseconds(1), CancellationToken.None);
            logger.Log(LogLevel.Information, "N");
            await Task.Delay(TimeSpan.FromMilliseconds(2), CancellationToken.None);
            logger.Log(LogLevel.Information, "N");
            await Task.Delay(TimeSpan.FromMilliseconds(3), CancellationToken.None);
            logger.Log(LogLevel.Information, "A");
        }

        // Assert

        {
            var testLoggerBuffer = appServiceProvider.GetTestLoggerBuffer();
            var snapshot = testLoggerBuffer.GetSnapshot();
            _ = await Assert.That(snapshot).FindNext(new(Message: "A"));
            _ = await Assert.That(snapshot).FindNext(new(Message: "N")).And.OccurredWithin(TimeSpan.Zero, TimeSpan.FromMilliseconds(1));
            _ = await Assert.That(snapshot).FindNext(new(Message: "N")).And.OccurredWithin(TimeSpan.Zero, TimeSpan.FromMilliseconds(2));
            _ = await Assert.That(snapshot).FindNext(new(Message: "A")).And.OccurredWithin(TimeSpan.Zero, TimeSpan.FromMilliseconds(3));
        }
        appServiceProvider.Dispose();
    }
}