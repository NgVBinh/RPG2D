public class EnemyStateMachine 
{
    public EnemyState currentState;

    public void Initialize(EnemyState enemyState)
    {
        currentState = enemyState;
        currentState.Enter();
    }
    public void ChangeState(EnemyState state)
    {
        currentState.Exit();
        currentState = state;
        currentState.Enter();
    }
}
