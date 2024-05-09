using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderHit_Controller : MonoBehaviour
{
    private CharacterStats target;
    [SerializeField] private float moveSpeed;

    private int damage;

    private bool isTrigger;

    private Animator anim;

    public void SetupThunder(int damage, CharacterStats target)
    {
        this.damage = damage;
        this.target = target;

        anim = GetComponentInChildren<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (isTrigger) return;

        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);
        transform.up = transform.position - target.transform.position;
        if (Vector2.Distance(transform.position, target.transform.position) < 0.1f)
        {
            anim.SetTrigger("Hit");
            transform.localScale = new Vector3(3, 3, 3);
            transform.localRotation = Quaternion.identity;
            anim.transform.localPosition = new Vector3(-0.1f, 0.2f, 0);
            Invoke("DamageAndSelfDestroy", 0.15f);
            isTrigger = true;
        }
    }

    private void DamageAndSelfDestroy()
    {
        target.TakeDamage(damage);
        Destroy(gameObject, 0.4f);
    }
}
