using CommandLine;

namespace EnvJsonConv;

public sealed class CommandLineOptions
{
    [Option('i', "input", Required = true, HelpText = "Input file")]
    public required string Input { get; set; }

    [Option('o', "output", Required = true, HelpText = "Output file")]
    public required string Output { get; set; }

    [Option('a', "add-prefix", Required = false,  HelpText = "Prefix to be added")]
    public string? AddPrefix { get; set; }

    [Option('r', "remove-prefix", Required = false, HelpText = "Prefix to be removed")]
    public string? RemovePrefix { get; set; }
}
