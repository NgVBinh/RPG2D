using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemDrop : ItemDrop
{
    [SerializeField] private int ChanceToLooseEquipment;
    [SerializeField] private int ChanceToLooseMaterial;
    public override void GenerateDrop()
    {
        //base.GenerateDrop();
        Inventory inventory = Inventory.instance;

        List<InventoryItem> itemsToUnequip = new List<InventoryItem>();

        foreach (InventoryItem item in inventory.GetEquipmentItems())
        {
            if (Random.Range(0, 100) < ChanceToLooseEquipment)
            {
                DropItem(item.itemData);
                itemsToUnequip.Add(item);
            }
        }

        foreach(InventoryItem item in itemsToUnequip)
        {
            inventory.UnequipItem(item.itemData as ItemData_Equipment);
        }


        List<InventoryItem> materialsToLoose = new List<InventoryItem>();

        foreach (InventoryItem item in inventory.GetStashItems())
        {
            if (Random.Range(0, 100) < ChanceToLooseMaterial)
            {
                DropItem(item.itemData);
                materialsToLoose.Add(item);
            }
        }

        foreach (InventoryItem item in materialsToLoose)
        {
            inventory.RemoveItem(item.itemData);
        }

        inventory.UpdateSlotUI();
    }
}
