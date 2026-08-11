using UnityEditor.Callbacks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
public class Knight : BulletBase
{
    public override void Throw(Vector3 des)
    {
        direction.y = 0;
        rb.linearVelocity = direction * speed;
    }
}