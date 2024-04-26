using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [Header("Bounce Infor")]
    [SerializeField] private float speedBounce;
    private bool isBouncing ;
    private int amountOfBounce;
    private List<Transform> enemysTransform;
    private int enemyTarget;

    [Header("Pierce infor")]
    private int amountOfPierce;
    private void Awake()
    {
        anim=GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        player = PlayerManager.instance.player;

        if(amountOfPierce<=0)
        anim.SetBool("Rotation", true);
    }

    private void Update()
    {
        if (canRotate)
        {
            transform.right = rb.velocity;
        }

        SwordReturnPlayer();

        SwordBounceLogic();
    }

    private void SwordReturnPlayer()
    {
        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, player.transform.position) < 1)
            {
                player.CatchTheSword();
                Destroy(gameObject);
            }
        }
    }

    private void SwordBounceLogic()
    {
        if (isBouncing && enemysTransform.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemysTransform[enemyTarget].position, speedBounce * Time.deltaTime);

            if (Vector2.Distance(transform.position, enemysTransform[enemyTarget].position) < 0.05f)
            {
                enemyTarget++;
                amountOfBounce--;

                if (amountOfBounce <= 0)
                {
                    isBouncing = false;
                    isReturning = true;
                }
                if (enemyTarget >= enemysTransform.Count)
                {
                    enemyTarget = 0;
                }
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

    public void SetupBounce(bool isBouncing,int amountOfBounce)
    {
        enemysTransform = new List<Transform>();
        this.isBouncing = isBouncing;
        this.amountOfBounce = amountOfBounce;
    }

    public void SetupPierce(int amountOfPierce)
    {
        this.amountOfPierce = amountOfPierce;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning) return;

        collision.GetComponent<Enemy>()?.TakeDamage();

        if (isBouncing)
        {
            if (collision.GetComponent<Enemy>() != null && enemysTransform.Count <= 0)
            {
                Collider2D[] collider2D = Physics2D.OverlapCircleAll(transform.position, 5f);
                foreach (Collider2D hit in collider2D)
                {
                    if (hit.GetComponent<Enemy>() != null)
                    {
                        enemysTransform.Add(hit.transform);
                    }
                }
            }
        }

        if(amountOfPierce>0 && collision.GetComponent<Enemy>()!= null)
        {
            amountOfPierce--;
            return;
        }

        StuckInto(collision);
    }

    private void StuckInto(Collider2D collision)
    {
        canRotate = false;
        circleCollider.enabled = false;

        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if(isBouncing && enemysTransform.Count!=0) return;

        anim.SetBool("Rotation", false);
        transform.parent = collision.transform;
    }
}
