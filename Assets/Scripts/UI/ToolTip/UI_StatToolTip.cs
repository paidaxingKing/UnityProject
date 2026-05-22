using TMPro;
using UnityEngine;

public class UI_StatToolTip : UI_ToolTip
{
    private Player_Stats playerStats;
    private TextMeshProUGUI statToolTipText;

    protected override void Awake()
    {
        base.Awake();
        playerStats = FindFirstObjectByType<Player_Stats>();
        statToolTipText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void ShowToolTip(bool show, RectTransform targetTransform,StatType statType)
    {
        base.ShowToolTip(show, targetTransform);

        statToolTipText.text = GetStatToolTip(statType);
    }

    public string GetStatToolTip(StatType statType)
    {
        switch (statType)
        {
            case StatType.MaxHealth:
                return "Maximum Health";

            case StatType.Strength:
                return "Each 1 point of Strength increases Damage by 1 and Critical Damage by 0.5%";

            case StatType.Agility:
                return "Each 1 point of Agility increases Critical Chance by 0.5% and Evasion by 0.5%";

            case StatType.Intelligence:
                return "Each 1 point of Intelligence increases Elemental Resistance by 0.5% and bonus Elemental Damage by 1\n" +
                       "If base Elemental Damage is 0, no bonus Elemental Damage is provided";

            case StatType.Vitality:
                return "Each 1 point of Vitality increases Maximum Health by 5 and Armor by 1";

            case StatType.Damage:
                return "affects the base damage dealt by the character's normal attacks or skills";

            case StatType.CritChance:
                return "determines the probability of triggering a critical hit when attacking";

            case StatType.CritPower:
                return "determines the bonus damage multiplier when a critical hit is triggered";

            case StatType.ArmorReduction:
                return "reduces the target's Armor so they take more physical damage";

            case StatType.FireDamage:
                return "adds fire elemental damage to attacks";

            case StatType.IceDamage:
                return "adds ice elemental damage to attacks";

            case StatType.LightningDamage:
                return "adds lightning elemental damage to attacks";

            case StatType.Armor:
                return "reduces physical damage taken\n" +
                       "Current damage reduction from Armor: " + playerStats.GetArmorMitigation(0) * 100 + "%";

            case StatType.Evasion:
                return "determines the probability of successfully dodging an incoming attack";

            case StatType.IceResistance:
                return "reduces ice elemental damage taken";

            case StatType.FireResistance:
                return "reduces fire elemental damage taken";

            case StatType.LightningResistance:
                return "reduces lightning elemental damage taken";

            case StatType.ElementalDamage:
                return "Total Elemental Damage is combined from the three elemental damage types. \n " + 
                        "The highest elemental damage value contributes its full value, while the other two contribute half of their values";

            default:
                return string.Empty;
        }
    }


}
