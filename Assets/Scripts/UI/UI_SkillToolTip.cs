using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_SkillToolTip : MonoBehaviour
{
    public TextMeshProUGUI skillDescriptionText;
    public TextMeshProUGUI skillName;

    public void ShowToolTip(string _skillDescription,string _skillName)
    {
        skillName.text = _skillName;
        skillDescriptionText.text = _skillDescription;

        Vector2 mousePos = Input.mousePosition;
        float xOffset = 0;
        if (mousePos.x < 500)
            xOffset = 300;
        else if(mousePos.x > 1000)
            xOffset = -300;

        gameObject.transform.position = new Vector2(mousePos.x + xOffset, mousePos.y + 150);

        gameObject.SetActive(true);
    }

    public void HideToolTip()=>gameObject.SetActive(false);
}
