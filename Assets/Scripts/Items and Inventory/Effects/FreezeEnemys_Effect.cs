using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Freeze enemies Effect", menuName = "Data/Item Effect/Freeze Enemies")]
public class FreezeEnemys_Effect : ItemEffect
{
    [SerializeField] private float duration;
    [SerializeField] private float radius;
    public override void ExecuteEffect(Transform _transform)
    {
        PlayerStats playerStats= PlayerManager.instance.player.GetComponent<PlayerStats>();
        // Use effect when player health < 30%
        if (playerStats.currentHealth > playerStats.GetMaxHealthValue() * 0.3f) return;

        //if (!Inventory.instance.CanUseArmorEffect()) return;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_transform.position, radius);
        foreach(Collider2D collider in colliders)
        {
            if (collider.GetComponent<Enemy>() != null)
            {
                collider.GetComponent<Enemy>().FreezeTimeFor(duration);
            }
        }
        Debug.Log("FREEZE");
    }
}
