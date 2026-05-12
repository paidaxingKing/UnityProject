using UnityEngine;

public class Entity_Stats : MonoBehaviour
{
    public Stat_SetupSO defaultStatSetup;

    public Stat_ResourceGroup resourceStats;
    public Stat_MajorGroup majorStats;
    public Stat_OffensiveGroup offensiveStats;
    public Stat_DefensiveGroup defensiveStats;

    public float GetElementalDamege(out ElementType elementType,float scaleFactor)
    {
        float fireDamage = offensiveStats.fireDamage.GetValue();
        float iceDamage = offensiveStats.iceDamage.GetValue();
        float lightningDamage = offensiveStats.lightningDamage.GetValue();

        float bonusElementaDamage = majorStats.intelligence.GetValue() * 1.5f; // 每一点智力值增加1.5点元素伤害

        elementType = ElementType.Fire;
        float highestDamage = fireDamage;
        
        if (iceDamage > highestDamage)
        {
            highestDamage = iceDamage;
            elementType = ElementType.Ice;
        }

        if (lightningDamage > highestDamage)
        {
            highestDamage = lightningDamage;
            elementType = ElementType.Lightning;
        }

        if (highestDamage == 0)
        {
            elementType = ElementType.None; // 如果没有任何元素伤害，元素类型设为None
            return 0; // 如果没有任何元素伤害，直接返回0
        }

        float bonusFireDamage = (elementType == ElementType.Fire) ? 0 : fireDamage * 0.5f;
        float bonusIceDamage = (elementType == ElementType.Ice) ? 0 : iceDamage * 0.5f;
        float bonusLightningDamage = (elementType == ElementType.Lightning) ? 0 : lightningDamage * 0.5f;

        // 计算最终元素伤害，最高的元素伤害全额加成，其他元素伤害加成50%，再加上智力提供的元素伤害加成
        float weakerElementDamage = bonusFireDamage + bonusIceDamage + bonusLightningDamage;
        float finalDamage = highestDamage + bonusElementaDamage + weakerElementDamage; 
        return finalDamage * scaleFactor;
    }

    public float GetPhysicalDamage(out bool isCriticalHit,float scaleFactor)
    {
        float baseDamage = offensiveStats.damage.GetValue();
        float bonuslDamage = majorStats.strength.GetValue(); // 每一点力量值增加1点物理伤害
        float totalBaseDamage = baseDamage + bonuslDamage;

        float baseCritChance = offensiveStats.critChance.GetValue();
        float bonusCritChance = majorStats.agility.GetValue() * 0.5f; // 每一点敏捷值增加0.5的暴击率
        float totalCritChance = baseCritChance + bonusCritChance;

        float baseCritPower = offensiveStats.critPower.GetValue();
        float bonusCritPower = majorStats.strength.GetValue() * 0.5f; // 每一点力量值增加0.5的暴击伤害加成
        float totalCritPower = baseCritPower + bonusCritPower;

        isCriticalHit = Random.Range(0, 100) < totalCritChance; // 判断是否暴击

        float finalDamage = isCriticalHit ? totalBaseDamage * (1 + totalCritPower / 100f) : totalBaseDamage; // 计算最终伤害

        return finalDamage * scaleFactor;

    }

    public float GetElementalResistance(ElementType elementType)
    {
        float baseResistance = 0;
        float bonusResistance = majorStats.intelligence.GetValue() * 0.5f; // 每一点智力值增加0.5点元素抗性

        switch (elementType)
        {
            case ElementType.Fire:
                baseResistance = defensiveStats.fireResistance.GetValue();
                break;
            case ElementType.Ice:
                baseResistance = defensiveStats.iceResistance.GetValue();
                break;
            case ElementType.Lightning:
                baseResistance = defensiveStats.lightningResistance.GetValue();
                break;
        }

        float totalResistance = baseResistance + bonusResistance;
        float resistanceCap = 75f; // 设置元素抗性上限为75%，cap是指上限的意思，避免元素抗性过高导致游戏失衡
        float finalResistance = Mathf.Clamp(totalResistance, 0, resistanceCap) / 100; // 将元素抗性限制在0到上限之间

        return finalResistance;
    }

