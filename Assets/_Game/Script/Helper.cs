using UnityEngine;
public static class Helper
{
    //TODO cho vafo ulti 
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
        return(Distance2D(pos,des) < range * range) ? false : true;
    }
    
}