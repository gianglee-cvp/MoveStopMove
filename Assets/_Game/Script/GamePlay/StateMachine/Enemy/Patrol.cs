using _Framework.StateMachine;

public class PatrolState : IState<Bot>
{
    public void OnEnter(Bot enemy)
    {
        enemy.InitPatrol();
    }

    public void OnExecute(Bot enemy)
    {
        enemy.ExecutePatrol();
    }

    public void OnExit(Bot enemy)
    {
        enemy.ExitPatrol();
    }
}
