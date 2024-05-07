
using UnityEngine;

public class EnemyState 
{
    protected Enemy enemy;
    protected EnemyStateMachine stateMachine;
    private string animationName;

    protected float stateTimer;
    protected bool triggerCalled;
    public EnemyState(Enemy enemy, EnemyStateMachine stateMachine, string animationName) {
        this.enemy = enemy;
        this.stateMachine = stateMachine;
        this.animationName = animationName;
    }

    public virtual void Enter()
    {
        enemy.animator.SetBool(animationName, true);
        triggerCalled = false;
    }
    public virtual void Exit() 
    { 
        enemy.animator.SetBool(animationName,false);   
        enemy.AssignLastAnimName( animationName);
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
