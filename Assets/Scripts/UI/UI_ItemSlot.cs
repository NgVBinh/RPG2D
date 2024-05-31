using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public InventoryItem item;

    [SerializeField] protected Image imageItem;
    [SerializeField] protected TextMeshProUGUI textItem;

    protected UI UI;

    protected virtual void Start()
    {
        UI = GetComponentInParent<UI>();

    }
    public virtual void CleanUpSlot()
    {
        item = null;
        imageItem.color = Color.clear;
        imageItem.sprite = null;
        textItem.text = "";
    }

    public void UpdateSlot(InventoryItem _newitem)
    {
        imageItem.color = Color.white;

        item = _newitem;
        if (item != null)
        {
            imageItem.sprite = item.itemData.icon;
            if (item.stackSize > 1)
                textItem.text = item.stackSize.ToString();
            else if (item.stackSize == 1)
                textItem.text = "";

        }

    }


    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (item == null) return;
        UI.itemTooltip.HideTooltip();
        if (Input.GetKey(KeyCode.LeftControl))
        {
            Inventory.instance.RemoveItem(item.itemData);
            //Inventory.instance.UpdateSlotUI();

            return;
        }

        if (item.itemData.itemType == ItemType.Equipment)
        {
            Inventory.instance.EquipItem(item.itemData);
            //Inventory.instance.UpdateSlotUI();
            return;
        }

        UI.itemTooltip.HideTooltip();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null) return;
        UI.itemTooltip.ShowTooltip(item.itemData as ItemData_Equipment);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (item == null) return;

        UI.itemTooltip.HideTooltip();
    }
}
