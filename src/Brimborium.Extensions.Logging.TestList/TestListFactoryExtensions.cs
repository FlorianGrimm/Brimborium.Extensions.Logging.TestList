// MIT License Copyright (c) Florian Grimm

namespace Microsoft.Extensions.Logging;

/// <summary>
/// Extension methods for the <see cref="ILoggerFactory"/> class.
/// </summary>
public static class TestListLoggerFactoryExtensions {
    /// <summary>
    /// Adds a unit test logger named 'TestList' to the factory.
    /// </summary>
    /// <param name="builder">The extension method argument.</param>
    public static ILoggingBuilder AddTestList(this ILoggingBuilder builder) {
        builder.Services.TryAddSingleton<TestLoggerBuffer>();
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, TestListLoggerProvider>());

        return builder;
    }

    /// <summary>
    /// Get the required service <see cref="TestLoggerBuffer"/>.
    /// </summary>
    /// <param name="serviceProvider">the serviceProvider</param>
    /// <returns>the singleton</returns>
    public static TestLoggerBuffer GetTestLoggerBuffer(this IServiceProvider serviceProvider)
        => serviceProvider.GetRequiredService<TestLoggerBuffer>();
}

