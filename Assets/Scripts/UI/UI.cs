using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private GameObject settingUI;
    [SerializeField] private GameObject ingameUI;

    public UI_ItemTooltip itemTooltip;
    public UI_StatTooltip statTooltip;
    public UI_SkillToolTip skillToolTip;

    public UI_CraftWindow craftWindow;

    private void Awake()
    {
        SwitchTo(skillTreeUI);
    }

    private void Start()
    {
       // SwitchTo(skillTreeUI);
        SwitchTo(ingameUI);
    }

    public void SwitchTo(GameObject _menu)
    {

        for(int i=0;i<transform.childCount;i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        if(_menu != null)
        _menu.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (ingameUI.activeInHierarchy)
            {
                SwitchTo(characterUI);
            }
            else
            {
                SwitchTo(ingameUI);
            }
        }
    }
}
