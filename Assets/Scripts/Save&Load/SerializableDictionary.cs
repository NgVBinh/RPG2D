using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableDictionary<TKey,TValue> : Dictionary<TKey, TValue>,ISerializationCallbackReceiver
{
    [SerializeField] private List<TKey> keys = new List<TKey>();
    [SerializeField] private List<TValue> values = new List<TValue>();
    public void OnBeforeSerialize()
    {
        //Debug.LogWarning("call OnBeforeSerialize");

        keys.Clear();
        values.Clear();

        foreach(KeyValuePair<TKey, TValue> kvp in this)
        {
            keys.Add(kvp.Key);
            values.Add(kvp.Value);
        }
    }
    public void OnAfterDeserialize()
    {
        //Debug.LogWarning("call OnAfterDeserialize");

        this.Clear();
        if(keys.Count != values.Count) {
            Debug.LogWarning("keys count not equal values count");
        }

        for(int i = 0; i < keys.Count; i++)
        {
            this.Add(keys[i], values[i]);
        }
    }


    
}
