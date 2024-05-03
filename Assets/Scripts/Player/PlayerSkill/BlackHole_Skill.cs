using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
