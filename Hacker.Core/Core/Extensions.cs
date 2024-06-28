namespace Hacker.Core.Core;

public static class Extensions
{
    public static bool IsEqual(this string value1, string value2)
    {
        return string.Equals(value1, value2, StringComparison.OrdinalIgnoreCase);
    }

    public static string Join(this IEnumerable<string> list, string separator = " ")
    {
        return list.IsEmpty() ? string.Empty : string.Join(separator, list);
    }

    public static bool IsEmpty<T>(this IEnumerable<T> list)
    {
        return list == null || !list.Any();
    }

    public static string ReplaceArrayBraket(this string value)
    {
        value = value.Replace("[", string.Empty);
        value = value.Replace("]", string.Empty);
        return value;
    }

    public static string ReplaceTokens(this string value, IDictionary<string, object>? tokens)
    {
        if (string.IsNullOrWhiteSpace(value) || (tokens?.Count ?? 0) <= 0) return value;

        var result = value;
        tokens.ForEach((k, v) => { result = result.Replace($"{{{k}}}", v?.ToString() ?? string.Empty, StringComparison.OrdinalIgnoreCase); });
        return result;
    }

    public static IDictionary<K, V>? ForEach<K, V>(this IDictionary<K, V>? dictionary, Action<K, V> action)
    {
        if (dictionary == null) return dictionary;

        foreach (var i in dictionary)
        {
            action(i.Key, i.Value);
        }

        return dictionary;
    }
}
