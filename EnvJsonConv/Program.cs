using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace EnvJsonConv;

internal static class Program
{
    // see: https://docs.microsoft.com/en-us/windows/desktop/Debug/system-error-codes
    private const int Success = 0;
    private const int ErrorBadArguments = 160;

    private static ServiceProvider CreateServiceProvider()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped<Converter>();
        var serviceProvider = serviceCollection.BuildServiceProvider();
        return serviceProvider;
    }

    [SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "CancellationTokenSource is disposed in the end of the method")]
    private static async Task<int> OnParsedAsync(CommandLineOptions commandLineOptions)
    {
        var cancelTokenSource = new CancellationTokenSource();
        Console.CancelKeyPress += (_, a) =>
        {
            cancelTokenSource.Cancel();
            a.Cancel = true;
        };

        await using var serviceProvider = CreateServiceProvider();
        var messageBrokerService = serviceProvider.GetRequiredService<Converter>();
        messageBrokerService.Convert(commandLineOptions);

        return Success;
    }

    private static Task<int> OnNotParsedAsync(IEnumerable<Error> errors)
    {
        return Task.FromResult(ErrorBadArguments);
    }

    private static async Task<int> Main(string[] args)
    {
        using var parser = new Parser(settings =>
        {
            settings.CaseSensitive = true;
            settings.IgnoreUnknownArguments = false;
            settings.HelpWriter = Console.Out;
        });
        var exitCode = await parser.ParseArguments<CommandLineOptions>(args)
            .MapResult(OnParsedAsync, OnNotParsedAsync);
        return exitCode;
    }
}
