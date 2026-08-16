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
    protected Character owner;
    [SerializeField] protected float speed = 10f;
    [SerializeField] protected float range;
    [SerializeField] protected Rigidbody rb;
    protected Vector3 direction;
    protected Vector3 rootPos;

    public void Init(Vector3 startDir , float rangeAttack , Vector3 rootPosion , Character ch)
    {
        rb.linearVelocity = Vector3.zero;
        direction = startDir;
        range = rangeAttack;
        rootPos  = rootPosion;
        owner = ch;
    }
    public void OnTriggerEnter(Collider other)
    {
        Character target = CacheComponent<Collider,Character>.Get(other);
        if(owner == null ||target == null || target == owner ) return;
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
        // Debug.Log("destroy");
        SimplePool.DeSpawn(this);
        // Destroy(gameObject);
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