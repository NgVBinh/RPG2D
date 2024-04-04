using UnityEngine;
public class PlayerState
{
    protected float horizontal;
    protected Rigidbody2D rb;

    protected Player player;
    protected PlayerStateMachine stateMachine;


    private string animBoolName;
    public PlayerState(Player player, PlayerStateMachine stateMachine, string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter() {
        Debug.Log("Enter " + animBoolName);
        player.animator.SetBool(animBoolName, true);
        rb = player.rb;
    }
    public virtual void Update() {
        //Debug.Log("Update " + animBoolName);
        horizontal = Input.GetAxisRaw("Horizontal");
        player.animator.SetFloat("yVelocity", rb.velocity.y);
    }
    public virtual void Exit() {
        player.animator.SetBool(animBoolName, false);
        Debug.Log("Exit " + animBoolName);
    }
}
