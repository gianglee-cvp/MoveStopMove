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
    [SerializeField] protected float speed = 10f;
    [SerializeField] protected float range;
    [SerializeField] protected Rigidbody rb;
    protected Vector3 direction;

    protected Vector3 rootPos;

    public void Init(Transform start , float rangeAttack , Vector3 rootPosion)
    {
        direction = start.forward;
        range = rangeAttack;
        rootPos  = rootPosion;

        TF.position =  start.position;
        TF.rotation = start.rotation;
    }
    public virtual void Throw(Vector3 des)
    {
    }
    public bool CheckDisTance()
    {
        float offset = Helper.Distance2D(TF.position, rootPos);
        return (offset >  range * range) ? true : false;
    }
    //TODO cho lai vao pool
    public void DeSpawn()
    {
        Debug.Log("destroy");
        Destroy(gameObject);
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