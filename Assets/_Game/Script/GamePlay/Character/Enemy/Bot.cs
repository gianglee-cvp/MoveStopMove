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
        SetTarget();
        currentState?.OnExecute(this);
    }
    public override void OnInit()
    {
        base.OnInit();
        spawnPos = TF != null ? TF.position : transform.position;
        //TODO đổi thành random skin
        characterVisual.ApplySkin();
        ChangeState(idleState);
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
        return (currentTarget != null);
    }
    public void SetDestination(Vector3 destination)
    {
        this.destination = destination;
        agent.SetDestination(destination);
    }
    public override void OnDead()
    {
        base.OnDead();
        BotManager.Instance.DeSpawnBot(this);
    }

}   
