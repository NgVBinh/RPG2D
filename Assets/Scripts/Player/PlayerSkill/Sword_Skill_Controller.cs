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

    [Header("Bounce Infor")]
    [SerializeField] private float speedBounce;
    [SerializeField] private float distanceCanBounce;
    private bool isBouncing;
    private int amountOfBounce;
    private List<Transform> enemysTransform;
    private int enemyTarget;

    [Header("Pierce infor")]
    private int amountOfPierce;

    [Header("Spin infor")]
    private float maxTravelDistance;
    private bool isSpinning;
    private bool wasStopped;
    private float spinDuration;
    private float spinTimer;

    private float spinDirection;

    private float hitTimer;
    private float hitCoolDown;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();

    }

    private void Start()
    {
        player = PlayerManager.instance.player;

        if (amountOfPierce <= 0)
            anim.SetBool("Rotation", true);

        if(isSpinning)
        {
            spinDirection = Mathf.Clamp(rb.velocity.x,-1,1);
        }
    }

    private void Update()
    {
        if (canRotate)
        {
            transform.right = rb.velocity;
        }

        SwordReturnPlayer();

        SwordBounceLogic();

        SwordSpinLogic();
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
                enemysTransform[enemyTarget].GetComponent<Enemy>().TakeDamage();

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

    private void SwordSpinLogic()
    {
        if (isSpinning)
        {
            if (Vector2.Distance(transform.position, player.transform.position) > maxTravelDistance && !wasStopped)
            {
                StopWhenSpinning();
            }

            if (wasStopped)
            {
                spinTimer -= Time.deltaTime;
                if (spinTimer < 0)
                {
                    isSpinning = false;
                    isReturning = true;
                }
                // sword move
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDirection, transform.position.y), 1.5f * Time.deltaTime);
                //Enemies take damage
                hitTimer -= Time.deltaTime;
                if (hitTimer < 0)
                {
                    hitTimer = hitCoolDown;

                    Collider2D[] collider2D = Physics2D.OverlapCircleAll(transform.position, 1);
                    foreach (Collider2D hit in collider2D)
                    {
                        if (hit.GetComponent<Enemy>() != null)
                        {
                            hit.GetComponent<Enemy>().TakeDamage();
                        }
                    }
                }
            }
        }
    }

    private void StopWhenSpinning()
    {
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
    }

    public void ReturnSword()
    {
        isReturning = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = null;
    }

    // set up sword
    public void SetupSword(Vector2 launchDir, float swordGravity)
    {
        rb.velocity = launchDir;
        rb.gravityScale = swordGravity;
    }

    public void SetupBounce(bool isBouncing, int amountOfBounce)
    {
        enemysTransform = new List<Transform>();
        this.isBouncing = isBouncing;
        this.amountOfBounce = amountOfBounce;
    }

    public void SetupPierce(int amountOfPierce)
    {
        this.amountOfPierce = amountOfPierce;
    }

    public void SetupSpin(bool isSpinning, float maxTravelDistance, float spinDuration,float hitCoolDown)
    {
        this.isSpinning = isSpinning;
        this.maxTravelDistance = maxTravelDistance;
        this.spinDuration = spinDuration;
        this.hitCoolDown = hitCoolDown;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning) return;

        if (isBouncing)
        {
            EnemysTargetForBounce(collision);
        }

        if (isSpinning)
        {
            StopWhenSpinning();
            return;
        }

        collision.GetComponent<Enemy>()?.TakeDamage();

        if (amountOfPierce > 0 && collision.GetComponent<Enemy>() != null)
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

        if (isBouncing && enemysTransform.Count != 0) return;

        anim.SetBool("Rotation", false);
        transform.parent = collision.transform;
    }

    private void EnemysTargetForBounce(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null && enemysTransform.Count <= 0)
        {
            Collider2D[] collider2D = Physics2D.OverlapCircleAll(transform.position, distanceCanBounce);
            foreach (Collider2D hit in collider2D)
            {
                if (hit.GetComponent<Enemy>() != null)
                {
                    enemysTransform.Add(hit.transform);
                }
            }
        }
    }

}
