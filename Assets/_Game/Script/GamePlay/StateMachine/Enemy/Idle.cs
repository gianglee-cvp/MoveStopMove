using _Framework.StateMachine;

public class IdleState : IState<Bot>
{
    public void OnEnter(Bot enemy)
    {
        enemy.InitIdle();
    }

    public void OnExecute(Bot enemy)
    {
        enemy.ExecuteIdle();
    }

    public void OnExit(Bot enemy)
    {
        enemy.ExitIdle();
    }
}
