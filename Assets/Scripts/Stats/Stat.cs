
using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class Stat
{
    [SerializeField] private int baseValue;

    public List<int> modifiers = new List<int>();

    public int GetValue()
    {
        int finalValue = baseValue;
        for(int i = 0; i < modifiers.Count; i++)
        {
            finalValue += modifiers[i];
        }
        return finalValue;
    }
    public void SetDefaultValue(int  valueDefault)
    {
        baseValue = valueDefault;
    }
    public void AddModifier(int modifier)
    {
        modifiers.Add(modifier);
    }

    public void RemoveModifier(int modifier)
    {
        modifiers.Remove(modifier);
    }
}
