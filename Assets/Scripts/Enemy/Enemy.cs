using UnityEngine;

public class Enemy : Entity
{
    public EnemyStateMachine stateMachine;
    [Header("Stunned infor")]
    public Vector2 stunDirection;
    public float stunDuration;

    protected bool canbeStun;
    [SerializeField] protected GameObject counterImage;

    [Header("Move Infor")]
    public float idleTime;
    public float moveSpeed;

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

    public virtual void OpenCounterAttackWindow()
    {
        canbeStun = true;
        counterImage.SetActive(true);
    }

    public virtual void CloseCounterAttackWindow()
    {
        canbeStun = false;
        counterImage.SetActive(false);
    }

    public virtual bool CanBeStunned()
    {
        if (canbeStun)
        {
            CloseCounterAttackWindow();
            return true;
        }    
        return false;
    }
}
