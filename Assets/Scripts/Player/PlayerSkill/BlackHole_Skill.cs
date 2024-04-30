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
    [SerializeField] private float blackholdDuration;

    protected BlackHole_Skill_Controller currentBlackhold;
    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        GameObject newBlackhole = Instantiate(blackHolePrefab,player.transform.position,Quaternion.identity);
        currentBlackhold = newBlackhole.GetComponent<BlackHole_Skill_Controller>();
        currentBlackhold.SetupBlackHole(amountAttack, cloneAttackCooldown,maxSizeScale,speedGrow,speedShrink,blackholdDuration);
    }

    public bool SkillCompleted()
    {
        if (!currentBlackhold) return false;

        if (currentBlackhold.playerCanExitsState)
        {
            currentBlackhold = null;
            return true;
        }

        return false;
    }
}
