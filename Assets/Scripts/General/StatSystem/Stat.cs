
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
    [SerializeField] private float baseValue;
    [SerializeField] private List<StatModifier> modifications = new List<StatModifier>();//对属性数据的所有调整信息

    private float finalValue;
    private bool needToRecalculate = true;

    public float GetValue()
    {
        if (needToRecalculate)
        {
            finalValue = GetFinalValue();
            needToRecalculate = false;     
        }
        return finalValue;
    }

    public void AddModifier(float value, String source)
    {
        needToRecalculate = true;
        StatModifier mod = new StatModifier(value, source);
        modifications.Add(mod);
    }

    public void RemoveModifier(string source)
    {
        needToRecalculate = true;
        modifications.RemoveAll(modification => modification.source == source);
    }


    private float GetFinalValue()
    {
        float finalValue = baseValue;

        foreach (var item in modifications)
        {
            finalValue = finalValue + item.value;
        }

        return finalValue;
    }

    
    public void SetBaseValue(float value)
    {
        baseValue = value;
    }
}

[Serializable]
public class StatModifier
{
    public float value;
    public string source;

    public StatModifier(float value, string source)
    {
        this.value = value;
        this.source = source;
    }
}
