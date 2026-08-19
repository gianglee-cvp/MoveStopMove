using Unity.VisualScripting;
using UnityEngine;

public enum BulletType
{
    Knight = 0,
    Hammer = 1,
    Boomerang = 2
}

public class BulletBase : GameUnit
{
    protected virtual BulletType Type => BulletType.Knight;
    [SerializeField] protected Character owner;
    [SerializeField] protected float speed = 10f;
    protected float size;
    protected float speedScale => size * speed;
    [SerializeField] protected float range;
    protected Vector3 target;
    protected Vector3 rootPos;

    public virtual void Init(Vector3 startDir, float rangeAttack, Vector3 rootPosion, Character ch)
    {
        TF.localScale = ch.Scale;
        size = ch.Size;
        range = rangeAttack;
        target = Helper.CopyPositionXZ(TF.position + startDir * rangeAttack, TF.position);
        rootPos = rootPosion;
        owner = ch;
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        Character target = CacheComponent<Collider, Character>.Get(other);
        if (owner == null || target == null || target == owner || target.IsDead) return;

        CollectExp(target);
        target.OnDead();
        DeSpawn();
    }

    public virtual void Throw()
    {
    }

    public bool CheckDisTance()
    {
        float offset = Helper.Distance2D(TF.position, rootPos);
        return offset > range * range;
    }

    public void DeSpawn()
    {
        SimplePool.DeSpawn(this);
    }

    public void CollectExp(Character ch)
    {
        if (owner == null || !owner.isActiveAndEnabled) return;
        owner.CollectExp(ch.Level);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(TF.position, 0.15f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(rootPos, 0.15f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(TF.position, rootPos);
    }
}
