using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftSlot : UI_ItemSlot
{

    protected override void Start()
    {
        base.Start();
    }

    public void SetupCraftSlot(ItemData_Equipment _data)
    {
        if (_data == null) return;

        item.itemData = _data;
        imageItem.sprite = _data.icon;
        textItem.text = _data.name; 
    }

    private void OnEnable()
    {
        UpdateSlot(item);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        ItemData_Equipment craftData = item.itemData as ItemData_Equipment;

        //if (Inventory.instance.CanCraft(craftData,craftData.craftingMaterials))
        //{
        //    Debug.Log("craft success");
        //}
        UI.craftWindow.SetupCraftWindow(craftData);
        Inventory.instance.UpdateSlotUI();
    }
}
