using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] protected float coolDown;
    protected float coolDownTimer;

    // Update is called once per frame
    protected virtual void Update()
    {
        coolDownTimer -= Time.deltaTime;
    }

    public virtual bool CanUseSkill()
    {
        if(coolDownTimer < 0)
        {
            coolDownTimer = coolDown;
            UseSkill();
            return true;
        }

        Debug.Log("Skill is on cool down");
        return false;
    }

    public virtual void UseSkill()
    {
        // do something

    }
}
