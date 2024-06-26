using System;
using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Collision infor")]
    [SerializeField] protected Transform groundCheckTransform;
    [SerializeField] protected float groundCheckDistance;

    [SerializeField] protected Transform wallCheckTransform;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask WhatIsground;

    public Transform attackCheckTransform;
    public float attackRadius;

    public int facingDir { get; private set; } = 1;
    protected bool isFacingRight = true;

    [Header("KnockBack infor")]
    [SerializeField] protected Vector2 knockbackDir;
    [SerializeField] private float knockbackDuration;
    private bool isKnockback;

    public Action onFlipped;

    #region Components
    public Animator animator { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public CapsuleCollider2D capsuleCollider { get; private set; }
    public EntityFX entityFX { get; private set; }
    public CharacterStats characterStats { get; private set; }
    public SpriteRenderer spriteRenderer { get; private set; }
    #endregion


    protected virtual void Awake()
    {

    }
    protected virtual void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();

        entityFX = GetComponentInChildren<EntityFX>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        characterStats = GetComponent<CharacterStats>();
    }
    protected virtual void Update()
    {

    }


    #region Velocity
    public void SetVelocity(float xInput, float yInput)
    {
        if (isKnockback) return;
        rb.velocity = new Vector2(xInput, yInput);
        FlipController(xInput);
    }
    public void VelocityZero()
    {
        if (isKnockback) return;
        rb.velocity = Vector2.zero;
    }

    #endregion

    #region Colisions
    public virtual bool GroundDetected() => Physics2D.Raycast(groundCheckTransform.position, Vector2.down, groundCheckDistance, WhatIsground);

    public virtual bool WallDetected() => Physics2D.Raycast(wallCheckTransform.position, Vector2.right * facingDir, wallCheckDistance, WhatIsground);

    public virtual void BeDamagedEffect()
    {
        if (entityFX != null)
        {
            entityFX.StartCoroutine("HitEffect");
        }
        StartCoroutine("HitKnockback");

    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheckTransform.position, new Vector3(groundCheckTransform.position.x, groundCheckTransform.position.y - groundCheckDistance, 0));

        Gizmos.DrawLine(wallCheckTransform.position, new Vector3(wallCheckTransform.position.x + wallCheckDistance, wallCheckTransform.position.y));

        Gizmos.DrawWireSphere(attackCheckTransform.position, attackRadius);
    }
    #endregion

    #region Flip
    public void Flip()
    {
        facingDir *= -1;
        isFacingRight = !isFacingRight;
        transform.Rotate(0, 180, 0);

        if (onFlipped != null)
            onFlipped();
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

    protected virtual IEnumerator HitKnockback()
    {
        isKnockback = true;
        rb.velocity = new Vector2(knockbackDir.x * (-facingDir), knockbackDir.y);
        yield return new WaitForSeconds(knockbackDuration);
        rb.velocity = Vector2.zero;
        isKnockback = false;
    }

    public virtual void Die()
    {

    }

    public virtual void EntitySlowBy(float slowPercentage, float duration){}

    protected virtual void ReturnDefaultSpeed()
    {

    }
}
