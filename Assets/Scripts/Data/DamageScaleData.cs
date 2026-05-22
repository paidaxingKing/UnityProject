using System;
using UnityEngine;

[Serializable]
public class DamageScaleData//用于存储伤害有关的倍率以及元素的效果数据
{
    [Header("Damage")]
    public float physical = 1;//物理倍率
    public float elemental = 1;//元素伤害倍率

    [Header("Chill")]
    public float chillDuration = 3;
    public float chillSlowMultiplier = 0.3f;

    [Header("Burn")]
    public float burnDuration = 3;
    public float burnDamageScale = 1;

    [Header("Shock")]
    public float shockDuration = 3;
    public float shockDamageScale = 1;
    public float shockCharge = 0.4f;
}
