using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackholdeState : PlayerState
{
    private float flyTimer=1f;
    private bool skillUsed;

    private float defaultGravity;
    public PlayerBlackholdeState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();
        defaultGravity =player.rb.gravityScale;

        skillUsed = false;
        stateTimer = flyTimer;
        rb.gravityScale = 0;
    }

    public override void Exit()
    {
        base.Exit();
        player.entityFX.MakeTransprent(false);
        player.rb.gravityScale = defaultGravity;
    }

    public override void Update()
    {
        base.Update();
        if(stateTimer > 0)
        {
            rb.velocity = new Vector2(0, 2.5f);
        }

        if(stateTimer < 0)
        {
            rb.velocity = new Vector2(0, -0.01f);
            if(!skillUsed)
            {
                if(player.skill.blackholeSkill.CanUseSkill())
                    skillUsed = true;
            }
        }

        if(player.skill.blackholeSkill.SkillCompleted())
        {
            stateMachine.ChangeState(player.airState);
        }
    }
}
