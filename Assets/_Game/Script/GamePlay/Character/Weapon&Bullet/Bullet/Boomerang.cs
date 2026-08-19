using UnityEngine;
public class Boomerang : BulletBase
{
    protected override BulletType Type => BulletType.Boomerang;
    [SerializeField] protected float rotationSpeed;
    [SerializeField] protected Vector3 targetPos;
    protected float catchDistance;
    protected bool isReturning;
    [SerializeField] protected bool isNullOwner;
    protected float speedUpReturning = 2f;
    public override void Init(Vector3 startDir, float rangeAttack, Vector3 rootPosion, Character ch)
    {
        base.Init(startDir, rangeAttack, rootPosion, ch);
        isReturning = false;
        isNullOwner = false;
        targetPos = Helper.CopyPositionXZ(owner.pos , TF.position);
        catchDistance = owner.catchDistance;
    }
    
    void Update()
    {
        //TODO doi thanh isdead
        if(owner == null || !owner.gameObject.activeSelf)
        {
            isNullOwner = true;
        }
        if (!isNullOwner)
        {
            targetPos =  Helper.CopyPositionXZ(owner.pos, TF.position);
            catchDistance = owner.catchDistance;
        }

        if (isReturning)
        {
            FlyBack();
            return;
        }
        FlyOut();
    }
    public override void OnTriggerEnter(Collider other)
    {
        Character target = CacheComponent<Collider,Character>.Get(other);
        if( owner == null || target == null || target == owner || target.IsDead ) return;
        CollectExp(target);
        target.OnDead();
    }
    public void FlyOut()
    {
        TF.position = Vector3.MoveTowards(TF.position,target,speedScale * Time.deltaTime);
        TF.Rotate(Vector3.down * rotationSpeed * Time.deltaTime);
        if (CheckDisTance())
        {
            isReturning = true;
        }
    }
    public void FlyBack()
    {
        TF.position = Vector3.MoveTowards(TF.position,targetPos,speedScale * speedUpReturning * Time.deltaTime);
        TF.Rotate(Vector3.down * rotationSpeed * Time.deltaTime);
        if (Helper.Distance2D(TF.position, targetPos) <= catchDistance * catchDistance)
        {
            DeSpawn();
        }
    }
}