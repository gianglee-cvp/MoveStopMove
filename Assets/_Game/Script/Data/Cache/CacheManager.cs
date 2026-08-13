using UnityEngine;

public static class CacheManager
{
    public static void ClearAll()
    {
        //TODO Physic ignore attackrange chỉ có va chạm với 
        CacheComponent<Collider , Character>.Clear();
    }
}