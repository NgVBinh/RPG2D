using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Parry_Skill : Skill
{
    [Header("Parry Skill")]
    [SerializeField] private UI_SkillTreeSlot parrySkill;
    public bool parryUnlocked { get;private set; }

    [Header("Parry Restore")]
    [SerializeField] private UI_SkillTreeSlot parryRestoreSkill;
    public bool parryRestoreUnlocked { get;private set; }
    [Range(0f,1f)]
    [SerializeField] private float parryRestorePercent;


    [Header("Parry with Mirage")]
    [SerializeField] private UI_SkillTreeSlot parryWithMirageSkill;
    public bool parryWithMirageUnlocked { get; private set; }

    protected override void Start()
    {
        base.Start();
        parrySkill.GetComponent<Button>().onClick.AddListener(UnlockParrySkill);
        parryRestoreSkill.GetComponent<Button>().onClick.AddListener(UnlockParryRestoreSkill);
        parryWithMirageSkill.GetComponent<Button>().onClick.AddListener(UnlockParryWithMirageSkill);
    }
    public override void UseSkill()
    {
        base.UseSkill();
        if (parryRestoreUnlocked)
        {
            int restoreAmount =Mathf.RoundToInt(player.characterStats.GetMaxHealthValue()*parryRestorePercent);
            player.characterStats.IncreaseHealthBy(restoreAmount);
        }
    }

    public void UnlockParrySkill()
    {
        if (parrySkill.unlockded)
            parryUnlocked = true;
    }

    public void UnlockParryRestoreSkill()
    {
        if (parryRestoreSkill.unlockded)
            parryRestoreUnlocked = true;
    }

    public void UnlockParryWithMirageSkill()
    {
        if (parryWithMirageSkill.unlockded)
            parryWithMirageUnlocked = true;
    }

    public void MakeMirageOnParry(Transform spawnTransform)
    {
        if (parryWithMirageUnlocked)
        {
            SkillManager.instance.cloneSkill.CreateCloneWithDelay(spawnTransform);
        }
    }
}
