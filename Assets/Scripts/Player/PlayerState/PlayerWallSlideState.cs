
using UnityEngine;
using UnityEngine.UI;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
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

        if(Input.GetKey(KeyCode.Space)) {
            stateMachine.ChangeState(player.wallJumpState);
            return;
        }

        if (player.GroundDetected())
        {
            stateMachine.ChangeState(player.idleState);
        }

        if (horizontal != 0 && player.facingDir != horizontal)
        {
            stateMachine.ChangeState(player.airState);
        }

        if(!player.GroundDetected() && !player.WallDetected())
        {
            stateMachine.ChangeState(player.airState);
        }

        // slide speed controll
        if (vertical < 0)
        {
            rb.velocity = new UnityEngine.Vector2(0, rb.velocity.y);

        }
        else
        {
            rb.velocity = new UnityEngine.Vector2(0, rb.velocity.y * 0.7f);
        }
    }

}
