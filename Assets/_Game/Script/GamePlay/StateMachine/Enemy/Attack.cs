using _Framework.StateMachine;

public class AttackState : IState<Bot>
{
    public void OnEnter(Bot enemy)
    {
        enemy.InitAttack();
    }

    public void OnExecute(Bot enemy)
    {
        enemy.ExecuteAttack();
    }

    public void OnExit(Bot enemy)
    {
        enemy.ExitAttack();
    }
}
