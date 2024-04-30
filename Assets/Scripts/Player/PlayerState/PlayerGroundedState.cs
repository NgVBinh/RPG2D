using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
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

        if (Input.GetKeyDown(KeyCode.R))
        {
            stateMachine.ChangeState(player.blackholdeState);
        }

        if(Input.GetMouseButtonDown(1) && HasNoSword() && player.skill.swordSkill.CanUseSkill())
        {
            stateMachine.ChangeState(player.aimSwordState);
        }

        if(Input.GetKeyDown(KeyCode.Space)&& player.GroundDetected())
        {
            stateMachine.ChangeState(player.jumpState);
        }

        if(!player.GroundDetected())
        {
            stateMachine.ChangeState(player.airState);
        }

        if(Input.GetMouseButtonDown(0))
        {
            stateMachine.ChangeState(player.attackState);
        }
        //if(Input.GetKeyUp(KeyCode.LeftShift) && player.GroundDetected())
        //{
        //    stateMachine.ChangeState(player.dashState);
        //}
    }

    private bool HasNoSword()
    {
        if(!player.sword)
        {
            return true;
        }

        player.sword.GetComponent<Sword_Skill_Controller>().ReturnSword();
        return false;
    }

}
