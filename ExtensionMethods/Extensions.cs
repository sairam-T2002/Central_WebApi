using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Text;

namespace ExtensionMethods;

public static class Extensions
{
    public static T? DeepClone<T>( this T source )
    {
        if (source == null)
        {
            return default;
        }

        try
        {
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve,
                MaxDepth = 64
            };

            string serialized = JsonSerializer.Serialize(source, options);
            return JsonSerializer.Deserialize<T>(serialized, options);
        }
        catch (JsonException jsonEx)
        {
            throw new InvalidOperationException("Failed to serialize or deserialize the object during deep cloning.", jsonEx);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An unexpected error occurred during deep cloning.", ex);
        }
    }

    public static string JSONStringify<T>( this T obj )
    {
        string? temp = JsonSerializer.Serialize(obj);
        if (temp == null)
        {
            throw new InvalidOperationException($"Serialization failed for {typeof(T).Name}");
        }
        return temp;
    }

    public static T JSONParse<T>( this string str )
    {
        T? temp = JsonSerializer.Deserialize<T>(str);
        if (temp == null)
        {
            throw new InvalidOperationException($"Deserialization failed for {typeof(T).Name}");
        }
        return temp;
    }

    public static string CapitalizeFirstLetter( this string str )
    {
        if (string.IsNullOrWhiteSpace(str))
            return str;

        return $"{char.ToUpper(str[0], CultureInfo.CurrentCulture)}{str.Substring(1)}";
    }

    public static string ToTitleCase( this string str )
    {
        if (string.IsNullOrWhiteSpace(str))
            return str;

        TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
        return textInfo.ToTitleCase(str.ToLower());
    }

    public static string ToCamelCase( this string str )
    {
        if (string.IsNullOrWhiteSpace(str))
            return str;

        TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
        string titleCase = textInfo.ToTitleCase(str.ToLower());
        string camelCase = Regex.Replace(titleCase, @"\s+", "");

        if (camelCase.Length > 0)
        {
            camelCase = char.ToLower(camelCase[0]) + camelCase.Substring(1);
        }

        return camelCase;
    }

    public static bool IsEven( this int value )
    {
        return value % 2 == 0;
    }

    public static bool IsPrime( this int value )
    {
        if (value < 2)
            return false;
        if (value == 2)
            return true;
        if (value.IsEven())
            return false;

        int sqrt = (int)Math.Sqrt(value);
        for (int i = 3; i <= sqrt; i += 2)
        {
            if (value % i == 0)
                return false;
        }

        return true;
    }

    public static string Base64Encode( this string str ) 
    { 
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(str));
    }

    public static string Base64Decode(this string str )
    {
        return Encoding.UTF8.GetString(Convert.FromBase64String(str));
    }
}