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

    }
}
