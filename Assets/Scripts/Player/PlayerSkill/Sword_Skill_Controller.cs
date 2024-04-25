using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D circleCollider;

    [SerializeField] private float returnSpeed;

    private bool canRotate = true;
    private bool isReturning;

    private Player player;
    private void Awake()
    {
        anim=GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        player = PlayerManager.instance.player;
        anim.SetBool("Rotation", true);
    }

    private void Update()
    {
        if(canRotate)
        {
            transform.right = rb.velocity;
        }

        if(isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position,returnSpeed*Time.deltaTime);

            if (Vector2.Distance(transform.position, player.transform.position) < 1)
            {
                player.CatchTheSword();
                Destroy(gameObject);
            } 
        }
    }

    public void ReturnSword()
    {
        isReturning = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = null;
    }

    public void SetupSword(Vector2 launchDir,float swordGravity)
    {
        rb.velocity = launchDir;
        rb.gravityScale = swordGravity;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning) return;
        anim.SetBool("Rotation", false);
        canRotate = false;
        rb.bodyType = RigidbodyType2D.Kinematic;
        circleCollider.enabled = false;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        transform.parent = collision.transform;
    }
}
