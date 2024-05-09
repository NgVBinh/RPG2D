using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ice and Fire Effect", menuName = "Data/Item Effect/Ice and Fire")]

public class IceAndFire_Effect : ItemEffect
{
    [SerializeField] private GameObject iceAndFirePref;
    [SerializeField] private float xVelocity;
    public override void ExecuteEffect(Transform _respawTransform)
    {
        Player player = PlayerManager.instance.player;

        bool thirdAttack = player.attackState.comboCounter == 2;

        if (thirdAttack)
        {

        GameObject newIceAndFire = Instantiate(iceAndFirePref, _respawTransform.position, player.transform.rotation);
            newIceAndFire.GetComponent<Rigidbody2D>().velocity = new Vector2 (xVelocity*player.facingDir, 0);
        }
    }
}
