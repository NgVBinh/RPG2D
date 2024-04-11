using UnityEngine;

public class Enemy_SkeletonAnimationTriggers : MonoBehaviour
{
    private Skeleton_Enemy skeleton => GetComponentInParent<Skeleton_Enemy>();

    public void AnimationTrigger()
    {
        skeleton.AnimationFinishTrigger();
    }
}
