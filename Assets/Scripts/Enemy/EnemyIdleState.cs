
using UnityEngine;

public class EnemyIdleState : EnemyState
{
    public EnemyIdleState(Enemy enemy, EnemyStateMachine enemyState, string animationName) : base(enemy, enemyState, animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = 5f;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        Debug.Log(stateTimer.ToString());
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
    }

    
}
