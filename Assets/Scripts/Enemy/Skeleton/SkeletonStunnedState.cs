using UnityEngine;
public class SkeletonStunnedState : EnemyState
{
    private Skeleton_Enemy skeleton;
    public SkeletonStunnedState(Enemy enemy, EnemyStateMachine stateMachine, string animationName, Skeleton_Enemy skeleton) : base(enemy, stateMachine, animationName)
    {
        this.skeleton = skeleton;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = skeleton.stunDuration;
        skeleton.rb.velocity = new Vector2(-skeleton.facingDir * skeleton.stunDirection.x, skeleton.stunDirection.y);

        skeleton.entityFX.InvokeRepeating("HitBlinkRed", 0, 0.1f);
    }

    public override void Exit()
    {
        base.Exit();
        skeleton.entityFX.Invoke("CancelBlinkRed",0);
        
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(skeleton.idleState);
        }
    }
}
