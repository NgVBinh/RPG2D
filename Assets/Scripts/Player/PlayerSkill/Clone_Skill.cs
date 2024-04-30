using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill : Skill
{
    [Header("CloneSkill infor")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [SerializeField] private bool canAttack;
   public void CreateClone(Transform positionClone,Vector3 offset)
    {
        GameObject  newClone = Instantiate(clonePrefab);
        newClone.GetComponent<Clone_Skill_Controller>().SetUpClone(positionClone,cloneDuration,canAttack,offset);
    }
}
