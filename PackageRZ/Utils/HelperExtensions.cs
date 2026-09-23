using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reflection;

namespace PackageRZ.Utils;

public static class HelperExtensions
{
    private static readonly ConcurrentDictionary<Enum, string> _enumCache = new();

    public static string GetEnumDescription(this Enum value)
    {
        if (value == null)
            return string.Empty;

        return _enumCache.GetOrAdd(value, (val) =>
        {
            var enumName = val.ToString();
            var fi = val.GetType().GetField(enumName);
            var attribute = fi?.GetCustomAttribute<DescriptionAttribute>(false);
            return attribute?.Description ?? enumName;
        });
    }
}
