using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour, ISaveManager
{
    public static Inventory instance;

    public List<ItemData> startingItems = new List<ItemData>();

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
    [SerializeField] private Transform statSlotsParent;

    private UI_ItemSlot[] inventoryItemSlots;
    private UI_ItemSlot[] stashItemSlots;
    private UI_EquipmentSlot[] equipmentItemSlots;
    private UI_StatSlot[] statSlots;


    [Header("Item cooldown")]
    private float lastTimeUsedFlask;
    public float flaskCooldown;
    private float lastTimeUsedArmorEffect;
    private float armorEffectCooldown;

    [Header("Data base")]
    public List<InventoryItem> loadedItems;
    public List<ItemData_Equipment> loadedEquipment;
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
        statSlots = statSlotsParent.GetComponentsInChildren<UI_StatSlot>();
        AddStartingItems();

    }

    private void AddStartingItems()
    {
        if(loadedEquipment.Count > 0)
        {
            foreach(ItemData_Equipment item in loadedEquipment)
            {
                EquipItem(item);
            }
        }

        if (loadedItems.Count > 0)
        {
            foreach (InventoryItem item in loadedItems)
            {
                for(int i = 0; i < item.stackSize; i++)
                {
                    AddItem(item.itemData);
                }
            }
            return;
        }

        foreach (ItemData item in startingItems)
        {
            AddItem(item);
        }
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
            AddItem(oldEquipment);
        }

        newEquipment.AddModifiers();
        equipItems.Add(newItem);
        dictionaryEquip.Add(newEquipment, newItem);

        RemoveItem(_item);

        //UpdateSlotUI();
    }

    public void UnequipItem(ItemData_Equipment itemToRemove)
    {
        if (dictionaryEquip.TryGetValue(itemToRemove, out InventoryItem value))
        {
            itemToRemove.RemoveModifiers();
            equipItems.Remove(value);
            dictionaryEquip.Remove(itemToRemove);

            //UpdateSlotUI();
        }
    }

    public void UpdateSlotUI()
    {

        for (int i = 0; i < equipmentItemSlots.Length; i++)
        {

            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in dictionaryEquip)
            {
                if (item.Key.equipmentType == equipmentItemSlots[i].equipmentType)
                {
                    equipmentItemSlots[i].UpdateSlot(item.Value);
                }

            }
        }

        for (int i = 0; i < inventoryItemSlots.Length; i++)
        {
            inventoryItemSlots[i].CleanUpSlot();
        }

        for (int i = 0; i < stashItemSlots.Length; i++)
        {
            stashItemSlots[i].CleanUpSlot();
        }

        //for (int i = 0; i < equipmentItemSlots.Length; i++)
        //{
        //    equipmentItemSlots[i].CleanUpSlot();
        //}


        for (int i = 0; i < inventoryItems.Count; i++)
        {
            inventoryItemSlots[i].UpdateSlot(inventoryItems[i]);
        }

        for (int i = 0; i < stashItems.Count; i++)
        {
            stashItemSlots[i].UpdateSlot(stashItems[i]);
        }

        UpdatePlayerStatUI();
    }

    public void UpdatePlayerStatUI()
    {
        for (int i = 0; i < statSlots.Length; i++)
        {
            statSlots[i].UpdateStatValueUI();
        }
    }

    public bool CanAddItem()
    {

        if (inventoryItems.Count >= inventoryItemSlots.Length)
        {
            Debug.Log("no more space");
            return false;

        }

        return true;
    }

    public void AddItem(ItemData _item)
    {
        if (_item.itemType == ItemType.Equipment && CanAddItem())
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

    public void RemoveItem(ItemData _item)
    {
        if (_item.itemType == ItemType.Equipment)
        {
            if (dictionaryInventory.TryGetValue(_item, out InventoryItem value))
            {

                if (value.stackSize <= 1)
                {
                    dictionaryInventory.Remove(_item);
                    inventoryItems.Remove(value);
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

    public bool CanCraft(ItemData_Equipment itemToCraft, List<InventoryItem> requiredMaterials)
    {
        List<InventoryItem> materialsToRemove = new List<InventoryItem>();

        for (int i = 0; i < requiredMaterials.Count; i++)
        {
            if (dictionaryStash.TryGetValue(requiredMaterials[i].itemData, out InventoryItem stashValue))
            {
                if (stashValue.stackSize < requiredMaterials[i].stackSize)
                {
                    Debug.Log("not enough materials: " + requiredMaterials[i].itemData.name);
                    return false;
                }
                else
                {
                    InventoryItem itemToRemove = requiredMaterials[i];
                    itemToRemove.stackSize = requiredMaterials[i].stackSize;
                    materialsToRemove.Add(itemToRemove);
                }
            }
            else
            {
                return false;
            }
        }
        // remove item used
        foreach (InventoryItem materials in materialsToRemove)
        {
            for (int i = 0; i < materials.stackSize; i++)
            {
                RemoveItem(materials.itemData);
            }
        }

        AddItem(itemToCraft);

        return true;
    }

    public List<InventoryItem> GetEquipmentItems() => equipItems;
    public List<InventoryItem> GetStashItems() => stashItems;

    public ItemData_Equipment GetEquipment(EquipmentType _type)
    {
        //ItemData_Equipment equipedItem = null;
        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in dictionaryEquip)
        {
            if (item.Key.equipmentType == _type)
            {
                //equipedItem = item.Key;
                return item.Key;//
            }
        }

        return null;
    }

    public void RemoveItemEquipment(EquipmentType _type)
    {
        for (int i = 0; i < equipmentItemSlots.Length; i++)
        {
            if (_type == equipmentItemSlots[i].equipmentType)
            {
                equipmentItemSlots[i].CleanUpSlot();
            }
        }

    }

    public void UseFlask()
    {
        ItemData_Equipment currentFlask = GetEquipment(EquipmentType.Flask);
        if (currentFlask == null) return;

        if (Time.time > lastTimeUsedFlask + flaskCooldown)
        {
            currentFlask.ExecuteItemEffect(null);
            lastTimeUsedFlask = Time.time;
            flaskCooldown = currentFlask.itemCooldown;
        }
        else
        {
            //Debug.Log("Cooldown use: "+currentFlask.name);
        }
    }

    public bool CanUseArmorEffect()
    {
        ItemData_Equipment currentArmor = GetEquipment(EquipmentType.Armor);
        if (currentArmor == null) return false;

        if (Time.time > lastTimeUsedArmorEffect + armorEffectCooldown)
        {
            lastTimeUsedArmorEffect = Time.time;
            armorEffectCooldown = currentArmor.itemCooldown;
            return true;
        }
        else
        {
            //Debug.Log("Cooldown use: " + currentArmor.name);
            return false;
        }
    }

    public void LoadData(GameData gameData)
    {
        foreach(KeyValuePair<string,int> pair in gameData.inventory)
        {
            foreach(var item in GetItemDataBase())
            {
                if(item!=null&& item.itemID == pair.Key)
                {
                    InventoryItem itemToLoad = new InventoryItem(item);
                    itemToLoad.stackSize = pair.Value;
                    loadedItems.Add(itemToLoad);
                }
            }
        }

        foreach (string itemEquipmentID in gameData.equpmentsID)
        {
            foreach (var item in GetItemDataBase())
            {
                if (item != null && item.itemID == itemEquipmentID)
                {
                    loadedEquipment.Add(item as ItemData_Equipment);
                }
            }
        }
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.inventory.Clear();
        gameData.equpmentsID.Clear();

        foreach (KeyValuePair<ItemData, InventoryItem> pair in dictionaryInventory)
        {
            gameData.inventory.Add(pair.Key.itemID, pair.Value.stackSize);
        }

        foreach (KeyValuePair<ItemData, InventoryItem> pair in dictionaryStash)
        {
            gameData.inventory.Add(pair.Key.itemID, pair.Value.stackSize);
        }

        foreach(KeyValuePair<ItemData_Equipment,InventoryItem> pair in dictionaryEquip)
        {
            gameData.equpmentsID.Add(pair.Key.itemID);
        }
    }


    private List<ItemData> GetItemDataBase(){
        List<ItemData> itemDataBase = new List<ItemData>();
         string[] assetNames = AssetDatabase.FindAssets("", new[] { "Assets/Data/Items" });
        
        foreach(string SOName in assetNames)
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(SOName);
            var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(SOpath);
            itemDataBase.Add(itemData);
        }

        return itemDataBase;
    }

}
