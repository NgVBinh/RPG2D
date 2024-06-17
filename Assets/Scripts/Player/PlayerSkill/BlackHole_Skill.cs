using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackHole_Skill : Skill
{
    [Space]
    [SerializeField] private GameObject blackHolePrefab;
    [SerializeField] private int maxSizeScale;
    [SerializeField] private float speedGrow;
    [SerializeField] private float speedShrink;
    [Space]
    [SerializeField] private int amountAttack;
    [SerializeField] private float cloneAttackCooldown;
    [SerializeField] private float blackholeDuration;

    protected BlackHole_Skill_Controller currentBlackhole;

    [Header("Blackhokd Skill")]
    [SerializeField] private UI_SkillTreeSlot blackholdSkill;
    public bool blackholdUnlocked;

    protected override void Start()
    {
        base.Start();
        blackholdSkill.GetComponent<Button>().onClick.AddListener(UnlockBlackhold);
    }
    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        GameObject newBlackhole = Instantiate(blackHolePrefab,player.transform.position,Quaternion.identity);
        currentBlackhole = newBlackhole.GetComponent<BlackHole_Skill_Controller>();
        currentBlackhole.SetupBlackHole(amountAttack, cloneAttackCooldown,maxSizeScale,speedGrow,speedShrink,blackholeDuration);
    }

    public bool SkillCompleted()
    {
        if (!currentBlackhole) return false;

        if (currentBlackhole.playerCanExitsState)
        {
            currentBlackhole = null;
            return true;
        }

        return false;
    }

    public float GetBlackholeRadius()
    {
        return (float)maxSizeScale / 2;
    }

    private void UnlockBlackhold()
    {
        if (blackholdSkill.unlockded)
        {
            blackholdUnlocked = true;
        }
    }

    protected override void CheckUnlockSkill()
    {
        UnlockBlackhold();
    }
}
