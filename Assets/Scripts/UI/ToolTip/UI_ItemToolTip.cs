using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class UI_ItemToolTip : UI_ToolTip
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemType;
    [SerializeField] private TextMeshProUGUI itemInfo;

    public void ShowToolTip(bool show, RectTransform targetTransform,Inventory_Item itemToShow)
    {
        base.ShowToolTip(show, targetTransform);

        itemName.text = itemToShow.itemData.itemName;
        itemType.text = itemToShow.itemData.itemType.ToString();
        itemInfo.text = GetItemInfo(itemToShow);
    }

    public string GetItemInfo(Inventory_Item item)
    {
        if (item.itemData.itemType == ItemType.Material)
        {
            return "Used for crafting";
        }

        if (item.itemData.itemType == ItemType.Consumable)
        {
            return item.itemData.effect.effectDescription;
        }

        StringBuilder sb = new StringBuilder();

        sb.AppendLine("");

        foreach (var mod in item.modifiers)
        {
            string modType = GetStatNameByType(mod.statType);
            string modValue = IsPercentageStat(mod.statType) ? mod.value.ToString() + "%" : mod.value.ToString();
            sb.AppendLine("+ " + modValue + " " + modType);
        }

        if (item.itemEffect != null)
        {
            sb.AppendLine("");
            sb.AppendLine("Unique effect: ");
            sb.AppendLine(item.itemEffect.effectDescription);
        }

        return sb.ToString();
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

            case StatType.FireResistance: return "Fire Resistance";
            case StatType.IceResistance: return "Ice Resistance";
            case StatType.LightningResistance: return "Lightning Resistance";
            default: return "Unknow Stat";

        }
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
    
}
