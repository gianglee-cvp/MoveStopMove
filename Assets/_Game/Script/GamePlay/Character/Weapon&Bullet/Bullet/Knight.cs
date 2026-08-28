using UnityEngine;
public class Knight : BulletBase
{
    void Update()
    {
        if (isOnObstacle) return;
        TF.position = Vector3.MoveTowards(TF.position,target,speedScale * Time.deltaTime);
        if (CheckDisTance())
        {
            DeSpawn();
        }
    }
}