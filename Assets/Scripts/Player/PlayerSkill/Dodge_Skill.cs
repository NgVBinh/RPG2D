using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dodge_Skill : Skill
{
    [Header("Dodge Skill")]
    [SerializeField] private UI_SkillTreeSlot dodgeSkill;
    [SerializeField] private int evasionAmount=10;
    public bool dodgeUnlocked;// { get; private set; }

    [Header("Dodge with Mirage")]
    [SerializeField] private UI_SkillTreeSlot dodgeMirageSkill;
    public bool dodgeMirageUnlocked;// { get; private set; }

    private void UnlockDodgeSkill()
    {
        if (dodgeUnlocked) return;
        if (dodgeSkill.unlockded)
        {
            dodgeUnlocked = true;
            player.characterStats.evasion.AddModifier(evasionAmount);
            Inventory.instance.UpdatePlayerStatUI();
        }
    }
    private void UnlockDodgeMirage()
    {
        if (dodgeMirageSkill.unlockded)
            dodgeMirageUnlocked = true;
    }
    protected override void Start()
    {
        base.Start();
        dodgeSkill.GetComponent<Button>().onClick.AddListener(UnlockDodgeSkill) ;
        dodgeMirageSkill.GetComponent<Button>().onClick.AddListener(UnlockDodgeMirage);
    }

    public void CreateMirageOnDodge()
    {
        if (dodgeMirageUnlocked)
            SkillManager.instance.cloneSkill.CreateClone(player.transform,new Vector2( 1.5f * player.facingDir,0));
    }
    public override void UseSkill()
    {
        base.UseSkill();
    }
    protected override void CheckUnlockSkill()
    {
        UnlockDodgeSkill();
        UnlockDodgeMirage();
    }
}
