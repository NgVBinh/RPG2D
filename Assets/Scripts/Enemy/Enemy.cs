using UnityEngine;

public class Enemy : Entity
{
    public EnemyStateMachine stateMachine;

    [Header("Attack Infor")]
    [SerializeField] protected LayerMask WhatIsPlayer;
    public float attackDistance;
    public float attackCoolDown;
    public float battleTime;
    [HideInInspector] public float lastTimeAttack;
    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();

    }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
    }

    public virtual RaycastHit2D PlayerDetected()=> Physics2D.Raycast(transform.position, Vector2.right * facingDir, 5, WhatIsPlayer);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + facingDir * attackDistance, transform.position.y));
    }

    public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();
}
