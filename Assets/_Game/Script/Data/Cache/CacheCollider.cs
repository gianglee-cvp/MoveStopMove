using System.Collections.Generic;
using System;
using UnityEngine;

public static class CacheComponent<TKey, TValue>
where TKey : Component
where TValue : Component
{
    private static readonly Dictionary<TKey, TValue> cache = new();

    public static void Add(TKey key, TValue value)
    {
        if (key == null || value == null) return;
        cache[key] = value;
    }

    public static void Remove(TKey key)
    {
        if (key == null) return;
        cache.Remove(key);
    }

    public static void Clear()
    {
        cache.Clear(); 
    }

    public static TValue Get(TKey key)
    {
        if (key == null) return null;

        if (!cache.ContainsKey(key) || cache[key] == null)
        {
            TValue value = key.GetComponent<TValue>();

            if (value != null)
            {
                cache[key] = value;
            }
        }

        return cache.ContainsKey(key) ? cache[key] : null;
    }
}