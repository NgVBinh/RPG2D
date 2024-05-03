using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill : Skill
{
    [Header("CloneSkill infor")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [SerializeField] private bool canAttack;

    [Header("Clone Duplicate")]
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private int chanceToDuplicate;

    [Header("Crystal Insead Of Clone")]
    public bool crystalInseadOfClone;

    [Space]
    [SerializeField] private bool canCreateCloneOnStart;
    [SerializeField] private bool canCreateCloneOnOver;
    [SerializeField] private bool canCreateCloneOnCounter;
    public void CreateClone(Transform positionClone,Vector3 offset)
    {
        if(crystalInseadOfClone)
        {
            SkillManager.instance.crystalSkill.CreateCrystal();
            //SkillManager.instance.crystalSkill.CurrentCrystalChooseRandomTarget();
            return;
        }

        GameObject  newClone = Instantiate(clonePrefab);
        newClone.GetComponent<Clone_Skill_Controller>().SetUpClone(positionClone,cloneDuration,canAttack,offset,findClosestEnemy(player.transform),canDuplicateClone,chanceToDuplicate);
    }

    public void CreateCloneStart() {
        if(canCreateCloneOnStart)
        CreateClone(player.transform, Vector2.zero);
    }

    public void CreateCloneOver()
    {
        if (canCreateCloneOnOver)
            CreateClone(player.transform, Vector2.zero);
    }

    public void CreateCloneCounter(Transform enemyTransform)
    {
        if (canCreateCloneOnCounter)
            StartCoroutine(DelayCreateClone(enemyTransform,new Vector2(2*player.facingDir,0)));
    }

    private IEnumerator DelayCreateClone(Transform transformCreate,Vector2 offset)
    {
        yield return new WaitForSeconds(0.4f);
        CreateClone(transformCreate,offset);

    }
}
