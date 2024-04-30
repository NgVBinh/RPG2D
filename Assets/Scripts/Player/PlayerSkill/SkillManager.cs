using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    public Dash_Skill dashSkill {  get; private set; }
    public Clone_Skill cloneSkill { get; private set; }
    public Sword_Skill swordSkill { get; private set; }
    public BlackHole_Skill blackholeSkill { get; private set; }
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }

    }

    private void Start()
    {
        dashSkill = GetComponent<Dash_Skill>();
        cloneSkill = GetComponent<Clone_Skill>();
        swordSkill = GetComponent<Sword_Skill>();
        blackholeSkill = GetComponent<BlackHole_Skill>();
    }
}
