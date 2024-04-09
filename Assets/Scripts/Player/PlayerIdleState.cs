using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.VelocityZero();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(horizontal!=0 && !player.isBusy) {
            stateMachine.ChangeState(player.moveState);
        }
    }

    
}
