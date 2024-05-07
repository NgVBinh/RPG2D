using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Skeleton_Enemy skeleton;
    protected override void Start()
    {
        base.Start();
        skeleton = GetComponent<Skeleton_Enemy>();
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }

    public override void Die()
    {
        base.Die();
        skeleton.Die();
    }
}
