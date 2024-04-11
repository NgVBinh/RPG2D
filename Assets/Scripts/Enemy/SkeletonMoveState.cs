public class SkeletonMoveState : EnemyState
{
    private Skeleton_Enemy enemySkeleton;
    public SkeletonMoveState(Enemy enemy, EnemyStateMachine enemyState, string animationName, Skeleton_Enemy enemySkeleton) : base(enemy, enemyState, animationName)
    {
        this.enemySkeleton = enemySkeleton;
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

        enemySkeleton.SetVelocity(enemySkeleton.moveSpeed * enemySkeleton.facingDir, enemySkeleton.rb.velocity.y);

        if(!enemySkeleton.GroundDetected()|| enemySkeleton.WallDetected())
        {
            stateMachine.ChangeState(enemySkeleton.idleState);
            enemySkeleton.Flip();
        }
    }
}
