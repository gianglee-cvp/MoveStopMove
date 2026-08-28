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
    [SerializeField] protected Collider col;
    protected Vector3 target;
    protected Vector3 rootPos;
    protected bool isOnObstacle;

    public virtual void Init(Vector3 startDir, float rangeAttack, Vector3 rootPosion, Character ch)
    {
        TF.localScale = ch.Scale;
        size = ch.Size;
        range = rangeAttack;
        target = Helper.CopyPositionXZ(TF.position + startDir * rangeAttack, TF.position);
        rootPos = rootPosion;
        owner = ch;
        isOnObstacle = false;
        col.enabled = true;
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        Debug.Log("check name " + other.gameObject.name);
        Character target = CacheComponent<Collider, Character>.Get(other);
        if(target != null && target != owner)
        {
            if(owner == null || target.IsDead ) return;
            CollectExp(target);
            target.OnDead();
            DeSpawn();
            return;
        }
        Obstacle obstacle = CacheComponent<Collider , Obstacle>.Get(other);
        if(obstacle != null)
        {
            Debug.Log("check obstacle");
            isOnObstacle = true;
            OnObstacle();
        }
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
        if (owner != null && owner.isActiveAndEnabled && !owner.IsDead)
        {
            owner.OnBulletDespawn(this);
        }
        SimplePool.DeSpawn(this);
        owner = null;
    }
    public virtual void OnObstacle()
    {   
        col.enabled = false;
        Invoke(nameof(DeSpawn),2f);
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
