using _Framework.StateMachine;
using UnityEngine;
using UnityEngine.AI;
public partial class Bot
{
    protected float timer;
    protected float randomTime;

    protected static IState<Bot> idleState = new IdleState();
    protected static IState<Bot> patrolState = new PatrolState();
    protected static IState<Bot> attackState = new AttackState();
    protected IState<Bot> currentState = null;

    #region Statemachine
    public void InitIdle()
    {
        timer = 0f;
        Idle();
        randomTime = Random.Range(1f, 5f);
    }
    public void ExecuteIdle()
    {
        if (HasTarGet())
        {
            ChangeState(attackState);
            return;
        }

        timer += Time.deltaTime;
        if (timer >= randomTime)
        {
            ChangeState(patrolState);
        }
    }
    public void ExitIdle(){}
    public void InitPatrol()
    {
        Vector3 patrolPoint;
        if (!TryGetRandomPatrolPoint(out patrolPoint))
        {
            ChangeState(idleState);
            return;
        }

        ChangeAnim(CharacterAnimType.Run);
        SetDestination(patrolPoint);
    }
    public void ExecutePatrol()
    {
        if (HasTarGet())
        {
            ChangeState(attackState);
            return;
        }

        if (agent == null || IsDestionation)
        {
            ChangeState(idleState);
        }
    }
    public void ExitPatrol()
    {
    }
    public void InitAttack()
    {
        if (!HasTarGet())
        {
            ChangeState(idleState);
            return;
        }

        if (!isAttacking && isAttackable)
        {
            SetDestination(tf.position);
            Attack();
        }
    }
    public void ExecuteAttack()
    {
        if (!HasTarGet())
        {
            if (isAttacking)
            {
                CancelAttack();
            }

            ChangeState(idleState);
            return;
        }

        if (!isAttacking && isAttackable)
        {
            Attack();
        }
    }
    public void ExitAttack()
    {
        if (isAttacking)
        {
            CancelAttack();
        }

        UpdateRotation();
    }

    private bool TryGetRandomPatrolPoint(out Vector3 patrolPoint)
    {
        for (int i = 0; i < patrolSampleAttempts; i++)
        {
            Vector2 randomCircle = Random.insideUnitCircle * patrolRadius;
            Vector3 randomPoint = spawnPos + new Vector3(randomCircle.x, 0f, randomCircle.y);

            if (UnityEngine.AI.NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, patrolRadius, UnityEngine.AI.NavMesh.AllAreas))
            {
                patrolPoint = hit.position;
                return true;
            }
        }

        patrolPoint = spawnPos;
        return false;
    }
    #endregion
    public void ChangeState(IState<Bot> newState)
    {
        if (currentState != newState)
        {
            Debug.Log(gameObject.name +$"ChangeState: {currentState?.GetType().Name} -> {newState?.GetType().Name}");
            currentState?.OnExit(this);
            currentState = newState;
            currentState?.OnEnter(this);
        }
    }
}
