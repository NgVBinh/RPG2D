
public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
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
        if (player.GroundDetected())
        {
            stateMachine.ChangeState(player.idleState);
        }

        if (horizontal != 0)
        {
            player.SetVelocity(horizontal * player.moveSpeed*0.8f, rb.velocity.y);
        }

        if(player.WallDetected())
        {
            stateMachine.ChangeState(player.wallSlideState);
        }
    }
}
