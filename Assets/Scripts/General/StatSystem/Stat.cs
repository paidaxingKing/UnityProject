using System;
using UnityEngine;

[Serializable]
public class Stat
{
    [SerializeField] private float baseValue;
    [SerializeField] private float bonusValue;

    public float GetValue()
    {
        return baseValue + bonusValue;
    }

    public void AddBonus(float value)
    {
        bonusValue += value;
    }

    public float GetBonusValue()
    {
        return bonusValue;
    }

    public void SetBaseValue(float value)
    {
        baseValue = value;
    }
}
