using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

public enum ItemType
{
    Material,
    Equipment
}


[CreateAssetMenu(fileName ="New item data",menuName ="Data/Item")]
public class ItemData : ScriptableObject
{
    public string itemID;
    public ItemType itemType;
    public string itemName;
    public Sprite icon;

    [Range(0, 100)]
    public int dropChance;

    protected StringBuilder sb = new StringBuilder();

    public virtual string GetDescriptiom()
    {
        return "";
    }

    private void OnValidate()
    {
#if UNITY_EDITOR
        string path = AssetDatabase.GetAssetPath(this);
        itemID = AssetDatabase.AssetPathToGUID(path);
#endif
    }
}
