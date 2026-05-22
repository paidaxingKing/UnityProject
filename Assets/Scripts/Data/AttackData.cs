using System;
using UnityEngine;

[Serializable]
public class AttackData
{
    public float physicalDamage;
    public float elementalDamage;
    public bool isCrit;
    public ElementType elementType;

    public ElementalEffectData effectData;

    public AttackData(Entity_Stats entity_Stats,DamageScaleData damageScale)
    {
        physicalDamage = entity_Stats.GetPhysicalDamage(out isCrit, damageScale.physical);
        elementalDamage = entity_Stats.GetElementalDamege(out elementType, damageScale.elemental);

        effectData = new ElementalEffectData(entity_Stats, damageScale);

    }
}
