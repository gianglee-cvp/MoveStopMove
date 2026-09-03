using UnityEngine;
using System.Collections.Generic;

public static class CacheManager
{
    private static readonly Dictionary<int, List<Vector3>> levelSpawnPoints = new Dictionary<int, List<Vector3>>();
    private static Vector3[] navMeshVertices;
    private static int[] navMeshIndices;
    private static float[] navMeshCumulativeAreas;
    private static float totalNavMeshArea;

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

    public static bool TryGetNavMeshCache(out Vector3[] vertices, out int[] indices, out float[] cumulativeAreas, out float totalArea)
    {
        vertices = navMeshVertices;
        indices = navMeshIndices;
        cumulativeAreas = navMeshCumulativeAreas;
        totalArea = totalNavMeshArea;

        return vertices != null && indices != null && indices.Length >= 3 && cumulativeAreas != null && cumulativeAreas.Length > 0 && totalArea > 0f;
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

        int triangleCount = indices.Length / 3;
        navMeshCumulativeAreas = new float[triangleCount];
        totalNavMeshArea = 0f;

        for (int i = 0; i < triangleCount; i++)
        {
            Vector3 a = vertices[indices[i * 3]];
            Vector3 b = vertices[indices[i * 3 + 1]];
            Vector3 c = vertices[indices[i * 3 + 2]];

            float area = Vector3.Cross(b - a, c - a).magnitude * 0.5f;
            totalNavMeshArea += area;
            navMeshCumulativeAreas[i] = totalNavMeshArea;
        }

        return true;
    }

    public static void ClearNavMeshCache()
    {
        navMeshVertices = null;
        navMeshIndices = null;
        navMeshCumulativeAreas = null;
        totalNavMeshArea = 0f;
    }

    public static void ClearAll()
    {
        CacheComponent<Collider, Character>.Clear();
        levelSpawnPoints.Clear();
        ClearNavMeshCache();
    }
}
