using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal Effect", menuName = "Data/Item Effect/Heal Effect")]

public class Heal_Effect : ItemEffect
{
    [Range(0f, 1f)]
    [SerializeField] private float healPresent;
    public override void ExecuteEffect(Transform _targetTransform)
    {
        PlayerStats playerStat = PlayerManager.instance.player.GetComponent<PlayerStats>();
        int healAmount =Mathf.RoundToInt(playerStat.GetMaxHealthValue()* healPresent);

        playerStat.IncreaseHealthBy(healAmount);
    }
}
