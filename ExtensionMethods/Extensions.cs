using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ExtensionMethods
{
    public static class Extensions
    {
        public static T DeepClone<T>( this T toClone )
        {
            T t = default(T);
            try
            {
                if (toClone != null)
                    t = JsonSerializer.Deserialize<T>(JsonSerializer.Serialize<T>(toClone));
            }
            catch { }
            return t;
        }

        public static string JSONStringify<T>( this T obj )
        {
            string? temp = JsonSerializer.Serialize(obj);
            if(temp == null)
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

            return $"{char.ToUpper(str[0], System.Globalization.CultureInfo.CurrentCulture)}{str.Substring(1)}";
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

        public static bool IsEven(this int value )
        {
            return value % 2 == 0;
        }

        public static bool IsPrime(this int value)
        {
            if (value < 2)
                return false;
            if(value == 2)
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
    }
}
