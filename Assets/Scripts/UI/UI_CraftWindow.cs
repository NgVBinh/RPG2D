using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftWindow : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Image[] materials;

    [SerializeField] private Button craftBtn;
    public void SetupCraftWindow(ItemData_Equipment _data)
    {
        for(int i = 0; i < materials.Length; i++)
        {
            materials[i].color = Color.clear;
            materials[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.clear;
        }

        for(int i = 0; i<_data.craftingMaterials.Count; i++)
        {
            if(_data.craftingMaterials.Count> materials.Length)
            {
                Debug.LogWarning("More slot material");
            }

            materials[i].sprite = _data.craftingMaterials[i].itemData.icon;
            materials[i].color = Color.white;

            TextMeshProUGUI slotMaterialText = materials[i].GetComponentInChildren<TextMeshProUGUI>();
            slotMaterialText.text = _data.craftingMaterials[i].stackSize.ToString();
            slotMaterialText.color = Color.white;
        }

        icon.sprite = _data.icon;
        nameText.text = _data.name;
        descriptionText.text = _data.GetDescriptiom().ToString();

        craftBtn.onClick.RemoveAllListeners();
        craftBtn.onClick.AddListener(()=>Inventory.instance.CanCraft(_data,_data.craftingMaterials));
    }

    //public void ClickBtnCraft(ItemData_Equipment equipmentData)
    //{
    //    if(Inventory.instance.CanCraft(equipmentData, equipmentData.craftingMaterials))
    //    {
    //        Debug.Log("craft success");
    //    }
    //}
}
