using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Equipmnt", menuName = "Data/Item Effect")]

public class ItemEffect : ScriptableObject
{
    public virtual void ExecuteEffect(Transform _targetTransform)
    {
        Debug.Log("Effect exxecuted!");
    }
}
