using System.Collections.Generic;

namespace SharpFileSystem
{
    public static class Extensions
    {
        public static void Add<TKey, TValue>(ICollection<KeyValuePair<TKey, TValue>> collection, TKey key, TValue value)
        {
            collection.Add(new KeyValuePair<TKey, TValue>(key, value));
        }
    }
}
