using UnityEngine;
public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        //rb.velocity = new Vector2 (rb.velocity.x, player.jumpForce);
        player.SetVelocity(rb.velocity.x, player.jumpForce);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (horizontal != 0)
        {
            player.SetVelocity(horizontal * player.moveSpeed * 0.8f, rb.velocity.y);
        }

        if (rb.velocity.y < 0)
        {
            stateMachine.ChangeState(player.airState);
        }
    }
}
