using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillTreeSlot : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public bool unlockded; //{  get; private set; }
    [SerializeField] private UI_SkillTreeSlot[] shoudBeUnlocked;
    [SerializeField] private UI_SkillTreeSlot[] shoudBeLocked;

    [SerializeField] private string skillName;
    [SerializeField] private int skillPrice;
    [TextArea]
    [SerializeField] private string skillDescription;
    [SerializeField] private Color lockSkillColor;
    private Image skillImage;
    private Button unlockButton;

    private UI ui;
    private void OnValidate()
    {
        gameObject.name ="SkillSlotUI - "+ skillName;
    }

    private void Awake()
    {
        unlockButton = GetComponent<Button>();
        unlockButton.onClick.AddListener(() => UnlockSkillSlot());
    }

    private void Start()
    {
        ui = GetComponentInParent<UI>();

        skillImage = GetComponent<Image>();
        skillImage.color = lockSkillColor;

    }

    public void UnlockSkillSlot()
    {
        if (unlockded) return;

        for(int i = 0; i < shoudBeUnlocked.Length; i++)
        {
            if (shoudBeUnlocked[i].unlockded == false)
            {
                Debug.Log("Cann't unlock skill by " + shoudBeUnlocked[i].name + " locked");
                return;
            }
        }

        for (int i = 0; i < shoudBeLocked.Length; i++)
        {
            if (shoudBeLocked[i].unlockded == true)
            {
                Debug.Log("Cann't unlock skill by " + shoudBeLocked[i].name + " unlocked");
                return;
            }
        }

        if (!PlayerManager.instance.HaveEnoughMoney(skillPrice))
            return;

        unlockded = true;
        Debug.Log(this.name + " unlock");
        skillImage.color = Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(skillDescription, skillName);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillToolTip.HideToolTip();

    }
}
