
using UnityEngine;

public class SkeletonAttackState : EnemyState
{
    private Skeleton_Enemy skeleton;
    public SkeletonAttackState(Enemy enemy, EnemyStateMachine stateMachine, string animationName, Skeleton_Enemy skeleton) : base(enemy, stateMachine, animationName)
    {
        this.skeleton = skeleton;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.VelocityZero();
    }

    public override void Exit()
    {
        base.Exit();
        enemy.lastTimeAttack=Time.time;
    }

    public override void Update()
    {
        base.Update();
        if (triggerCalled)
        {
            stateMachine.ChangeState(skeleton.battleState);
        }
    }
}
