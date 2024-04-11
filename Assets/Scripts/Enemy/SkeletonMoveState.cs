using UnityEngine;

public class SkeletonMoveState : SkeletonGroundState
{
    public SkeletonMoveState(Enemy enemy, EnemyStateMachine stateMachine, string animationName, Skeleton_Enemy skeleton) : base(enemy, stateMachine, animationName, skeleton)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        skeleton.SetVelocity(skeleton.moveSpeed * skeleton.facingDir, skeleton.rb.velocity.y);

        if(!skeleton.GroundDetected()|| skeleton.WallDetected())
        {
            stateMachine.ChangeState(skeleton.idleState);
            skeleton.Flip();
        }
    }
}
