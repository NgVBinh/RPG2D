
using UnityEngine;

public class EnemyState 
{
    protected Enemy enemy;
    protected EnemyStateMachine stateMachine;
    private string animationName;

    protected float stateTimer;
    public EnemyState(Enemy enemy, EnemyStateMachine stateMachine, string animationName) {
        this.enemy = enemy;
        this.stateMachine = stateMachine;
        this.animationName = animationName;
    }

    public virtual void Enter()
    {
        enemy.animator.SetBool(animationName, true);
    }
    public virtual void Exit() 
    { 
        enemy.animator.SetBool(animationName,false);                            
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }
}
