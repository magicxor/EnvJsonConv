namespace EnvJsonConv;

public static class KeyTransformer
{
    private static string SafeSubstring(string? src, int startIndex)
    {
        ArgumentNullException.ThrowIfNull(src);

        if (startIndex < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(startIndex), $"{nameof(startIndex)} must be greater than or equal to 0");
        }

        return src.Length - 1 < startIndex
            ? string.Empty
            : src[startIndex..];
    }

    public static string TransformKey(string key, string? addPrefix, string? removePrefix)
    {
        ArgumentNullException.ThrowIfNull(key);

        if (!string.IsNullOrEmpty(removePrefix)
            && key.StartsWith(removePrefix, StringComparison.Ordinal))
        {
            key = SafeSubstring(key, removePrefix.Length);
        }

        if (!string.IsNullOrEmpty(addPrefix))
        {
            key = addPrefix + key;
        }

        return key;
    }
}
