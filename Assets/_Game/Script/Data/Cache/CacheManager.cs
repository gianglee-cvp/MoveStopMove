using UnityEngine;
using System.Collections.Generic;

public static class CacheManager
{
    private static readonly Dictionary<int, List<Vector3>> levelSpawnPoints = new Dictionary<int, List<Vector3>>();
    private static Vector3[] navMeshVertices;
    private static int[] navMeshIndices;

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

    public static bool TryGetNavMeshCache(out Vector3[] vertices, out int[] indices)
    {
        vertices = navMeshVertices;
        indices = navMeshIndices;

        return vertices != null && indices != null && indices.Length >= 3;
    }

    public static bool SetNavMeshCache(Vector3[] vertices, int[] indices)
    {
        if (vertices == null || indices == null || indices.Length < 3)
        {
            ClearNavMeshCache();
            return false;
        }

        navMeshVertices = vertices;
        navMeshIndices = indices;
        return true;
    }

    public static void ClearNavMeshCache()
    {
        navMeshVertices = null;
        navMeshIndices = null;
    }

    public static void ClearAll()
    {
        CacheComponent<Collider, Character>.Clear();
        levelSpawnPoints.Clear();
        ClearNavMeshCache();
    }
}
