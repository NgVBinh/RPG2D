using UnityEngine;

public class Clone_Skill_Controller : MonoBehaviour
{
    private Player player;

    private float cloneTimerLosing;
    [SerializeField] private float cloneLossingSpeed;

    private float cloneAttackMultiplier;
    [SerializeField] private Transform attackCheckTransform;
    [SerializeField] private float attackRadius;

    private bool canDuplicateClone;
    private int chanceToDuplicate;

    private Transform closestEnemy;

    private int facingDir = 1;

    #region Components
    private SpriteRenderer sr;
    private Animator animator;
    #endregion

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        cloneTimerLosing -= Time.deltaTime;
        if (cloneTimerLosing < 0)
        {
            sr.color = new Color(1, 1, 1, sr.color.a - Time.deltaTime * cloneLossingSpeed);

            if (sr.color.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }


    public void SetUpClone(Transform newTransform, float cloneDuration, bool canAttack,float attackMultiplier,Vector3 offset, Transform closestEnemy,bool canDuplicate, int chanceToDuplicate,Player player)
    {
        transform.position = newTransform.position+offset;
        cloneTimerLosing = cloneDuration;
        cloneAttackMultiplier = attackMultiplier;
        this.closestEnemy = closestEnemy;
        this.canDuplicateClone = canDuplicate;
        this.chanceToDuplicate = chanceToDuplicate;
        this.player = player;
        FaceClosestEnemy();

        if (canAttack)
        {
            animator.SetInteger("AttackCounter", Random.Range(1, 4));
        }
    }

    private void AnimationTrigger()
    {
        cloneTimerLosing = -0.1f;
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheckTransform.position, attackRadius);

        foreach (Collider2D collider in colliders)
        {
            if (collider.GetComponent<Enemy>() != null)
            {
                //player.characterStats.DoDamage(collider.GetComponent<CharacterStats>());
                PlayerStats playerStats = player.GetComponent<PlayerStats>();
                EnemyStats enemyStats = collider.GetComponent<EnemyStats>();

                playerStats.CloneDoDamage(enemyStats, cloneAttackMultiplier);

                if (player.skill.cloneSkill.aggresiveCloneUnlock)
                {
                    ItemData_Equipment swordEquipment = Inventory.instance.GetEquipment(EquipmentType.Sword);
                    if (swordEquipment != null)
                    {
                        swordEquipment.ExecuteItemEffect(collider.transform);
                    }
                }

                if(canDuplicateClone)
                {
                    if (Random.Range(0, 100) < chanceToDuplicate)
                    {
                        SkillManager.instance.cloneSkill.CreateClone(collider.transform, new Vector2(0.5f *facingDir,0));
                    }
                }
            }
        }
    }

    private void FaceClosestEnemy()
    {
        if (closestEnemy != null)
        {
            if (transform.position.x > closestEnemy.transform.position.x)
            {
                facingDir = -1;
                transform.Rotate(0, 180, 0);
            }

        }
    }
}
