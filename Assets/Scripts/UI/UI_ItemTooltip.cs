using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ItemTooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemType;
    [SerializeField] private TextMeshProUGUI itemDescription;
    
    public void ShowTooltip(ItemData_Equipment equipment)
    {
        if(equipment == null) return;
        itemName.text = equipment.itemName;
        itemType.text = equipment.equipmentType.ToString();
        itemDescription.text = equipment.GetDescriptiom().ToString();
        gameObject.SetActive(true);
    }

    public void HideTooltip() =>gameObject.SetActive(false);
}
