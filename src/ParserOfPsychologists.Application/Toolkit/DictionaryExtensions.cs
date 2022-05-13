namespace ParserOfPsychologists.Application.Toolkit;

public static class DictionaryExtensions
{
    public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> source, IDictionary<TKey, TValue> provider)
    {
        foreach (var kv in provider)
            source.Add(kv.Key, kv.Value);
    }
}