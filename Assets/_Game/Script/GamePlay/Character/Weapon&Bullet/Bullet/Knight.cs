using UnityEngine;
public class Knight : BulletBase
{
    void Update()
    {
        TF.position = Vector3.MoveTowards(TF.position,target,speed * Time.deltaTime);
        if (CheckDisTance())
        {
            DeSpawn();
        }
    }
}