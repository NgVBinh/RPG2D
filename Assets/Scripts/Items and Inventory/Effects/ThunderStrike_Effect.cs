using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Thunder Strike Effect", menuName = "Data/Item Effect/Thunder Strike")]
public class ThunderStrike_Effect : ItemEffect
{
    [SerializeField] private GameObject thunderStrikeEffect;

    public override void ExecuteEffect(Transform _targetTransform)
    {
        //base.ExecuteEffect(_targetTransform);
        GameObject newThunderStrike = Instantiate(thunderStrikeEffect,_targetTransform.position,Quaternion.identity);
    }
}
