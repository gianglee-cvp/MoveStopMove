using NUnit.Framework;
using UnityEngine;
public class Hammer : BulletBase
{
    [SerializeField] float rotationSpeed = 500f;
    void Update()
    {
        TF.position = Vector3.MoveTowards(TF.position,target,speed * Time.deltaTime);
        TF.Rotate(Vector3.down * rotationSpeed * Time.deltaTime);
        if (CheckDisTance())
        {
            DeSpawn();
        }
    }
}