    public float GetArmorReduction()
    {
        
        float finalReduction = offensiveStats.armorReduction.GetValue() / 100f;

        return finalReduction;
    }

    public float GetArmorMitigation(float armorReduction)//mitigation是指减免的意思，armor mitigation就是护甲减免，指的是护甲提供的伤害减免效果
    {
        float baseArmor = defensiveStats.armor.GetValue();
        float bonusArmor = majorStats.vitality.GetValue() * 1; // 每一点活力值增加1点护甲
        float totalArmor = baseArmor + bonusArmor;

        float reductionMultiplier = Mathf.Clamp(1 - armorReduction,0,1); // 计算护甲穿透后的护甲值乘数，护甲穿透是一个百分比，减少护甲提供的减免效果

        float reducedArmor = totalArmor * reductionMultiplier; // 计算穿透后的护甲值

        float mitigation = reducedArmor / (reducedArmor + 100); // 计算伤害减免，假设每100点护甲提供50%的伤害减免
        float mitigationCap = 0.75f; // 设置伤害减免上限为75%，cap是指上限的意思，避免伤害减免过高导致游戏失衡
        float finalMitigation = Mathf.Clamp(mitigation, 0, mitigationCap); // 将伤害减免限制在0到上限之间

        return finalMitigation;

    }

    public float GetMaxHp()
    {
        float baseHp = resourceStats.maxHealth.GetValue();
        float bonusHp = majorStats.vitality.GetValue() * 5; // 每一点活力值增加5点生命值上线
        float finalHp = baseHp + bonusHp;

        return finalHp;
    }

    public float GetEvasion()
    {
        float baseEvasion = defensiveStats.evasion.GetValue();
        float bonusEvasion = majorStats.agility.GetValue() * 0.5f; // 每一点敏捷值增加0.5点闪避

        float totalEvasion = baseEvasion + bonusEvasion;
        float evasionCap = 75f; // 设置闪避上限为75%,cap是指上限的意思，避免闪避过高导致游戏失衡
        float finalEvasion = Mathf.Clamp(totalEvasion, 0, evasionCap); // 将总闪避值限制在0到上限之间
        return finalEvasion;
    }

    [ContextMenu("Update Default Stat Setup")]
    public void ApplyDefaultStatsSetup()
    {
        if (defaultStatSetup == null)
        {
            Debug.Log("No default stat setup assigned");
            return;
        }

        resourceStats.maxHealth.SetBaseValue(defaultStatSetup.maxHp);
        resourceStats.healthRegen.SetBaseValue(defaultStatSetup.healthRegen);

        majorStats.strength.SetBaseValue(defaultStatSetup.strength);
        majorStats.agility.SetBaseValue(defaultStatSetup.agility);
        majorStats.intelligence.SetBaseValue(defaultStatSetup.intelligence);
        majorStats.vitality.SetBaseValue(defaultStatSetup.vitality);

        offensiveStats.damage.SetBaseValue(defaultStatSetup.damage);
        offensiveStats.critChance.SetBaseValue(defaultStatSetup.critChance);
        offensiveStats.critPower.SetBaseValue(defaultStatSetup.critPower);
        offensiveStats.armorReduction.SetBaseValue(defaultStatSetup.armorReduction);

        offensiveStats.iceDamage.SetBaseValue(defaultStatSetup.iceDamage);
        offensiveStats.fireDamage.SetBaseValue(defaultStatSetup.fireDamage);
        offensiveStats.lightningDamage.SetBaseValue(defaultStatSetup.lightningDamage);

        defensiveStats.iceResistance.SetBaseValue(defaultStatSetup.iceResistance);
        defensiveStats.fireResistance.SetBaseValue(defaultStatSetup.fireResistance);
        defensiveStats.lightningResistance.SetBaseValue(defaultStatSetup.lightningResistance);

        defensiveStats.armor.SetBaseValue(defaultStatSetup.armor);
        defensiveStats.evasion.SetBaseValue(defaultStatSetup.evasion);
    }
}
