using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clone_Skill : Skill
{
    [Header("CloneSkill infor")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    private float attackMultiplier;

    [Header("Clone Skill")]
    [SerializeField] private UI_SkillTreeSlot cloneSkill;
    [SerializeField] private float cloneAttackMultiplier;
    [SerializeField] private bool canAttack;

    [Header("Aggresive Clone")]
    [SerializeField] private UI_SkillTreeSlot aggresiveCloneSkill;
    [SerializeField] private float aggresiveCloneAttackMultiplier;
    public bool aggresiveCloneUnlock { get; private set; }


    [Header("Multiple Clone")]
    [SerializeField] private UI_SkillTreeSlot multiCloneSkill;
    [SerializeField] private float multiCloneAttackMultiplier;
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private int chanceToDuplicate;

    [Header("Crystal Insead Of Clone")]
    [SerializeField] private UI_SkillTreeSlot CrystalInsteadCloneSkill;
    public bool crystalInseadOfClone;

    protected override void Start()
    {
        base.Start();
        cloneSkill.GetComponent<Button>().onClick.AddListener(UnlockCloneSkill);
        aggresiveCloneSkill.GetComponent<Button>().onClick.AddListener(UnlockAggresiveCloneSkill);
        multiCloneSkill.GetComponent<Button>().onClick.AddListener(UnlockMultiCloneSkill);
        CrystalInsteadCloneSkill.GetComponent<Button>().onClick.AddListener(UnlockCrystalInsteadCloneSkill);
    }

    public void CreateClone(Transform positionClone, Vector3 offset)
    {
        if (crystalInseadOfClone)
        {
            SkillManager.instance.crystalSkill.CreateCrystal();
            //SkillManager.instance.crystalSkill.CurrentCrystalChooseRandomTarget();
            return;
        }

        GameObject newClone = Instantiate(clonePrefab);
        newClone.GetComponent<Clone_Skill_Controller>().SetUpClone(positionClone, cloneDuration, canAttack,attackMultiplier, offset, findClosestEnemy(player.transform), canDuplicateClone, chanceToDuplicate, player);
    }


    public void CreateCloneWithDelay(Transform enemyTransform)
    {
        StartCoroutine(CloneDelayCoroutine(enemyTransform, new Vector2(2 * player.facingDir, 0)));
    }

    private IEnumerator CloneDelayCoroutine(Transform transformCreate, Vector2 offset)
    {
        yield return new WaitForSeconds(0.4f);
        CreateClone(transformCreate, offset);

    }

    #region Unlock Skill

    private void UnlockCloneSkill()
    {
        if (cloneSkill.unlockded)
        {
            attackMultiplier = cloneAttackMultiplier;
            canAttack = true;
        }
    }

    private void UnlockAggresiveCloneSkill()
    {
        if (aggresiveCloneSkill.unlockded)
        {
            aggresiveCloneUnlock = true;
            attackMultiplier = aggresiveCloneAttackMultiplier;
        }
    }

    private void UnlockCrystalInsteadCloneSkill()
    {
        if (CrystalInsteadCloneSkill.unlockded)
        {
            crystalInseadOfClone = true;
        }
    }

    private void UnlockMultiCloneSkill()
    {
        if (multiCloneSkill.unlockded)
        {
            canDuplicateClone = true;
            attackMultiplier = multiCloneAttackMultiplier;
        }
    }

    protected override void CheckUnlockSkill()
    {
        UnlockCloneSkill();
        UnlockAggresiveCloneSkill();
        UnlockCrystalInsteadCloneSkill();
        UnlockMultiCloneSkill();
    }
    #endregion
}
