
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EquipmentType
{
    Sword,
    Armor,
    Amulet,
    Flask
}

[CreateAssetMenu(fileName ="Item Equipmnt",menuName = "Data/Equipment") ]
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;

    [Header("Equipment Item")]
    public float itemCooldown;
    public ItemEffect[] itemEffects;
    [TextArea]
    public string itemEffectDescription;

    [Header("Craft Requirement")]
    public List<InventoryItem> craftingMaterials;

    [Header("Major stats")]
    public int strength;    // 1 point increase damage by 1 and crit power by 1%
    public int agility;     // 1 point increase evasion by 1% and crit chance by 1%
    public int intelligence;// 1 point increase magic damage by 1 and magic resistance by 3
    public int vitality;    // 1 point increase health by 3 or 5 points

    [Header("Offensive stats")]
    public int damage;
    public int critChance;
    public int critPower;  // default 150 % damage 

    [Header("Defencive stats")]
    public int health;
    public int evasion;     // 1 point increase evasion by 1%
    public int armor;
    public int magicResistance;

    [Header("Magic stats")]
    public int fireDamage;
    public int iceDamage;
    public int lightingDamage;

    private int description;
    public void ExecuteItemEffect(Transform _targetTransform)
    {
        foreach(var item in itemEffects)
        {
            item.ExecuteEffect(_targetTransform);
        }
    }

    public override string GetDescriptiom()
    {
        sb.Length = 0;
        description = 0;
        AddItemDescription(strength, "Strength");
        AddItemDescription(agility, "Agility");
        AddItemDescription(intelligence, "Intelligence");
        AddItemDescription(vitality, "Vitality");

        AddItemDescription(damage, "Damage");
        AddItemDescription(critChance, "Crit Chance");
        AddItemDescription(critPower, "Crit Power");

        AddItemDescription(health, "Health");
        AddItemDescription(evasion, "Evasion");
        AddItemDescription(armor, "Armor");
        AddItemDescription(magicResistance, "MagicResistance");

        AddItemDescription(fireDamage, "Fire damage");
        AddItemDescription(iceDamage, "Ice damage");
        AddItemDescription(lightingDamage, "Lighting damage");

        for(int i = 0; i < 5 - description; i++)
        {
            sb.AppendLine();
            sb.Append("");
        }
        if(itemEffectDescription.Length > 0)
        {
            sb.AppendLine();
            sb.Append(itemEffectDescription);
        }
        return sb.ToString();
    }

    private void AddItemDescription(int _value,string _name)
    {
        if (_value != 0)
            if (sb.Length > 0)
                sb.AppendLine();
        if(_value > 0)
            sb.Append("+ "+_value+" "+_name);

        description++;
    }

    public void AddModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.AddModifier(strength);
        playerStats.agility.AddModifier(agility);
        playerStats.intelligence.AddModifier(intelligence);
        playerStats.vitality.AddModifier(vitality);

        playerStats.damage.AddModifier(damage);
        playerStats.critChance.AddModifier(critChance);
        playerStats.critPower.AddModifier(critPower);

        playerStats.maxHealth.AddModifier(health);
        playerStats.evasion.AddModifier(evasion);
        playerStats.armor.AddModifier(armor);
        playerStats.magicResistance.AddModifier(magicResistance);

        playerStats.fireDamage.AddModifier(fireDamage);
        playerStats.iceDamage.AddModifier(iceDamage);
        playerStats.lightingDamage.AddModifier(lightingDamage);
    }

    public void RemoveModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.RemoveModifier(strength);
        playerStats.agility.RemoveModifier(agility);
        playerStats.intelligence.RemoveModifier(intelligence);
        playerStats.vitality.RemoveModifier(vitality);

        playerStats.damage.RemoveModifier(damage);
        playerStats.critChance.RemoveModifier(critChance);
        playerStats.critPower.RemoveModifier(critPower);

        playerStats.maxHealth.RemoveModifier(health);
        playerStats.evasion.RemoveModifier(evasion);
        playerStats.armor.RemoveModifier(armor);
        playerStats.magicResistance.AddModifier(magicResistance);

        playerStats.fireDamage.RemoveModifier(fireDamage);
        playerStats.iceDamage.RemoveModifier(iceDamage);
        playerStats.lightingDamage.RemoveModifier(lightingDamage);
    }

}
