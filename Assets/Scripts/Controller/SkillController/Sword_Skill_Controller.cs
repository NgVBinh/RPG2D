using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D circleCollider;

    private float freezeTime;

    private float returnSpeed;

    private bool canRotate = true;
    private bool isReturning;

    private Player player;

    [Header("Bounce Infor")]
    private float speedBounce;
    private float distanceCanBounce;
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

        Invoke("DestroyMe", 7f);
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

    private void DestroyMe()
    {
        Destroy(gameObject);
    }

    private void SwordReturnPlayer()
    {
        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, player.transform.position) < 1)
            {
                player.CatchTheSword();
                DestroyMe();
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
                SwordSkillDamage(enemysTransform[enemyTarget].GetComponent<Enemy>());

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
                            SwordSkillDamage(hit.GetComponent<Enemy>());
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
    public void SetupSword(Vector2 launchDir, float swordGravity,float freezeTime,float returnSpeed)
    {
        rb.velocity = launchDir;
        rb.gravityScale = swordGravity;
        this.freezeTime = freezeTime;
        this.returnSpeed = returnSpeed;
    }

    public void SetupBounce(bool isBouncing, int amountOfBounce,float speedBounce,float distanceCanBounce)
    {
        enemysTransform = new List<Transform>();
        this.isBouncing = isBouncing;
        this.amountOfBounce = amountOfBounce;
        this.speedBounce = speedBounce;
        this.distanceCanBounce = distanceCanBounce;
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

        if (collision.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            SwordSkillDamage(enemy);
        }

        if (amountOfPierce > 0 && collision.GetComponent<Enemy>() != null)
        {
            amountOfPierce--;
            return;
        }

        StuckInto(collision);
    }

    private void SwordSkillDamage(Enemy enemy)
    {
        player.characterStats.DoDamage(enemy.GetComponent<CharacterStats>());
        enemy.FreezeTimeFor(freezeTime);

        ItemData_Equipment swordEquipment = Inventory.instance.GetEquipment(EquipmentType.Sword);
        if (swordEquipment != null)
        {
            swordEquipment.ExecuteItemEffect(enemy.transform);
        }
       
        ItemData_Equipment amuletEquipment = Inventory.instance.GetEquipment(EquipmentType.Amulet);
        if (amuletEquipment != null)
        {
            amuletEquipment.ExecuteItemEffect(enemy.transform);
        }
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
