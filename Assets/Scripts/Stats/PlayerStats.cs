using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private Player player;
    protected override void Start()
    {
        base.Start();
        player = GetComponent<Player>();
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }

    public override void OnEvasion()
    {
        // funtion create mirage when player avoid
        base.OnEvasion();
        Debug.Log("Create mirage");
        player.skill.dodgeSkill.CreateMirageOnDodge();
    }

    public override void Die()
    {
        base.Die();
        player.Die();
        GetComponent<PlayerItemDrop>()?.GenerateDrop();
    }
    protected override void DecreaseHealth(int damage)
    {
        base.DecreaseHealth(damage);

        ItemData_Equipment currentArmor = Inventory.instance.GetEquipment(EquipmentType.Armor);
        if (currentArmor != null)
        {
            if( !Inventory.instance.CanUseArmorEffect()) return;
            currentArmor.ExecuteItemEffect(player.transform);
        }
    }

    public void CloneDoDamage(CharacterStats target, float multiplier)
    {
        if (TargetCanAvoidAttack(target)) return;

        int totalDamage = damage.GetValue() + strength.GetValue();

        if (multiplier != 0)
        {
            totalDamage =Mathf.RoundToInt( totalDamage* multiplier);
        }

        if (CanCrit()) totalDamage = CalculateCritialDamage(totalDamage);

        totalDamage = CheckTargetArmor(target, totalDamage);

        target.TakeDamage(totalDamage);
    }
}
