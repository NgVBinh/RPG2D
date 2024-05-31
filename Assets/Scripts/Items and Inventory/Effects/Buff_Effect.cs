using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        playerStat.IncreaseStatBy(amountBuff,buffDuration,playerStat.GetStat(buffType));
    }

    
}
