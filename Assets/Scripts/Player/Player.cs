using System.Collections;
using UnityEditor;
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
    private float defaultMoveSpeed;
    private float defaultJumpForce;
    private float defaultDashSpeed;

    [Space]
    public float forceReturnSword;

    //[SerializeField] private float dashCooldown;
    //private float dashTimer;
    [Header("Dash Infor")]
    public float dashDuration;
    public float dashDir { get; private set; }
    public float dashSpeed;

    public SkillManager skill { get; private set; }
    public GameObject sword {  get; private set; }
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
    public PlayerAimSwordState aimSwordState { get; private set; }
    public PlayerCatchSwordState catchSwordState { get; private set; }
    public PlayerBlackholdeState blackholdeState { get; private set; }
    public PlayerDieState playerDieState { get; private set; }
    #endregion

    public GameObject characterUI;
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
        aimSwordState = new PlayerAimSwordState(this, stateMachine, "AimSword");
        catchSwordState = new PlayerCatchSwordState(this, stateMachine, "CatchSword");
        blackholdeState = new PlayerBlackholdeState(this, stateMachine, "Jump");
        playerDieState = new PlayerDieState(this, stateMachine, "Die");
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        
        stateMachine.Initialize(idleState);

        skill = SkillManager.instance;

        defaultMoveSpeed = moveSpeed;
        defaultJumpForce = jumpForce;
        defaultDashSpeed = dashSpeed;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();
        PlayerDashController();

        if (Input.GetKeyDown(KeyCode.E)&&skill.parrySkill.CanUseSkill() && skill.parrySkill.parryUnlocked)
        {
            //Debug.Log("counter attack");
            stateMachine.ChangeState(counterAttackState);
        }

        if (Input.GetKeyDown(KeyCode.Q) && skill.crystalSkill.crystalUnlocked)
        {
            skill.crystalSkill.CanUseSkill();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Inventory.instance.UseFlask();
        }

        //if (Input.GetKeyDown(KeyCode.Tab))
        //{
        //    if (characterUI.activeInHierarchy)
        //    {
        //        characterUI.SetActive(false);
        //    }
        //    else
        //    {
        //        characterUI.SetActive(true);
        //    }
        //}
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
        if (WallDetected()) return;

        if (!SkillManager.instance.dashSkill.dashUnlocked) return;

        if (Input.GetKeyDown(KeyCode.LeftShift) && skill.dashSkill.CanUseSkill())
        {
            dashDir = Input.GetAxisRaw("Horizontal");
            if (dashDir == 0) dashDir = facingDir;
            stateMachine.ChangeState(dashState);
        }
    }

    public void AssignNewSword(GameObject newSword)
    {
        sword = newSword;
    }

    public void CatchTheSword()
    {
        stateMachine.ChangeState(catchSwordState);
        sword = null;
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(playerDieState);
    }

    public override void EntitySlowBy(float slowPercentage, float duration)
    {
        base.EntitySlowBy(slowPercentage, duration);

        moveSpeed *= (1-slowPercentage);
        jumpForce *= (1-slowPercentage);
        dashSpeed *= (1-slowPercentage);

        animator.speed *= (1 - slowPercentage);
        Invoke("ReturnDefaultSpeed", duration);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();
        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;
        dashSpeed = defaultDashSpeed;

        animator.speed = 1;
    }
}
