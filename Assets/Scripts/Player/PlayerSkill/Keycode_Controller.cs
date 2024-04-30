using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Keycode_Controller : MonoBehaviour
{
    private KeyCode myKeyCode;
    private TextMeshProUGUI myText;

    private Transform enemyTransform;

    private SpriteRenderer sr;
    private BlackHole_Skill_Controller blackholeSkill;
    public void SetupKeycode(KeyCode newKeycode,Transform enemyTransform,BlackHole_Skill_Controller backholeSkill)
    {
        myKeyCode = newKeycode;
        this.enemyTransform = enemyTransform;
        this.blackholeSkill = backholeSkill;
        myText = GetComponentInChildren<TextMeshProUGUI>();
        myText.text = myKeyCode.ToString();

        sr = GetComponent<SpriteRenderer>();
    }


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(myKeyCode)) {
            blackholeSkill.AddEnemyTarget(enemyTransform);

            myText.color = Color.clear;
            sr.color = Color.clear;
        }
    }
}
