using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Collision infor")]
    [SerializeField] protected Transform groundCheckTransform;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheckTransform;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask WhatIsground;

    public int facingDir { get; private set; } = 1;
    protected bool isFacingRight = true;

    #region Components
    public Animator animator { get; private set; }
    public Rigidbody2D rb { get; private set; }
    #endregion

    protected virtual void Awake()
    {

    }
    protected virtual void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    protected virtual void Update()
    {

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
    public virtual bool GroundDetected() => Physics2D.Raycast(groundCheckTransform.position, Vector2.down, groundCheckDistance, WhatIsground);

    public virtual bool WallDetected() => Physics2D.Raycast(wallCheckTransform.position, Vector2.right * facingDir, wallCheckDistance, WhatIsground);

    protected virtual void OnDrawGizmos()
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
