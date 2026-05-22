using System;
using UnityEngine;

[Serializable]
public class ElementalEffectData //用于获得元素伤害有关的具体数据
{
    public float chillDuration;
    public float chillSlowMultiplier;

    public float burnDuration;
    public float burnDamage;

    public float shockDuration;
    public float shockDamage;
    public float shockCharge;

    public ElementalEffectData(Entity_Stats entityStats,DamageScaleData damageScale)
    {
        chillDuration = damageScale.chillDuration;
        chillSlowMultiplier = damageScale.chillSlowMultiplier;

        burnDuration = damageScale.burnDuration;
        burnDamage = entityStats.offensiveStats.fireDamage.GetValue() * damageScale.burnDamageScale;//基础伤害乘以倍率

        shockDuration = damageScale.shockDuration;
        shockDamage = entityStats.offensiveStats.lightningDamage.GetValue() * damageScale.shockDamageScale;
        shockCharge = damageScale.shockCharge;
    }

}



