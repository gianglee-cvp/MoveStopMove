using UnityEngine;

public static class CacheManager
{
    public static void ClearAll()
    {
        CacheComponent<Collider , Character>.Clear();
    }
}