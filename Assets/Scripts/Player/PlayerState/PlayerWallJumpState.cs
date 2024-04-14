public class PlayerWallJumpState : PlayerState
{
    public PlayerWallJumpState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetVelocity(-player.facingDir*5, player.jumpForce);
        stateTimer = 0.4f;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (player.GroundDetected())
        {
            stateMachine.ChangeState(player.idleState);
        }

        if(stateTimer < 0) {
            stateMachine.ChangeState(player.airState);
        }
    }

}
