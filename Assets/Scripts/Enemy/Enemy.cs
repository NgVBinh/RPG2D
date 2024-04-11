using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyStateMachine stateMachine;
    
    public Animator animator { get; private set; }

    public EnemyIdleState idleState { get; private set; }
    public EnemyMoveState moveState { get; private set; }

    private void Awake()
    {
        stateMachine = new EnemyStateMachine();
        idleState = new EnemyIdleState(this, stateMachine,"Idle");
        moveState = new EnemyMoveState(this, stateMachine, "Move");
    }
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();

        stateMachine.Initialize(idleState);
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.currentState.Update();
    }
}
