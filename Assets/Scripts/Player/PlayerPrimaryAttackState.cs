
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    private int comboCounter;
    private int comboWindow = 2;
    private float lastTime;
    public PlayerPrimaryAttackState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = 0.1f;

        if (comboCounter >2 || Time.time > lastTime+comboWindow) {
            comboCounter = 0;
        }
        //Debug.Log(comboCounter.ToString());
        player.animator.SetInteger("ComboCounter",comboCounter);

        float attackDir = player.facingDir;
        if (horizontal!=0) attackDir = horizontal;
        else
        player.SetVelocity(player.attackMovement[comboCounter].x * attackDir, player.attackMovement[comboCounter].y);

    }

    public override void Exit()
    {
        base.Exit();
        comboCounter++;
        lastTime = Time.time;
        player.StartCoroutine("BusyFor", 0.15f);
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0) player.VelocityZero();

        if (triggerCalled) stateMachine.ChangeState(player.idleState);
        
    }
}
