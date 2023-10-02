using System.Text;
using IniParser;
using IniParser.Model;
using IniParser.Model.Configuration;
using Newtonsoft.Json.Linq;

namespace EnvJsonConv;

public static class JsonToEnvConverter
{
    private static readonly Encoding Utf8WithoutBom = new UTF8Encoding(false);

    private static void SetupIniParserConfiguration(IniParserConfiguration iniParserConfiguration)
    {
        iniParserConfiguration.AssigmentSpacer = string.Empty;
        iniParserConfiguration.AllowDuplicateKeys = false;
    }

    private static void ConvertJObjectToIni(JToken token, IniData data, string fullKeyName, string? addPrefix, string? removePrefix)
    {
        switch (token)
        {
            case JValue { Value: not null } jValue:
            {
                var key = KeyTransformer.TransformKey(fullKeyName.TrimEnd('_'), addPrefix, removePrefix);
                data.Global[key] = jValue.Value.ToString();
                break;
            }

            case JObject jObject:
            {
                foreach (var kvp in jObject)
                {
                    if (kvp.Value != null)
                    {
                        var newPrefix = fullKeyName + kvp.Key + "__";
                        ConvertJObjectToIni(kvp.Value, data, newPrefix, addPrefix, removePrefix);
                    }
                }

                break;
            }

            case JArray jArray:
            {
                for (var i = 0; i < jArray.Count; i++)
                {
                    var newPrefix = fullKeyName + i + "__";
                    ConvertJObjectToIni(jArray[i], data, newPrefix, addPrefix, removePrefix);
                }

                break;
            }
        }
    }

    public static void Convert(string inputJsonPath, string outputEnvPath, string? addPrefix, string? removePrefix)
    {
        ArgumentException.ThrowIfNullOrEmpty(inputJsonPath);
        ArgumentException.ThrowIfNullOrEmpty(outputEnvPath);

        var jsonString = File.ReadAllText(inputJsonPath);
        var jsonObject = JObject.Parse(jsonString);

        var data = new IniData();
        SetupIniParserConfiguration(data.Configuration);
        ConvertJObjectToIni(jsonObject, data, string.Empty, addPrefix, removePrefix);

        var parser = new FileIniDataParser();
        SetupIniParserConfiguration(parser.Parser.Configuration);
        parser.WriteFile(outputEnvPath, data, Utf8WithoutBom);
    }
}
