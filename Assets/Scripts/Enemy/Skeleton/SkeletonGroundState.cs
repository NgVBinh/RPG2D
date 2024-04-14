using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonGroundState : EnemyState
{
    protected Skeleton_Enemy skeleton;

    private Transform playerTransform;
    public SkeletonGroundState(Enemy enemy, EnemyStateMachine stateMachine, string animationName, Skeleton_Enemy skeleton) : base(enemy, stateMachine, animationName)
    {
        this.skeleton = skeleton;
    }

    public override void Enter()
    {
        base.Enter();
        playerTransform = GameObject.FindFirstObjectByType<Player>().transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (skeleton.PlayerDetected() || Vector2.Distance(skeleton.transform.position,playerTransform.position) <2)
        {
            stateMachine.ChangeState(skeleton.battleState);
        }
    }
}
