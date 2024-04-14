using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();
    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheckTransform.position, player.attackRadius);

        foreach(Collider2D collider in colliders)
        {
            if(collider.GetComponent<Enemy>()!= null)
            {
                collider.GetComponent<Enemy>().TakeDamage();
            }
        }
    }
}
