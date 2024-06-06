using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SwordType
{
    Regular,
    Bounce,
    Pierce,
    Spin
}

public class Sword_Skill : Skill
{
    [Header("Sword infor")]
    [SerializeField] private UI_SkillTreeSlot swordSkill;
    public bool swordUnlocked { get; private set; }
    [SerializeField] private SwordType swordType = SwordType.Regular;
    [SerializeField] private GameObject swordThrowPref;
    [SerializeField] private Vector2 launchDir;
    [SerializeField] private float gravitySword;
    [SerializeField] private float freezeTime;
    [SerializeField] private float returnSpeed;


    [Header("Bounce infor")]
    [SerializeField] private UI_SkillTreeSlot swordBounceSkill;
    [SerializeField] private int amountOfBounce;
    [SerializeField] private float gravityBounceSword;
    [SerializeField] private float speedBounce;
    [SerializeField] private float distanceCanBounce;

    [Header("Pierce infor")]
    [SerializeField] private UI_SkillTreeSlot swordPierceSkill;
    [SerializeField] private int amountOfPierce;
    [SerializeField] private float gravityPierceSword;

    [Header("Spin infor")]
    [SerializeField] private UI_SkillTreeSlot swordSpinSkill;
    [SerializeField] private float spinDuration;
    [SerializeField] private float maxTravelDistance;
    [SerializeField] private float gravitySpinSword;
    [SerializeField] private float hitCooldown;

    [Header("Passive skills")]
    [SerializeField] private UI_SkillTreeSlot timeStopSkill;
    public bool timeStopUnlocked { get; private set; }
    [SerializeField] private UI_SkillTreeSlot vulnerableSkill;
    public bool vulnerableUnlocked { get; private set; }

    private Vector2 finalDir;
    [Header("Aim Dots")]
    [SerializeField] private Transform dotsParent;
    [SerializeField] private GameObject dotPref;
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBetweenDots;
    private GameObject[] dots;
    protected override void Start()
    {
        base.Start();

        swordSkill.GetComponent<Button>().onClick.AddListener(UnlockSwordSkill);
        swordSpinSkill.GetComponent<Button>().onClick.AddListener(UnlockSpinSwordSkill);
        swordBounceSkill.GetComponent<Button>().onClick.AddListener(UnlockBouncySwordSkill);
        swordPierceSkill.GetComponent<Button>().onClick.AddListener(UnlockPierceSwordSkill);
        timeStopSkill.GetComponent<Button>().onClick.AddListener(UnlockTimeStopSkill);
        vulnerableSkill.GetComponent<Button>().onClick.AddListener(UnlockVulnerableSkill);

        GenerateDots();
        SetupGravity();
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetMouseButton(1) && swordUnlocked)
        {
            finalDir = new Vector2(AimDirection().normalized.x * launchDir.x, AimDirection().normalized.y * launchDir.y);
            for (int i = 0; i < numberOfDots; i++)
            {
                dots[i].transform.position = DotPosition(i * spaceBetweenDots);
            }

        }

    }
    public void CreateSword()
    {
        GameObject newSword = Instantiate(swordThrowPref, player.transform.position, transform.rotation);

        Sword_Skill_Controller swordSkillScript = newSword.GetComponent<Sword_Skill_Controller>();
        swordSkillScript.SetupSword(finalDir, gravitySword, freezeTime, returnSpeed);

        if (swordType == SwordType.Bounce)
        {
            swordSkillScript.SetupBounce(true, amountOfBounce, speedBounce, distanceCanBounce);
        }
        else if (swordType == SwordType.Pierce)
        {
            swordSkillScript.SetupPierce(amountOfPierce);
        }
        else if (swordType == SwordType.Spin)
        {
            swordSkillScript.SetupSpin(true, maxTravelDistance, spinDuration, hitCooldown);
        }


        player.AssignNewSword(newSword);
        ActiveDots(false);


    }

    private void SetupGravity()
    {
        if (swordType == SwordType.Bounce)
        {
            gravitySword = gravityBounceSword;
        }
        else if (swordType == SwordType.Pierce)
        {
            gravitySword = gravityPierceSword;
        }
        else if (swordType == SwordType.Spin)
        {
            gravitySword = gravitySpinSword;
        }
    }

    #region unlock skill
    private void UnlockSwordSkill()
    {
        if (swordSkill.unlockded)
        {
            swordType = SwordType.Regular;
            swordUnlocked = true;
        }
    }

    private void UnlockBouncySwordSkill()
    {
        if (swordBounceSkill.unlockded)
        {
            swordType = SwordType.Bounce;
        }
    }

    private void UnlockPierceSwordSkill()
    {
        if (swordPierceSkill.unlockded)
        {
            swordType = SwordType.Pierce;
        }
    }
    private void UnlockSpinSwordSkill()
    {
        if (swordSpinSkill.unlockded)
        {
            swordType = SwordType.Spin;
        }
    }

    private void UnlockTimeStopSkill()
    {
        if(timeStopSkill.unlockded)
            timeStopUnlocked = true;
    }

    private void UnlockVulnerableSkill()
    {
        if (vulnerableSkill.unlockded)
            vulnerableUnlocked = true;
    }
    #endregion

    #region AimSword
    private Vector2 AimDirection()
    {
        Vector2 playerPos = (Vector2)player.transform.position;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return mousePos - playerPos;

    }

    private void GenerateDots()
    {
        dots = new GameObject[numberOfDots];
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i] = Instantiate(dotPref, player.transform.position, Quaternion.identity, dotsParent);
        }
    }

    public void ActiveDots(bool isActive)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(isActive);
        }
    }

    private Vector2 DotPosition(float t)
    {
        return (Vector2)player.transform.position + finalDir * t + 0.5f * (Physics2D.gravity * gravitySword) * (t * t);
    }

    #endregion


}
