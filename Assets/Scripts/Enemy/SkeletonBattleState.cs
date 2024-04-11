using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    private Skeleton_Enemy skeleton;

    private Transform playerTransform;
    private int moveDir;
    public SkeletonBattleState(Enemy enemy, EnemyStateMachine stateMachine, string animationName,Skeleton_Enemy skeleton) : base(enemy, stateMachine, animationName)
    {
        this.skeleton = skeleton;
    }

    public override void Enter()
    {
        base.Enter();
        playerTransform = GameObject.FindObjectOfType<Player>().transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (enemy.PlayerDetected())
        {
            stateTimer = skeleton.battleTime;
            if(skeleton.PlayerDetected().distance < skeleton.attackDistance && Time.time>skeleton.lastTimeAttack) {
                stateMachine.ChangeState(skeleton.attackState);
            }
        }
        else
        {
            if(stateTimer<0 || Vector2.Distance(playerTransform.position,skeleton.transform.position)>5) stateMachine.ChangeState(skeleton.idleState);
        }
        

        if(playerTransform.position.x > skeleton.transform.position.x)
        {
            moveDir = 1;
        }else if( playerTransform.position.x < skeleton.transform.position.x)
        {
            moveDir = -1;
        }

        skeleton.SetVelocity(skeleton.moveSpeed * moveDir, skeleton.rb.velocity.y);
    }
}
