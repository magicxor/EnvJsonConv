namespace EnvJsonConv;

public sealed class Converter
{
    public void Convert(CommandLineOptions commandLineOptions)
    {
        ArgumentNullException.ThrowIfNull(commandLineOptions);

        if (commandLineOptions.Input.EndsWith(".json", StringComparison.OrdinalIgnoreCase)
            && commandLineOptions.Output.EndsWith(".env", StringComparison.OrdinalIgnoreCase))
        {
            JsonToEnvConverter.Convert(commandLineOptions.Input, commandLineOptions.Output, commandLineOptions.AddPrefix, commandLineOptions.RemovePrefix);
        }
        else if (commandLineOptions.Input.EndsWith(".env", StringComparison.OrdinalIgnoreCase)
                 && commandLineOptions.Output.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
        {
            EnvToJsonConverter.Convert(commandLineOptions.Input, commandLineOptions.Output, commandLineOptions.AddPrefix, commandLineOptions.RemovePrefix);
        }
        else
        {
            throw new ArgumentException("Invalid input/output file extensions. Supported combinations: .json/.env, .env/.json", nameof(commandLineOptions));
        }
    }
}
