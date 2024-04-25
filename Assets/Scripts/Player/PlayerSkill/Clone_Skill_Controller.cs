using UnityEngine;

public class Clone_Skill_Controller : MonoBehaviour
{
    private float cloneTimerLosing;
    [SerializeField] private float cloneLossingSpeed;

    [SerializeField] private Transform attackCheckTransform;
    [SerializeField] private float attackRadius;

    private Transform closestEnemy;

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


    public void SetUpClone(Transform newTransform, float cloneDuration, bool canAttack)
    {
        transform.position = newTransform.position;
        cloneTimerLosing = cloneDuration;
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
            }
        }
    }

    private void FaceClosestEnemy()
    {
        float closestDistance = Mathf.Infinity;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);

        foreach (Collider2D collider in colliders)
        {
            if (collider.GetComponent<Enemy>() != null)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, collider.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = collider.transform;
                }
            }
        }

        if (closestEnemy != null)
        {
            if (transform.position.x > closestEnemy.transform.position.x)
            {
                transform.Rotate(0, 180, 0);
            }

        }
    }
}
