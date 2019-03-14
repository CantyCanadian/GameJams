using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DictionaryExtention
{
    /// <summary>
    /// Appends an entire dictionary to another.
    /// </summary>
    /// <typeparam name="K">Key type.</typeparam>
    /// <typeparam name="V">Value type.</typeparam>
    /// <param name="target">Object that is extended.</param>
    /// <param name="origin">Dictionary that gets appended onto extended object.</param>
    public static void Append<K, V>(this Dictionary<K, V> target, Dictionary<K, V> origin)
    {
        if (origin == null || target == null)
        {
            throw new System.ArgumentNullException("Collection is null");
        }

        foreach (var item in origin)
        {
            if (!target.ContainsKey(item.Key))
            {
                target.Add(item.Key, item.Value);
            }
        }
    }

    public static void Add<K, V>(this Dictionary<K, V> target, KeyValuePair<K, V> keyValue)
    {
        target.Add(keyValue.Key, keyValue.Value);
    }

    public static List<K> ExtractKeys<K, V>(this Dictionary<K, V> target)
    {
        return new List<K>(target.Keys);
    }

    public static List<V> ExtractValues<K, V>(this Dictionary<K, V> target)
    {
        return new List<V>(target.Values);
    }
}