using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Entity_StatusHandler : MonoBehaviour
{
    private ElementType currentEffect = ElementType.None;
    private Entity_VFX entity_VFX;
    private Entity_Stats entity_Stats;
    private Entity entity;

    private void Awake()
    {
        entity = GetComponent<Entity>();
        entity_VFX = GetComponent<Entity_VFX>();
        entity_Stats = GetComponent<Entity_Stats>();
    }

    public void ApplyChilledEffect(float duration,float slowMultiplier)
    {
        float iceResistance = entity_Stats.GetElementalResistance(ElementType.Ice);
        float reducedDuration = duration * (1 - iceResistance);
        
        StartCoroutine(ChilledEffectCo(reducedDuration,slowMultiplier));
    }

    private IEnumerator ChilledEffectCo(float duration,float slowMultiplier)
    {
        entity.SlowDownEntity(duration, slowMultiplier);
        currentEffect = ElementType.Ice;
        entity_VFX.PlayOnStatusVFX(duration, ElementType.Ice);

        yield return new WaitForSeconds(duration);

        currentEffect = ElementType.None;
    }

    public bool CanBeApplied(ElementType element)
    {
        return currentEffect == ElementType.None;
    }
}
