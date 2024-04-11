using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonIdleState : EnemyState
{
    private Skeleton_Enemy enemySkeleton;
    public SkeletonIdleState(Enemy enemy, EnemyStateMachine enemyState, string animationName, Skeleton_Enemy skeleton) : base(enemy, enemyState, animationName)
    {
        this.enemySkeleton = skeleton;
    }

    public override void Enter()
    {
        base.Enter();
        enemySkeleton.VelocityZero();
        stateTimer = enemySkeleton.idleTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemySkeleton.moveState);
        }
    }
}
