using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private SwordType swordType = SwordType.Regular;
    [SerializeField] private GameObject swordThrowPref;
    [SerializeField] private Vector2 launchDir;
    [SerializeField] private float gravitySword;
    [SerializeField] private float freezeTime;
    [SerializeField] private float returnSpeed;

    private Vector2 finalDir;

    [Header("Aim Dots")]
    [SerializeField] private Transform dotsParent;
    [SerializeField] private GameObject dotPref;
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBetweenDots;
    private GameObject[] dots;

    [Header("Bounce infor")]
    [SerializeField] private int amountOfBounce;
    [SerializeField] private float gravityBounceSword;
    [SerializeField] private float speedBounce;
    [SerializeField] private float distanceCanBounce;


    [Header("Pierce infor")]
    [SerializeField] private int amountOfPierce;
    [SerializeField] private float gravityPierceSword;

    [Header("Spin infor")]
    [SerializeField] private float spinDuration;
    [SerializeField] private float maxTravelDistance;
    [SerializeField] private float gravitySpinSword;
    [SerializeField] private float hitCooldown;
    protected override void Start()
    {
        base.Start();
        GenerateDots();

        SetupGravity();
    }

    protected override void Update()
    {
        base.Update();
        finalDir = new Vector2(AimDirection().normalized.x * launchDir.x, AimDirection().normalized.y * launchDir.y);

        if (Input.GetMouseButton(1))
        {
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
        swordSkillScript.SetupSword(finalDir, gravitySword,freezeTime,returnSpeed);

        if (swordType == SwordType.Bounce)
        {
            swordSkillScript.SetupBounce(true, amountOfBounce,speedBounce,distanceCanBounce);
        }
        else if (swordType == SwordType.Pierce)
        {
            swordSkillScript.SetupPierce( amountOfPierce);
        }else if(swordType == SwordType.Spin)
        {
            swordSkillScript.SetupSpin(true, maxTravelDistance, spinDuration,hitCooldown);
        }


        player.AssignNewSword(newSword);
        ActiveDots(false);


    }

    private void SetupGravity()
    {
        if(swordType == SwordType.Bounce)
        {
            gravitySword = gravityBounceSword;
        }else if(swordType == SwordType.Pierce)
        {
            gravitySword = gravityPierceSword;
        }else if(swordType== SwordType.Spin)
        {
            gravitySword = gravitySpinSword;
        }
    }

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
