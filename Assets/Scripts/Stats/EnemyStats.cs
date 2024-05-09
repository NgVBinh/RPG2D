using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy enemy;

    [Header("Level detail")]
    [SerializeField] private int level = 1;

    [Range(0f, 1f)]
    [SerializeField] private float percentageModifier = 0.4f;

    private ItemDrop mySystemDrop;
    protected override void Start()
    {
        ApplyModifiers();
        base.Start();
        enemy = GetComponent<Enemy>();
        mySystemDrop = GetComponent<ItemDrop>();
    }

    private void ApplyModifiers()
    {
        Modifier(strength);
        Modifier(agility);
        Modifier(intelligence);
        Modifier(vitality);

        Modifier(damage);
        Modifier(critChance);
        Modifier(critPower);

        Modifier(maxHealth);
        Modifier(armor);
        Modifier(evasion);
        Modifier(magicResistance);

        Modifier(fireDamage);
        Modifier(iceDamage);
        Modifier(lightingDamage);
    }

    private void Modifier(Stat _stats)
    {
        for(int i = 0; i < level; i++)
        {
            float modifier = _stats.GetValue() * percentageModifier;
            _stats.AddModifier(Mathf.RoundToInt(modifier));
        }
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }

    public override void Die()
    {
        base.Die();
        enemy.Die();

        if (mySystemDrop != null)
            mySystemDrop.GenerateDrop();
    }
}
