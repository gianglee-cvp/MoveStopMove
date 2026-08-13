using UnityEngine;
public class Knight : BulletBase
{
    public override void Throw()
    {
        direction.y = 0;
        rb.linearVelocity = direction * speed;
    }
    void Update()
    {
        if (CheckDisTance())
        {
            DeSpawn();
        }
    }
}