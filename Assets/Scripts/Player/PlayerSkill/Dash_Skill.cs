using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dash_Skill : Skill
{
    [Header("Dash")]
    [SerializeField] private UI_SkillTreeSlot dashSkill;
    public bool dashUnlocked;// { get;private set; }

    [Header("Clone on Dash")]
    [SerializeField] private UI_SkillTreeSlot cloneOnDashSkill;
    public bool cloneOnDashUnlocked { get; private set; }

    [Header("Clone on arrival")]
    [SerializeField] private UI_SkillTreeSlot cloneOnArrialSkill;
    public bool cloneOnArrialUnlocked { get; private set; }
    public override void UseSkill()
    {
        base.UseSkill();
        Debug.Log("create clone behind");
    }

    protected override void Start()
    {
        base.Start();
        dashSkill.GetComponent<Button>().onClick.AddListener(() => UnlockDash());
        cloneOnDashSkill.GetComponent<Button>().onClick.AddListener(UnlockCloneOnDash);
        cloneOnArrialSkill.GetComponent<Button>().onClick.AddListener(UnlockCloneOnArrival);
    }

    public void UnlockDash()
    {

        if (dashSkill.unlockded)
        {
            dashUnlocked = true;
        }
    }

    public void UnlockCloneOnDash()
    {
        if (cloneOnDashSkill.unlockded)
            cloneOnDashUnlocked = true;
    }

    public void UnlockCloneOnArrival()
    {
        if(cloneOnArrialSkill.unlockded)
        cloneOnArrialUnlocked = true;
    }

    public void CloneOnDash()
    {
        if (cloneOnDashUnlocked)
            SkillManager.instance.cloneSkill.CreateClone(player.transform, Vector2.zero);
    }

    public void CloneOnArrival()
    {
        if (cloneOnArrialUnlocked)
            SkillManager.instance.cloneSkill.CreateClone(player.transform, Vector2.zero);
    }
}
