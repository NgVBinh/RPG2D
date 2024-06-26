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
                EnemyStats target = collider.GetComponent<EnemyStats>();
                ExecuteEffectEquipment(target);
                player.characterStats.DoDamage(target);
            }
        }
    }

    private void ExecuteEffectEquipment(EnemyStats target)
    {
        ItemData_Equipment swordEquipment = Inventory.instance.GetEquipment(EquipmentType.Sword);
        if (swordEquipment != null)
        {
            swordEquipment.ExecuteItemEffect(target.transform);
        }
        //ItemData_Equipment armorEquipment = Inventory.instance.GetEquipment(EquipmentType.Armor);
        //if (armorEquipment != null)
        //{
        //    armorEquipment.ExecuteItemEffect(target.transform);
        //}
        ItemData_Equipment amuletEquipment = Inventory.instance.GetEquipment(EquipmentType.Amulet);
        if (amuletEquipment != null)
        {
            amuletEquipment.ExecuteItemEffect(target.transform);
        }
    }

    private void ThrowSword()
    {
        SkillManager.instance.swordSkill.CreateSword();
    }
}
