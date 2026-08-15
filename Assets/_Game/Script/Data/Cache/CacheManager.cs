using UnityEngine;
using System.Collections.Generic;

public static class CacheManager
{
    private static readonly Dictionary<int, List<Vector3>> levelSpawnPoints = new Dictionary<int, List<Vector3>>();

    public static List<Vector3> GetLevelSpawnPoints(int levelIndex)
    {
        if (levelSpawnPoints.TryGetValue(levelIndex, out List<Vector3> points))
        {
            return points;
        }
        return null;
    }

    public static void SetLevelSpawnPoints(int levelIndex, List<Vector3> points)
    {
        levelSpawnPoints[levelIndex] = points;
    }

    public static void ClearAll()
    {
        CacheComponent<Collider, Character>.Clear();
        levelSpawnPoints.Clear();
    }
}
