
using UnityEngine;

public class EnemyState 
{
    protected Enemy enemyBase;
    protected EnemyStateMachine stateMachine;
    private string animationName;

    protected float stateTimer;
    public EnemyState(Enemy enemy, EnemyStateMachine stateMachine, string animationName) {
        this.enemyBase = enemy;
        this.stateMachine = stateMachine;
        this.animationName = animationName;
    }

    public virtual void Enter()
    {
        enemyBase.animator.SetBool(animationName, true);
    }
    public virtual void Exit() 
    { 
        enemyBase.animator.SetBool(animationName,false);                            
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }
}
