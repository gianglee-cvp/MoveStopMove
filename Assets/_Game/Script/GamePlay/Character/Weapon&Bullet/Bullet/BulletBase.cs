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
    //TODO hide
    [SerializeField] protected Character owner;
    [SerializeField] protected float speed = 10f;
    [SerializeField] protected float range;
    protected Vector3 target;
    protected Vector3 rootPos;

    public virtual void Init(Vector3 startDir , float rangeAttack , Vector3 rootPosion , Character ch)
    {
        range = rangeAttack;
        target = Helper.CopyPositionXZ(TF.position + startDir * rangeAttack, TF.position);
        rootPos  = rootPosion;
        owner = ch;
    }
    public virtual void OnTriggerEnter(Collider other)
    {
        Character target = CacheComponent<Collider,Character>.Get(other);
        if( owner == null ||target == null || target == owner || target.IsDead) return;
        target.OnDead();
        DeSpawn();
    }
    public virtual void Throw()
    {
    }
    public bool CheckDisTance()
    {
        float offset = Helper.Distance2D(TF.position, rootPos);
        return (offset >  range * range) ? true : false;
    }
    public void DeSpawn()
    {
        SimplePool.DeSpawn(this);
    }
    private void OnDrawGizmos() 
    { 
        // Vị trí hiện tại 
        Gizmos.color = Color.green; 
        Gizmos.DrawSphere(TF.position, 0.15f); 
        // Vị trí đích 
        Gizmos.color = Color.red; 
        Gizmos.DrawSphere(rootPos, 0.15f); 
        // Đường nối giữa 2 điểm 
        Gizmos.color = Color.yellow; 
        Gizmos.DrawLine(TF.position, rootPos); 
    }
}