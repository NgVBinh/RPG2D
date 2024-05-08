using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public List<InventoryItem> inventoryItems;
    public Dictionary<ItemData, InventoryItem> dictionaryInventory;

    public List<InventoryItem> stashItems;
    public Dictionary<ItemData, InventoryItem> dictionaryStash;

    public List<InventoryItem> equipItems;
    public Dictionary<ItemData_Equipment, InventoryItem> dictionaryEquip;

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotsParent;
    [SerializeField] private Transform stashSlotsParent;
    [SerializeField] private Transform equipmentSlotsParent;

    [Header("Stash UI")]
    private UI_ItemSlot[] inventoryItemSlots;
    private UI_ItemSlot[] stashItemSlots;
    private UI_EquipmentSlot[] equipmentItemSlots;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        inventoryItems = new List<InventoryItem>();
        dictionaryInventory = new Dictionary<ItemData, InventoryItem>();

        stashItems = new List<InventoryItem>();
        dictionaryStash = new Dictionary<ItemData, InventoryItem>();

        equipItems = new List<InventoryItem>();
        dictionaryEquip = new Dictionary<ItemData_Equipment, InventoryItem>();

        inventoryItemSlots = inventorySlotsParent.GetComponentsInChildren<UI_ItemSlot>();
        stashItemSlots = stashSlotsParent.GetComponentsInChildren<UI_ItemSlot>();
        equipmentItemSlots = equipmentSlotsParent.GetComponentsInChildren<UI_EquipmentSlot>();

    }

    public void EquipItem(ItemData _item)
    {
        ItemData_Equipment newEquipment = _item as ItemData_Equipment;
        InventoryItem newItem = new InventoryItem(newEquipment);

        // Delete and then add equipment of the same type if any
        ItemData_Equipment oldEquipment = null;
        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in dictionaryEquip)
        {
            if (item.Key.equipmentType == newEquipment.equipmentType)
            {
                oldEquipment = item.Key;
            }
        }

        if (oldEquipment != null)
        {
            UnequipItem(oldEquipment);
            AddItemToInventory(oldEquipment);
        }

        newEquipment.AddModifiers();
        equipItems.Add(newItem);
        dictionaryEquip.Add(newEquipment, newItem);
        
        RemoveItem(_item);

        UpdateSlotUI();
    }

    private void UnequipItem(ItemData_Equipment itemToRemove)
    {
        if (dictionaryEquip.TryGetValue(itemToRemove, out InventoryItem value))
        {
            itemToRemove.RemoveModifiers();
            equipItems.Remove(value);
            dictionaryEquip.Remove(itemToRemove);
        }
    }

    private void UpdateSlotUI()
    {

        for (int i = 0; i < inventoryItemSlots.Length; i++)
        {
            inventoryItemSlots[i].CleanUpSlot();
        }

        for (int i = 0; i < stashItemSlots.Length; i++)
        {
            stashItemSlots[i].CleanUpSlot();
        }


        for(int i = 0; i <equipmentItemSlots.Length;i++) {
            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in dictionaryEquip)
            {
                if (item.Key.equipmentType == equipmentItemSlots[i].equipmentType)
                {
                    equipmentItemSlots[i].UpdateSlot(item.Value);
                }
            }
        }


        for (int i = 0; i < inventoryItems.Count; i++)
        {
            inventoryItemSlots[i].UpdateSlot(inventoryItems[i]);
        }

        for (int i = 0; i < stashItems.Count; i++)
        {
            stashItemSlots[i].UpdateSlot(stashItems[i]);
        }
    }

    public void AddItem(ItemData _item)
    {
        if (_item.itemType == ItemType.Equipment)
        {
            AddItemToInventory(_item);
        }
        else if (_item.itemType == ItemType.Material)
        {
            AddItemToStash(_item);
        }

        UpdateSlotUI();
    }

    private void AddItemToInventory(ItemData _item)
    {

        if (dictionaryInventory.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            dictionaryInventory.Add(_item, newItem);
            inventoryItems.Add(newItem);
        }


    }

    private void AddItemToStash(ItemData _item)
    {
        if (dictionaryStash.TryGetValue(_item, out InventoryItem stashValue))
        {
            stashValue.AddStack();
        }
        else
        {
            InventoryItem newStashItem = new InventoryItem(_item);
            dictionaryStash.Add(_item, newStashItem);
            stashItems.Add(newStashItem);
        }
    }

    private void RemoveItem(ItemData _item)
    {
        if (_item.itemType == ItemType.Equipment)
        {
            if (dictionaryInventory.TryGetValue(_item, out InventoryItem value))
            {

                if (value.stackSize <= 1)
                {
                    dictionaryInventory.Remove(_item);
                    inventoryItems.Remove(value);
                    Debug.Log("???");
                }
                else
                {
                    value.RemoveStack();
                }
            }
            else
            {
                Debug.LogWarning("Item not found");
            }
        }
        else if (_item.itemType == ItemType.Material)
        {
            if (dictionaryStash.TryGetValue(_item, out InventoryItem value))
            {
                if (value.stackSize <= 1)
                {
                    dictionaryStash.Remove(_item);
                    stashItems.Remove(value);
                }
                else
                {
                    value.RemoveStack();
                }
            }
            else
            {
                Debug.LogWarning("Item not found");
            }
        }

        UpdateSlotUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            ItemData itemRemove = inventoryItems[inventoryItems.Count - 1].itemData;
            RemoveItem(itemRemove);
        }
    }
}
