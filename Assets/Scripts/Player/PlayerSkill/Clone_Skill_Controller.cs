using UnityEngine;

public class Clone_Skill_Controller : MonoBehaviour
{
    private float cloneTimerLosing;
    [SerializeField] private float cloneLossingSpeed;

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


    public void SetUpClone(Transform newTransform, float cloneDuration, bool canAttack,Vector3 offset, Transform closestEnemy,bool canDuplicate, int chanceToDuplicate)
    {
        transform.position = newTransform.position+offset;
        cloneTimerLosing = cloneDuration;
        this.closestEnemy = closestEnemy;
        this.canDuplicateClone = canDuplicate;
        this.chanceToDuplicate = chanceToDuplicate;
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
                collider.GetComponent<Enemy>().TakeDamage();

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
