using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill_Controller : MonoBehaviour
{
    private float crystalExistTimer;

    private bool canExplode;
    private bool canMove;
    private float moveSpeed;

    private Transform closestEnemy;

    private bool canGrow;
    [SerializeField] private float growSpeed;

    [SerializeField] private LayerMask whatIsEnemy;

    private Animator animator;
    private CircleCollider2D circleCollider;
    public void SetupCrystal(float crystalduration, bool canExplode, bool canMove, float moveSpeed, Transform closestEnemy)
    {
        crystalExistTimer = crystalduration;
        this.canExplode = canExplode;
        this.canMove = canMove;
        this.moveSpeed = moveSpeed;
        this.closestEnemy = closestEnemy;

        animator = GetComponent<Animator>();
        circleCollider = GetComponent<CircleCollider2D>();

        
    }

    private void Update()
    {
        crystalExistTimer -= Time.deltaTime;
        if (crystalExistTimer < 0)
        {
            FinishCrystal();
        }

        if(canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale,new Vector2(3,3), growSpeed*Time.deltaTime);
        }

        if(canMove && closestEnemy!=null)
        {
            transform.position = Vector2.MoveTowards(transform.position,closestEnemy.position,moveSpeed*Time.deltaTime);
            if (Vector2.Distance(transform.position, closestEnemy.position) < 1)
            {
                canMove = false;
                FinishCrystal() ;
            }
        }
    }

    public void FinishCrystal()
    {
        if (canExplode) {
            canGrow = true;
            animator.SetTrigger("Explode");
        }
        else SeflDestroy();
    }

    public void SeflDestroy() => Destroy(gameObject);

    public void ChooseRandomEnemy()
    {
        float blackholdeRadius = SkillManager.instance.blackholeSkill.GetBlackholeRadius();
        Debug.Log(blackholdeRadius);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, blackholdeRadius, whatIsEnemy);

        if(colliders.Length > 0)
        {
            closestEnemy = colliders[Random.Range(0,colliders.Length)].transform;
        }
    }

    private void ExplodeAnimationEvent() {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, circleCollider.radius);

        foreach (Collider2D collider in colliders)
        {
            collider.GetComponent<Enemy>()?.TakeDamage();
            //if (collider.GetComponent<Enemy>() != null)
            //{
            //    collider.GetComponent<Enemy>().TakeDamage();
            //}
        }
    }
}
