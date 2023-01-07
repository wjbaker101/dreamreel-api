namespace Core.Extensions;

public static class EnumerableExtensions
{
    public static List<TResult> ConvertAll<T, TResult>(this IEnumerable<T> enumerable, Func<T, TResult> func)
    {
        return enumerable.Select(func).ToList();
    }

    public static List<TResult> ConvertAll<T, TResult>(this IEnumerable<T> enumerable, Func<T, int, TResult> func)
    {
        return enumerable.Select(func).ToList();
    }
}