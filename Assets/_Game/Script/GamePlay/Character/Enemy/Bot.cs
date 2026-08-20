using UnityEngine;
using _Framework.StateMachine;
using UnityEngine.AI;
public partial class Bot : Character
{
    [SerializeField] protected float patrolRadius = 20f;
    [SerializeField] protected int patrolSampleAttempts = 8;

    [SerializeField] protected NavMeshAgent agent ;
    private Vector3 destination;
    public bool IsDestionation => Vector3.Distance(TF.position, destination + (TF.position.y - destination.y) * Vector3.up) < 0.1f;

    protected Vector3 spawnPos;

    //TODO co the doi thanh code khong dung gamobject
    [SerializeField] GameObject indicator;
    void Update()
    {
        if(isDead) return;
        SetTarget();
        currentState?.OnExecute(this);
    }
    public override void OnInit()
    {
        base.OnInit();
        spawnPos = TF.position;
        characterVisual.ApplyRandomSkin();
        ChangeState(idleState);
        // TargetContainer.Instance.RegisterTarget(this);
        CanvasGamePlay canvas = UIManager.Instance.GetUI<CanvasGamePlay>();
        canvas.RegisterTarget(this);
    }
    public override void Idle()
    {
        base.Idle();
        SetDestination(TF.position);
    }
    public override void Attack()
    {
        SetDestination(TF.position);
        base.Attack();
    }

    public void ShowTargetIndicator()
    {
        if (!indicator.activeSelf)
        {
            indicator.SetActive(true);
        }
    }

    public void HideTargetIndicator()
    {
        if (indicator.activeSelf)
        {
            indicator.SetActive(false);
        }
    }
    public bool HasTarGet()
    {
        return currentTarget != null;
    }
    public void SetDestination(Vector3 destination)
    {
        this.destination = destination;
        agent.SetDestination(destination);
    }
    public override void OnDead()
    {
        SetDestination(TF.position);
        base.OnDead();
        //TODO khong dung invoke
        Invoke(nameof(DeSpawnBot) , 1.5f);
        UnregisterTarget();
    }
    public void UnregisterTarget()
    {
        CanvasGamePlay canvas = UIManager.Instance.GetUI<CanvasGamePlay>();
        canvas.UnregisterTarget(this);
    }
    public void DeSpawnBot()
    {
        characterVisual.DespawnSkin();
        BotManager.Instance.DeSpawnBot(this);
    }
    public Vector3 GetHeadPos()
    {
        return characterVisual.HeadPos;
    }
}   
