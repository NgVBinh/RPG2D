using UnityEngine;
public class PlayerState
{
    protected float horizontal;
    protected float vertical;
    protected Rigidbody2D rb;

    protected Player player;
    protected PlayerStateMachine stateMachine;

    private string animBoolName;

    protected float stateTimer;
    public PlayerState(Player player, PlayerStateMachine stateMachine, string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter() {
        //Debug.Log("Enter " + animBoolName);
        player.animator.SetBool(animBoolName, true);
        rb = player.rb;
    }
    public virtual void Update() {
        //Debug.Log("Update " + animBoolName);
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        player.animator.SetFloat("yVelocity", rb.velocity.y);
        stateTimer-=Time.deltaTime;
    }
    public virtual void Exit() {
        player.animator.SetBool(animBoolName, false);
        //Debug.Log("Exit " + animBoolName);
    }
}
