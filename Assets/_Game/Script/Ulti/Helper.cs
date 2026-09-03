using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public static class Helper
{
    public static Vector3 GetRandomSpawnPosition(float radius)
    {
        for (int i = 0; i < 8; i++)
        {
            Vector2 offset2D = Random.insideUnitCircle * radius;
            Vector3 randomPoint = new Vector3(offset2D.x, 0f, offset2D.y);

            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, radius, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }

        return Vector3.zero;
    }
    public static T RandomEnumValue<T>() where T : System.Enum
    {
        var values = System.Enum.GetValues(typeof(T));
        return (T)values.GetValue(
            UnityEngine.Random.Range(0, values.Length)
        );
    }

    public static float Distance2D(Vector3 pos , Vector3 des)
    {
        Vector2 pos2D =  new Vector2(pos.x , pos.z);
        Vector2 des2D  = new Vector2(des.x , des.z);
        // Debug.Log(Vector2.Distance(des2D,pos2D));
        return (des2D - pos2D).sqrMagnitude;
    }
    public static bool CheckDistanceOutRange(Vector3 pos , Vector3 des , float range)
    {
        // Debug.Log(range);
        return !(Distance2D(pos,des) < range * range + 1);
    }
    public static Vector3 CopyPositionXZ(Vector3 source, Vector3 target)
    {
        return new Vector3(source.x, target.y, source.z);
    }

    public static Vector3 GetRandomPointOnNavMesh()
    {
        if (!EnsureNavMeshCache(out Vector3[] vertices, out int[] indices, out float[] cumulativeAreas, out float totalArea))
        {
            return Vector3.zero;
        }

        float randomArea = Random.Range(0f, totalArea);
        int triangleIndex = 0;
        for (int i = 0; i < cumulativeAreas.Length; i++)
        {
            if (randomArea <= cumulativeAreas[i])
            {
                triangleIndex = i;
                break;
            }
        }

        int indexPtr = triangleIndex * 3;
        Vector3 a = vertices[indices[indexPtr]];
        Vector3 b = vertices[indices[indexPtr + 1]];
        Vector3 c = vertices[indices[indexPtr + 2]];

        float r1 = Random.value;
        float r2 = Random.value;

        if (r1 + r2 > 1f)
        {
            r1 = 1f - r1;
            r2 = 1f - r2;
        }

        return a + r1 * (b - a) + r2 * (c - a);
    }

    private static bool EnsureNavMeshCache(out Vector3[] vertices, out int[] indices, out float[] cumulativeAreas, out float totalArea)
    {
        if (CacheManager.TryGetNavMeshCache(out vertices, out indices, out cumulativeAreas, out totalArea))
        {
            return true;
        }

        NavMeshTriangulation triangulation = NavMesh.CalculateTriangulation();
        if (!CacheManager.SetNavMeshCache(triangulation.vertices, triangulation.indices))
        {
            Debug.LogWarning("[Helper] Khong tim thay du lieu NavMesh!");
            vertices = null;
            indices = null;
            cumulativeAreas = null;
            totalArea = 0f;
            return false;
        }

        return CacheManager.TryGetNavMeshCache(out vertices, out indices, out cumulativeAreas, out totalArea);
    }
}
