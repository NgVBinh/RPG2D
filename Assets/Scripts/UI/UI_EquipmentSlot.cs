using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EquipmentSlot : UI_ItemSlot
{
    public EquipmentType equipmentType;

   
    private void OnValidate()
    {
        gameObject.name ="Equipmet type: "+ equipmentType.ToString();
    }

    public override void CleanUpSlot()
    {
        base.CleanUpSlot();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        Inventory.instance.UnequipItem(item.itemData as ItemData_Equipment);
        Inventory.instance.AddItem(item.itemData as ItemData_Equipment);

        CleanUpSlot();
    }
}
