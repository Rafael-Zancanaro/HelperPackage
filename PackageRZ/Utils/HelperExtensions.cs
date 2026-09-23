using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reflection;

namespace PackageRZ.Utils;

/// <summary>
/// Provides utility extension methods for common data types.
/// </summary>
public static class HelperExtensions
{
    private static readonly ConcurrentDictionary<Enum, string> _enumCache = new();

    /// <summary>
    /// Gets the string description from the <see cref="DescriptionAttribute"/> of an Enum value.
    /// If no attribute is found, it returns the enum string representation.
    /// The results are cached for performance.
    /// </summary>
    /// <param name="value">The enum value.</param>
    /// <returns>The description string.</returns>
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
