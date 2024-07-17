using System.Text.Json;

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
                    t = JsonSerializer.Deserialize<T>(System.Text.Json.JsonSerializer.Serialize<T>(toClone));
            }
            catch { }
            return t;
        }

        public static string JSONStringify<T>( this T obj )
        {
            return JsonSerializer.Serialize(obj);
        }

        public static T Deserialize<T>( this string str )
        {
            return JsonSerializer.Deserialize<T>(str);
        }

    }
}
