using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = player.dashDuration;
        player.skill.dashSkill.CloneOnDash();
    }

    public override void Exit()
    {
        base.Exit();
        player.skill.dashSkill.CloneOnArrival();
        player.SetVelocity(0, rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(player.dashDir * player.dashSpeed, 0);

        if (stateTimer < 0) {
            if (player.GroundDetected())
                stateMachine.ChangeState(player.idleState);
            else
                stateMachine.ChangeState(player.airState);
        }

        if (!player.GroundDetected()&& player.WallDetected())
        {
            stateMachine.ChangeState(player.wallSlideState);
        }
    }

 }
