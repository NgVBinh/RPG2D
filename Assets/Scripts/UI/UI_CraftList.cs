using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftList : MonoBehaviour,IPointerDownHandler
{
    [SerializeField] private Transform craftSlotParent;
    [SerializeField] private GameObject craftSlotPrefab;

    [SerializeField] private List<ItemData_Equipment> crafEquipment;

    private void Start()
    {
        transform.parent.GetChild(0).GetComponent<UI_CraftList>().SetupCraftList();
        transform.parent.GetChild(0).GetComponent<UI_CraftList>().SetupDefaultCrafWindow();
    }

    public void SetupCraftList()
    {

        for(int i = 0; i < craftSlotParent.childCount; i++)
        {
            Destroy(craftSlotParent.GetChild(i).gameObject);
        }

        for(int i = 0;i<crafEquipment.Count; i++)
        {
            GameObject newSlotCraf = Instantiate(craftSlotPrefab,craftSlotParent);
            newSlotCraf.GetComponent<UI_CraftSlot>().SetupCraftSlot(crafEquipment[i]);
        }
    }

    public void SetupDefaultCrafWindow()
    {
        if (crafEquipment[0]!=null)
        GetComponentInParent<UI>()?.craftWindow.SetupCraftWindow(crafEquipment[0]);
       
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SetupCraftList();
        SetupDefaultCrafWindow();
    }
}
