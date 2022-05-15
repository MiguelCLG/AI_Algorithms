using Newtonsoft.Json;
namespace System
{
    public static partial class Extensions
    {
        public static T Next<T>(this T src) where T : struct
        {
            if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));

            T[] Arr = (T[])Enum.GetValues(src.GetType());
            int j = Array.IndexOf<T>(Arr, src) + 1;
            return (Arr.Length==j) ? Arr[0] : Arr[j];            
        }
        public static T Clone<T>(this T source)
        {
            var serialized =  JsonConvert.SerializeObject(source, Formatting.None,
                        new JsonSerializerSettings()
                        { 
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });

            var deserialized =JsonConvert.DeserializeObject<T>(serialized);

            return deserialized;
        }
    }
    public static class ObjectExtensions
    {
        public static T Copy<T>(this T original)
        {
            return (T)Copy((Object)original);
        }
    }

}