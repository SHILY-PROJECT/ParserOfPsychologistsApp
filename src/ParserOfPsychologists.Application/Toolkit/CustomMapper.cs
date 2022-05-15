namespace ParserOfPsychologists.Application.Parser;

public class CustomMapper
{
    public static void Map<TSource, TProvider>(TSource? sourceModel, TProvider? providerModel)
    {
        if (sourceModel is null) throw new ArgumentNullException(nameof(sourceModel));
        if (providerModel is null) throw new ArgumentNullException(nameof(providerModel));

        foreach (var prop in typeof(TProvider).GetProperties())
        {
            var value = prop.GetValue(providerModel);

            if (value != null && (value.GetType() != typeof(string) || !string.IsNullOrWhiteSpace(value as string)))
            {
                typeof(TSource)?.GetProperty(prop.Name)?.SetValue(sourceModel, value);
            }
        }
    }
}