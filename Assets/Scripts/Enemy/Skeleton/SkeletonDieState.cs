using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonDieState : EnemyState
{
    private Skeleton_Enemy skeleton;
    public SkeletonDieState(Enemy enemy, EnemyStateMachine stateMachine, string animationName, Skeleton_Enemy skeleton) : base(enemy, stateMachine, animationName)
    {
        this.skeleton = skeleton;
    }

    public override void Enter()
    {
        base.Enter();
        skeleton.animator.SetBool(skeleton.lastAnimBoolName, true);
        skeleton.capsuleCollider.enabled = false;
        skeleton.animator.speed= 0;
        stateTimer = 0.1f;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if(stateTimer > 0)
        {
            skeleton.rb.velocity = new Vector2(0, 10);
        }
    }
}
