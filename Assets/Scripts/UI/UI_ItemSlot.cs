using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler
{
    public InventoryItem item;

    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI textAmountItem;


    public void CleanUpSlot()
    {
        item = null;
        image.color = Color.clear;
        image.sprite = null;
        textAmountItem.text = "";
    }

    public void UpdateSlot(InventoryItem _newitem)
    {
        image.color = Color.white;

        item = _newitem;
        if (item != null)
        {
            image.sprite = item.itemData.icon;
            if (item.stackSize > 1)
                textAmountItem.text = item.stackSize.ToString();
            else if (item.stackSize == 1) 
                textAmountItem.text = "";
            
        }

    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if(item.itemData.itemType == ItemType.Equipment)
        {
            Inventory.instance.EquipItem(item.itemData);
        }
    }
}
