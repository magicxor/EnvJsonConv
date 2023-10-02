using Newtonsoft.Json;

namespace EnvJsonConv;

public static class EnvToJsonConverter
{
    public static void Convert(string inputEnvPath, string outputJsonPath, string? addPrefix, string? removePrefix)
    {
        var dictionary = new Dictionary<string, string>();
        var lines = File.ReadLines(inputEnvPath);

        foreach (var line in lines)
        {
            var parts = line.Split('=', 2, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 2)
            {
                var key = KeyTransformer.TransformKey(parts[0], addPrefix, removePrefix);
                var value = parts[1];
                dictionary.Add(key, value);
            }
        }

        File.WriteAllText(outputJsonPath, JsonConvert.SerializeObject(dictionary, Formatting.Indented));
    }
}
