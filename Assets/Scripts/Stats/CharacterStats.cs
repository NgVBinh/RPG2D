using System;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Major stats")]
    public Stat strength;    // 1 point increase damage by 1 and crit power by 1%
    public Stat agility;     // 1 point increase evasion by 1% and crit chance by 1%
    public Stat intelligence;// 1 point increase magic damage by 1 and magic resistance by 3
    public Stat vitality;    // 1 point increase health by 3 or 5 points

    [Header("Offensive stats")]
    public Stat damage;
    public Stat critChance;
    public Stat critPower;  // default 150 % damage 

    [Header("Defencive stats")]
    public Stat maxHealth;
    public Stat armor;
    public Stat evasion;     // 1 point increase evasion by 1%
    public Stat magicResistance;

    [Header("Magic stats")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightingDamage;

    public bool isIgnited; // does damage over time
    public bool isChilled; // reduce aromor 20%
    public bool isShocked; // reduce accuracy 20%

    [SerializeField] private float ailmentDuration;

    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;

    private int igniteDamage;
    [SerializeField] private float igniteDamageCooldown;
    private float igniteDamageTimer;

    private int shockDamage;
    [SerializeField] private GameObject shockTrikePrefab;

    public Action onHealthChanged;
    public bool isDead { get; private set; }

    //public int currentHealth { get; private set; }
    public int currentHealth;

    private EntityFX entityFX;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        entityFX = GetComponentInChildren<EntityFX>();

        currentHealth = GetMaxHealthValue();
        critPower.SetDefaultValue(150);
    }
    protected virtual void Update()
    {
        if (isIgnited)
        {
            ignitedTimer -= Time.deltaTime;
            if (ignitedTimer < 0) isIgnited = false;
            ApplyIgniteDamage();

        }
        if (isChilled)
        {
            chilledTimer -= Time.deltaTime;
            if (chilledTimer < 0) isChilled = false;
        }

        if (isShocked)
        {
            shockedTimer -= Time.deltaTime;
            if (shockedTimer < 0) isShocked = false;

        }


    }

    public virtual void DoDamage(CharacterStats target)
    {
        if (TargetCanAvoidAttack(target)) return;

        int totalDamage = damage.GetValue() + strength.GetValue();

        if (CanCrit()) totalDamage = CalculateCritialDamage(totalDamage);

        totalDamage = CheckTargetArmor(target, totalDamage);

        target.TakeDamage(totalDamage);
        //DoMagicalDamage(target);
    }

    public virtual void TakeDamage(int damage)
    {
        if (isDead) return;
        DecreaseHealth(damage);
        GetComponent<Entity>().BeDamagedEffect();
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void DecreaseHealth(int damage)
    {
        currentHealth -= damage;
        if (onHealthChanged != null)
        {
            onHealthChanged();
        }
    }

    public virtual void Die()
    {
        isDead = true;
        Debug.Log("die");
    }

    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + vitality.GetValue() * 5;
    }

    public virtual void DoMagicalDamage(CharacterStats target)
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightingDamage = lightingDamage.GetValue();

        int totalMagicDamage = _fireDamage + _iceDamage + _lightingDamage + intelligence.GetValue();

        // check target resistance magic
        totalMagicDamage -= target.magicResistance.GetValue() + (target.intelligence.GetValue() * 3);
        totalMagicDamage = Mathf.Clamp(totalMagicDamage, 0, int.MaxValue);

        target.TakeDamage(totalMagicDamage);

        ApplyAilmentToTarget(target, _fireDamage, _iceDamage, _lightingDamage);

    }

    #region Ailments
    private void ApplyAilmentToTarget(CharacterStats target, int _fireDamage, int _iceDamage, int _lightingDamage)
    {
        if (Mathf.Max(_fireDamage, _iceDamage, _lightingDamage) <= 0) return;

        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightingDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightingDamage;
        bool canApplyShock = _lightingDamage > _fireDamage && _lightingDamage > _iceDamage;

        if (canApplyIgnite)
        {
            target.ApplyIgniteAilment(true, _fireDamage);
            return;
        }
        else if (canApplyChill)
        {
            target.ApplyChillAilment(true);
            return;
        }
        else if (canApplyShock)
        {
            target.ApplyShockAilment(true, target,_lightingDamage);
            return;
        }


        // random ailment
        while (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            if (UnityEngine.Random.value < 0.33f && _fireDamage > 0)
            {
                target.ApplyIgniteAilment(true, _fireDamage);
                return;
            }
            if (UnityEngine.Random.value < 0.5f && _fireDamage > 0)
            {
                target.ApplyChillAilment(true);
                return;
            }

            if (_lightingDamage > 0)
            {
                target.ApplyShockAilment(true, target,_lightingDamage);
                return;
            }
        }
    }

    // public void SetIgniteDamage(int _igniteDamage) => igniteDamage = _igniteDamage;

    public void ApplyIgniteAilment(bool ignite, int fireDamage)
    {
        //if (isIgnited || isChilled || isShocked) return;

        if (ignite)
        {
            isIgnited = ignite;
            ignitedTimer = ailmentDuration;
            igniteDamageTimer = 0;
            igniteDamage = Mathf.RoundToInt(fireDamage * 0.2f);

            entityFX.igniteFXFor(ignitedTimer, igniteDamageCooldown);
        }
        else
        {
            isIgnited = false;
            ignitedTimer = 0;
            igniteDamageTimer = 0;
        }

    }

    public void ApplyChillAilment(bool chill)
    {
        //if (isIgnited || isChilled || isShocked) return;

        if (chill)
        {
            isChilled = chill;
            chilledTimer = ailmentDuration;
            entityFX.chillFXFor(chilledTimer, 0.3f);
            float slowPercent = 0.2f;
            GetComponent<Entity>().EntitySlowBy(slowPercent, chilledTimer);
        }
        else
        {
            isChilled = chill;
            chilledTimer = 0;
        }

    }

    public void ApplyShockAilment(bool shock, CharacterStats target,int lightDamage)
    {
        //if (isIgnited || isChilled || isShocked) return;

        if (shock)
        {
            isShocked = shock;
            shockedTimer = ailmentDuration;
            entityFX.shockFXFor(shockedTimer, 0.3f);
            shockDamage = Mathf.RoundToInt(lightDamage * 0.3f);

            // shock strike to target
            //GameObject shockStrike = Instantiate(shockTrikePrefab, target.transform.position, Quaternion.identity);
            //ThunderHit_Controller shockStrikeScript = shockStrike.GetComponent<ThunderHit_Controller>();
            //shockStrikeScript.SetupThunder(shockDamage, target);

            if (target.GetComponent<Player>()) return;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);
            float closestDistance = Mathf.Infinity;
            Transform nearEnemyTarget = null;

            foreach (Collider2D collider in colliders)
            {
                if (collider.GetComponent<Enemy>() != null)
                {
                    if (collider.GetComponent<CharacterStats>() != target)
                    {
                        Debug.Log(target.name);
                        float distanceToEnemy = Vector2.Distance(transform.position, collider.transform.position);
                        if (distanceToEnemy < closestDistance)
                        {
                            closestDistance = distanceToEnemy;
                            nearEnemyTarget = collider.transform;
                        }
                    }
                }
            }

            //shockStrikesToEnemyNear
            if (nearEnemyTarget != null)
            {
                GameObject shockStrikesToEnemyNear = Instantiate(shockTrikePrefab, transform.position, Quaternion.identity);
                ThunderHit_Controller thunderScripts = shockStrikesToEnemyNear.GetComponent<ThunderHit_Controller>();
                //CharacterStats enemyNear = nearEnemyTarget.GetComponent<CharacterStats>();
                thunderScripts.SetupThunder(shockDamage, nearEnemyTarget.GetComponent<CharacterStats>());
            }

        }
        else
        {
            isShocked = false;
            shockedTimer = 0;
        }

    }
    private void ApplyIgniteDamage()
    {
        igniteDamageTimer -= Time.deltaTime;
        if (igniteDamageTimer < 0 && isIgnited)
        {
            igniteDamageTimer = igniteDamageCooldown;
            Debug.Log("ignite damage" + igniteDamage);
            // take damage without effect
            DecreaseHealth(igniteDamage);
            if (currentHealth <= 0 && !isDead)
            {
                Die();
            }
        }
    }
    #endregion

    #region Stats Calculator
    private int CheckTargetArmor(CharacterStats target, int totalDamage)
    {
        if (target.isChilled)
            totalDamage -= Mathf.RoundToInt(target.armor.GetValue() * 0.8f);
        else
            totalDamage -= target.armor.GetValue();
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }

    private bool TargetCanAvoidAttack(CharacterStats target)
    {

        int totalEvasion = target.evasion.GetValue() + target.agility.GetValue();
        if (isShocked) totalEvasion += 20;
        if (UnityEngine.Random.Range(0, 100) < totalEvasion)
        {
            Debug.Log(target.name + " avoid attack");
            return true;
        }

        return false;
    }

    private bool CanCrit()
    {
        int totalCritChance = critChance.GetValue() + agility.GetValue();

        if (UnityEngine.Random.Range(0, 100) < totalCritChance)
        {
            return true;
        }

        return false;
    }

    private int CalculateCritialDamage(int damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * 0.01f;
        return Mathf.RoundToInt(damage * totalCritPower);
    }
    #endregion
}
