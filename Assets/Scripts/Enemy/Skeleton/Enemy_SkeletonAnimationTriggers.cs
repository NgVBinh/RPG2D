using UnityEngine;

public class Enemy_SkeletonAnimationTriggers : MonoBehaviour
{
    private Skeleton_Enemy skeleton => GetComponentInParent<Skeleton_Enemy>();

    private void AnimationTrigger()
    {
        skeleton.AnimationFinishTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(skeleton.attackCheckTransform.position, skeleton.attackRadius);

        foreach(Collider2D collider in colliders)
        {
            if(collider.GetComponent<Player>() != null)
            {
                collider.GetComponent<Player>().TakeDamage();
            }
        }
    }

    private void OpenCounterWindow()=> skeleton.OpenCounterAttackWindow();

    private void CloseCouterWindow()=> skeleton.CloseCounterAttackWindow();
}
