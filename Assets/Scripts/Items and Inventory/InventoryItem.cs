using System;

[Serializable]
public class InventoryItem
{
    public ItemData itemData;
    public int stackSize;
    public InventoryItem(ItemData _itemData)
    {
        itemData = _itemData;
        AddStack();
    }

    public void AddStack()=> stackSize++;
    public void RemoveStack()=> stackSize--;
}
