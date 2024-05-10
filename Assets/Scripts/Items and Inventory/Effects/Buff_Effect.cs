using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    strength,
    agility,
    intelligence,
    vitality,

    damage,
    critChance,
    critPower,

    health,
    armor,
    evasion,
    magicResistance,

    fireDamage,
    iceDamage,
    lightingDamage,
}


[CreateAssetMenu(fileName = "Buff Effect", menuName = "Data/Item Effect/Buff Effect")]
public class Buff_Effect : ItemEffect
{
    [SerializeField] private int amountBuff;
    [SerializeField] private float buffDuration;
    [SerializeField] private StatType buffType;

    private PlayerStats playerStat;
    public override void ExecuteEffect(Transform _targetTransform)
    {
        playerStat = PlayerManager.instance.player.GetComponent<PlayerStats>();
        playerStat.IncreaseStatBy(amountBuff,buffDuration, StatToModify());
    }

    private Stat StatToModify()
    {
        if(buffType == StatType.strength) { return playerStat.strength; }
        else if(buffType == StatType.agility) { return playerStat.agility; }
        else if( buffType == StatType.intelligence) { return playerStat.intelligence; }
        else if(buffType== StatType.vitality) {  return playerStat.vitality; }

        else if(buffType== StatType.damage) { return playerStat.damage; }
        else if(buffType==StatType.critChance) { return playerStat.critChance; }
        else if(buffType == StatType.critPower) { return playerStat.critPower; }

        else if (buffType == StatType.health) { return playerStat.maxHealth; }
        else if (buffType == StatType.armor) { return playerStat.armor; }
        else if (buffType == StatType.evasion) { return playerStat.evasion; }
        else if (buffType == StatType.magicResistance) { return playerStat.magicResistance; }

        else if (buffType == StatType.fireDamage) { return playerStat.fireDamage; }
        else if (buffType == StatType.iceDamage) { return playerStat.iceDamage; }
        else if (buffType == StatType.lightingDamage) { return playerStat.lightingDamage; }

        return null;
    }
}
