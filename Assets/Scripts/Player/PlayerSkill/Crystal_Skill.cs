using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crystal_Skill : Skill
{
    [SerializeField] private GameObject crystalPrefab;

    [SerializeField] private float crystalDuration;

    [Header("Crystal simple")]
    [SerializeField] private UI_SkillTreeSlot crystalSkill;
    public bool crystalUnlocked ;//{ get;private set; }

    [Header("Crystal Mirage")]
    [SerializeField] private UI_SkillTreeSlot cloneInsteadCrystalSkill;
    [SerializeField] private bool cloneInsteadOfCrystal;

    [Header("crystal Explode")]
    [SerializeField] private UI_SkillTreeSlot crystalExplodeSkill;
    [SerializeField] private bool canExplode;

    [Header("crystal Move")]
    [SerializeField] private UI_SkillTreeSlot crystalMoveSkill;
    [SerializeField] private bool canMove;
    [SerializeField] private float moveSpeed;

    [Header("Multi stacking crystal")]
    [SerializeField] private UI_SkillTreeSlot crystalMultiStackSkill;
    [SerializeField] private bool canUseMultiStack;
    [SerializeField] private int amountOfStacks;
    [SerializeField] private float multiStackCooldown;
    [SerializeField] private float useTimeWindow;
    [SerializeField] private List<GameObject> crystalLeft = new List<GameObject>();

    private GameObject currentCrystal;
    #region Unlock Skill
    private void UnlockCrystalSkill()
    {

        if (crystalSkill.unlockded)
        {
            crystalUnlocked = true;
            Debug.Log("aaa");
        }
    }
    private void UnlockCrystalMirageSkill()
    {
        if (cloneInsteadCrystalSkill.unlockded)
            cloneInsteadOfCrystal = true;
    }

    private void UnlockCrystalExplpodeSkill()
    {
        if (crystalExplodeSkill.unlockded)
            canExplode = true;
    }
    private void UnlockCrystalMoveSkill()
    {
        if (crystalMoveSkill.unlockded)
            canMove = true;
    }

    private void UnlockCrystalMultiStackSkill()
    {
        if (crystalMultiStackSkill.unlockded)
            canUseMultiStack = true;
    }
    #endregion
    protected override void Start()
    {
        base.Start();
        crystalSkill.GetComponent<Button>().onClick.AddListener(UnlockCrystalSkill);
        cloneInsteadCrystalSkill.GetComponent<Button>().onClick.AddListener(UnlockCrystalMirageSkill);
        crystalExplodeSkill.GetComponent<Button>().onClick.AddListener(UnlockCrystalExplpodeSkill);
        crystalMoveSkill.GetComponent<Button>().onClick.AddListener(UnlockCrystalMoveSkill);
        crystalMultiStackSkill.GetComponent<Button>().onClick.AddListener(UnlockCrystalMultiStackSkill);

    }
    public override void UseSkill()
    {
        base.UseSkill();

        if (CanUseMultiCrystal()) return;

        if (currentCrystal == null)
        {
            CreateCrystal();
        }
        else
        {
            if (canMove) { return; }
            // swap crystal with player
            Vector2 playerPos = player.transform.position;
            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPos;

            if (cloneInsteadOfCrystal)
            {
                player.skill.cloneSkill.CreateClone(currentCrystal.transform, Vector2.zero);
                Destroy(currentCrystal);
            }
            else
            {
                currentCrystal.GetComponent<Crystal_Skill_Controller>()?.FinishCrystal();
            }

        }
    }

    public void CreateCrystal()
    {
        currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
        Crystal_Skill_Controller crystalScript = currentCrystal.GetComponent<Crystal_Skill_Controller>();
        crystalScript.SetupCrystal(crystalDuration, canExplode, canMove, moveSpeed, findClosestEnemy(currentCrystal.transform),player);
    }

    public void CurrentCrystalChooseRandomTarget() => currentCrystal.GetComponent<Crystal_Skill_Controller>().ChooseRandomEnemy();

    private bool CanUseMultiCrystal()
    {
        if (canUseMultiStack)
        {
            if (crystalLeft.Count > 0)
            {
                if (crystalLeft.Count == amountOfStacks)
                    Invoke("ResetAbility", useTimeWindow);

                coolDown = 0;
                GameObject crystalToSpaw = crystalLeft[crystalLeft.Count - 1];
                GameObject newCrystal = Instantiate(crystalToSpaw, player.transform.position, Quaternion.identity);

                crystalLeft.Remove(crystalToSpaw);

                newCrystal.GetComponent<Crystal_Skill_Controller>().
                    SetupCrystal(crystalDuration, canExplode, canMove, moveSpeed, findClosestEnemy(newCrystal.transform), player);

                if (crystalLeft.Count <= 0)
                {
                    //cooldown, refil
                    coolDown = multiStackCooldown;
                    RefilCrystal();
                }
                return true;
            }
        }

        return false;
    }

    private void RefilCrystal()
    {
        int amountToAdd = amountOfStacks - crystalLeft.Count;
        for (int i = 0; i < amountToAdd; i++)
        {
            crystalLeft.Add(crystalPrefab);
        }
    }

    private void ResetAbility()
    {
        //if (coolDownTimer > 0) return;

        coolDownTimer = multiStackCooldown;
        RefilCrystal();
    }

    protected override void CheckUnlockSkill()
    {
        Debug.Log("???????");
        UnlockCrystalSkill();
        UnlockCrystalMirageSkill();
        UnlockCrystalExplpodeSkill();
        UnlockCrystalMoveSkill();
        UnlockCrystalMultiStackSkill();
    }
}
