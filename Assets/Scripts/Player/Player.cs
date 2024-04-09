using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Attack details")]
    public Vector2[] attackMovement;
    public bool isBusy { get; private set; }
    [Header("Move Infor")]
    public float moveSpeed = 1f;
    public float jumpForce = 1f;

    [Header("Dash Infor")]
    [SerializeField] private float dashCooldown;
    private float dashTimer;
    public float dashDir;
    public float dashSpeed;
    public float dashDuration;

    [Header("Collision infor")]
    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private Transform wallCheckTransform;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask WhatIsground;

    public int facingDir { get; private set; } = 1;
    private bool isFacingRight = true;

    #region Components
    public Animator animator { get; private set; }
    public Rigidbody2D rb { get; private set; }
    #endregion

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
    #endregion
    private void Awake()
    {

        stateMachine = new PlayerStateMachine();
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "Jump");
        attackState = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
    }
    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stateMachine.Initialize(idleState);
    }

    // Update is called once per frame
    private void Update()
    {
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
    }

    #region Velocity
    public void SetVelocity(float xInput, float yInput)
    {
        rb.velocity = new Vector2(xInput, yInput);
        FlipController(xInput);
    }
    public void VelocityZero() => rb.velocity = Vector2.zero;
    #endregion

    #region Colisions
    public bool GroundDetected() => Physics2D.Raycast(groundCheckTransform.position, Vector2.down, groundCheckDistance, WhatIsground);

    public bool WallDetected() => Physics2D.Raycast(wallCheckTransform.position, Vector2.right * facingDir, wallCheckDistance, WhatIsground);

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheckTransform.position, new Vector3(groundCheckTransform.position.x, groundCheckTransform.position.y - groundCheckDistance, 0));

        Gizmos.DrawLine(wallCheckTransform.position, new Vector3(wallCheckTransform.position.x + wallCheckDistance, wallCheckTransform.position.y));
    }
    #endregion

    #region Flip
    public void Flip()
    {
        facingDir *= -1;
        isFacingRight = !isFacingRight;
        transform.Rotate(0, 180, 0);
    }

    public void FlipController(float xInput)
    {
        if (xInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (xInput < 0 && isFacingRight)
        {
            Flip();
        }
    }
    #endregion
}
