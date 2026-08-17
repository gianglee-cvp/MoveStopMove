using UnityEngine;
using System.Collections.Generic;
public static class Helper
{
    public static Vector3 GetRandomSpawnPosition(float radius)
    {
        Vector2 offset2D = Random.insideUnitCircle * radius;
        return new Vector3(offset2D.x, 0f, offset2D.y);
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
}
