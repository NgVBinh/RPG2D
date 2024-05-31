using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_StatTooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statDescription;

    public void ShowTooltip(string _description)
    {
        statDescription.text = _description;
        gameObject.SetActive(true);
    }

    public void HideTooltip()=> gameObject.SetActive(false);
}
