using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class GameData 
{
    public int currency;
    public SerializableDictionary<string, int> inventory;
    public List<string> equpmentsID;
    public SerializableDictionary<string, bool> skillTree;
    public GameData() {
        currency = 1000;
        inventory = new SerializableDictionary<string, int>();
        equpmentsID = new List<string>();    
        skillTree = new SerializableDictionary<string, bool>();
    }
    public GameData(int currency) {
        this.currency = currency;
        inventory = new SerializableDictionary<string, int>();
        equpmentsID = new List<string>();
        skillTree = new SerializableDictionary<string, bool>();
    }
}
