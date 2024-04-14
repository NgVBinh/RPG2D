using System.Collections;
using UnityEngine;

public class Player : Entity
{
    [Header("Attack details")]
    public Vector2[] attackMovement;
    public float counterDuration;
    public bool isBusy { get; private set; }
    [Header("Move Infor")]
    public float moveSpeed = 1f;
    public float jumpForce = 1f;

    [Header("Dash Infor")]
    [SerializeField] private float dashCooldown;
    private float dashTimer;
    public float dashDir { get; private set; }
    public float dashSpeed;
    public float dashDuration;



    #region State
    public PlayerStateMachine stateMachine;
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerPrimaryAttackState attackState { get; private set; }
    public PlayerCounterAttackState counterAttackState { get; private set; }
    #endregion
    protected override void Awake()
    {

        base.Awake();

        stateMachine = new PlayerStateMachine();
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "Jump");
        attackState = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        counterAttackState = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        
        stateMachine.Initialize(idleState);

        
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();
        PlayerDashController();
    }

    public void AnimationTrigger()
    {
        stateMachine.currentState.AnimationFinishTrigger();
    }

    public IEnumerator BusyFor(float second)
    {
        isBusy = true;
        yield return new WaitForSeconds(second);
        isBusy = false;
    }



    private void PlayerDashController()
    {
        dashTimer -= Time.deltaTime;
        if (WallDetected()) return;

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashTimer < 0)
        {
            dashDir = Input.GetAxisRaw("Horizontal");

            if (dashDir == 0) dashDir = facingDir;

            stateMachine.ChangeState(dashState);
            dashTimer = dashCooldown;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            stateMachine.ChangeState(counterAttackState);
        }
    }
}
