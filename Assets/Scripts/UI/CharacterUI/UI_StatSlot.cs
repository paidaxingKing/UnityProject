using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_StatSlot : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    private Player_Stats playerStats;

    private RectTransform rect;
    private UI ui;

    [SerializeField] private StatType statSlotType;
    [SerializeField] private TextMeshProUGUI statName;
    [SerializeField] private TextMeshProUGUI statValue;

    private void Awake()
    {
        ui = GetComponentInParent<UI>();
        rect = GetComponent<RectTransform>(); 
        playerStats = FindFirstObjectByType<Player_Stats>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.statToolTip.ShowToolTip(true, rect,statSlotType);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.statToolTip.ShowToolTip(false,null);
    }


    private void OnValidate()
    {
        gameObject.name = "UI_Stat - " + GetStatNameByType(statSlotType);
        statName.text = GetStatNameByType(statSlotType);
    }

    public void UpdateStatValue()
    {
        Stat statToUpdate = playerStats.GetStatByType(statSlotType);

        if (statToUpdate == null && statSlotType != StatType.ElementalDamage) return;

        float value = 0;

        switch (statSlotType)
        {
            //Major stats
            case StatType.Strength:
                value = playerStats.majorStats.strength.GetValue();
                break;
            case StatType.Agility:
                value = playerStats.majorStats.agility.GetValue();
                break;
            case StatType.Vitality:
                value = playerStats.majorStats.vitality.GetValue();
                break;
            case StatType.Intelligence:
                value = playerStats.majorStats.intelligence.GetValue();
                break;

            //offensive stats
            case StatType.Damage:
                value = playerStats.GetBaseDamage();
                break;
            case StatType.CritChance:
                value = playerStats.GetCritChance();
                break;
            case StatType.CritPower:
                value = playerStats.GetCritPower();
                break;
            case StatType.ArmorReduction:
                value = playerStats.GetArmorReduction() * 100;
                break;

            //defensive stats
            case StatType.MaxHealth:
                value = playerStats.resourceStats.maxHealth.GetValue();
                break;
            case StatType.Evasion:
                value = playerStats.GetEvasion();
                break;
            case StatType.Armor:
                value = playerStats.GetBaseArmor();
                break;

            //elemental stats
            case StatType.FireDamage:
                value = playerStats.offensiveStats.fireDamage.GetValue();
                break;
            case StatType.IceDamage:
                value = playerStats.offensiveStats.iceDamage.GetValue();
                break;
            case StatType.LightningDamage:
                value = playerStats.offensiveStats.lightningDamage.GetValue();
                break;
            case StatType.ElementalDamage:
                value = playerStats.GetElementalDamege(out ElementType element, 1);
                break;

            //element resistance stats
            case StatType.FireResistance:
                value = playerStats.GetElementalResistance(ElementType.Fire) * 100;
                break;
            case StatType.IceResistance:
                value = playerStats.GetElementalResistance(ElementType.Ice) * 100;
                break;
            case StatType.LightningResistance:
                value = playerStats.GetElementalResistance(ElementType.Lightning) * 100;
                break;
        }

        statValue.text = IsPercentageStat(statSlotType) ? value + "%" : value.ToString();
    }

    private bool IsPercentageStat(StatType type)
    {
        switch (type)
        {
            case StatType.CritChance:
            case StatType.CritPower:
            case StatType.ArmorReduction:
            case StatType.IceResistance:
            case StatType.FireResistance:
            case StatType.LightningResistance:
            case StatType.Evasion:
                return true;

            default:
                return false;

        }
    }

    private string GetStatNameByType(StatType type)
    {
        switch (type)
        {
            case StatType.MaxHealth: return "Max Health";

            case StatType.FireDamage: return "Fire Damage";
            case StatType.IceDamage: return "Ice Damage";
            case StatType.LightningDamage: return "Lightning Damage";
            case StatType.Damage: return "Damage";

            case StatType.Strength: return "Strength";
            case StatType.Agility: return "Agility";
            case StatType.Vitality: return "Vitality";
            case StatType.Intelligence: return "Intelligence";

            case StatType.Armor: return "Armor";
            case StatType.ArmorReduction: return "Armor Reduction";

            case StatType.CritChance: return "Crit Chance";
            case StatType.CritPower: return "Crit Power";

            case StatType.Evasion: return "Evasion";
            case StatType.ElementalDamage: return "Elemental Damage";

            case StatType.FireResistance: return "Fire Resistance";
            case StatType.IceResistance: return "Ice Resistance";
            case StatType.LightningResistance: return "Lightning Resistance";
            default: return "Unknow Stat";

        }
    }

    
}
