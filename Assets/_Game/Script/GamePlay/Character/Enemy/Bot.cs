using UnityEngine;
using NavMesh;
using _Framework.StateMachine;
using UnityEngine.AI;
public partial class Bot : Character
{
    [SerializeField] protected float patrolRadius = 20f;
    [SerializeField] protected int patrolSampleAttempts = 8;

    [SerializeField] protected NavMeshAgent agent ;
    private Vector3 destination;
    public bool IsDestionation => Vector3.Distance(tf.position, destination + (tf.position.y - destination.y) * Vector3.up) < 0.1f;

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
        spawnPos = tf != null ? tf.position : transform.position;

        ChangeState(idleState);
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

    //TODO khong quay ơ trong visual nua , quay root thoi 
    public void UpdateRotation()
    {
        if (tf == null || characterVisual == null)
        {
            return;
        }

        tf.rotation = characterVisual.transform.rotation;
        characterVisual.transform.localRotation = Quaternion.identity;
    }
}   
