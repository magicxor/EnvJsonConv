using CommandLine;
using Microsoft.Extensions.DependencyInjection;

namespace EnvJsonConv;

internal static class Program
{
    // see: https://docs.microsoft.com/en-us/windows/desktop/Debug/system-error-codes
    private const int Success = 0;
    private const int ErrorBadArguments = 160;

    private static readonly CancellationTokenSource CancellationTokenSource = new();

    private static ServiceProvider CreateServiceProvider()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped<Converter>();
        var serviceProvider = serviceCollection.BuildServiceProvider();
        return serviceProvider;
    }

    private static async Task<int> OnParsedAsync(CommandLineOptions commandLineOptions)
    {
        Console.CancelKeyPress += (_, a) =>
        {
            CancellationTokenSource.Cancel();
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
        try
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
        finally
        {
            CancellationTokenSource.Dispose();
        }
    }
}
