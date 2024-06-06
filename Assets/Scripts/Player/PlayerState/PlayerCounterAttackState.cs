
using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    private bool canCreateClone;
    public PlayerCounterAttackState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        canCreateClone = true;
        player.VelocityZero();
        stateTimer = player.counterDuration;
        player.animator.SetBool("CounterSuccess", false);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheckTransform.position, player.attackRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.GetComponent<Enemy>() != null)
            {
                if (collider.GetComponent<Enemy>().CanBeStunned())
                {
                    stateTimer = 10;
                    player.animator.SetBool("CounterSuccess", true);

                    SkillManager.instance.parrySkill.UseSkill(); // restore when counter attack enemy

                    if (canCreateClone)
                    {
                        canCreateClone = false;
                        player.skill.parrySkill.MakeMirageOnParry(collider.transform);
                    }
                }
            }
        }

        if (stateTimer < 0 || triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
