using System.Collections;
using UnityEngine;

public class Entity_StatusHandler : MonoBehaviour
{
    private ElementType currentEffect = ElementType.None;
    private Entity_VFX entity_VFX;
    private Entity_Stats entity_Stats;
    private Entity_Health entity_Health;
    private Entity entity;

    [Header("Electrify effect details")]
    [SerializeField] private GameObject lightningStrikeVFX;
    [SerializeField] private float cuurentCharge;
    [SerializeField] private float maxiumCharge  = 1;
    private Coroutine electrifyCo;

    private void Awake()
    {
        entity = GetComponent<Entity>();
        entity_VFX = GetComponent<Entity_VFX>();
        entity_Stats = GetComponent<Entity_Stats>();
        entity_Health = GetComponent<Entity_Health>();
    }

    public void ApplyElectrifyEffect(float duration,float damage,float charge)
    {
        float lightningResistance = entity_Stats.GetElementalResistance(ElementType.Lightning);
        float reducedCharge = charge * (1 - lightningResistance);

        cuurentCharge += reducedCharge;

        if (cuurentCharge >= maxiumCharge)
        {
            LightningStrike(damage);
            StopElectrifyEffect();
            return;
        }

        if (electrifyCo != null)
        {
            StopCoroutine(electrifyCo);
        }
        //积累电荷，检查是否有足够的电荷造成电击
        electrifyCo = StartCoroutine(ElectrifyEffectCo(duration));
    }

    private IEnumerator ElectrifyEffectCo(float duration)
    {
        currentEffect = ElementType.Lightning;
        entity_VFX.PlayOnStatusVFX(duration, ElementType.Lightning);

        yield return new WaitForSeconds(duration);

        StopElectrifyEffect();
    }

    private void StopElectrifyEffect()
    {
        currentEffect = ElementType.None;
        cuurentCharge = 0;
        entity_VFX.StopAllVFX();
         
    }

    private void LightningStrike(float damage)
    {
        Instantiate(lightningStrikeVFX, transform.position, Quaternion.identity);
        entity_Health.ReduceHp(damage);
    }

    public void ApplyBurnEffect(float duration,float fireDamage)
    {
        float fireResistance = entity_Stats.GetElementalResistance(ElementType.Fire);
        float reducedDamage = fireDamage * (1 - fireResistance);

        StartCoroutine(BurnEffectCo(duration, reducedDamage));
    }

    private IEnumerator BurnEffectCo(float duraion,float totalDamage)
    {
        currentEffect = ElementType.Fire;
        entity_VFX.PlayOnStatusVFX(duraion,ElementType.Fire);

        int ticksPerSecond = 2;
        int tickCount = Mathf.RoundToInt(ticksPerSecond * duraion);

        float damagePerTcik = totalDamage / tickCount;
        float tickInterval = 1f / ticksPerSecond;
        
        for (int i = 0; i < tickCount; i++)
        {
            entity_Health.ReduceHp(damagePerTcik);
            yield return new WaitForSeconds(tickInterval);
        }

        currentEffect = ElementType.None;
    }

    public void ApplyChillEffect(float duration,float slowMultiplier)
    {
        float iceResistance = entity_Stats.GetElementalResistance(ElementType.Ice);
        float reducedDuration = duration * (1 - iceResistance);
        
        StartCoroutine(ChillEffectCo(reducedDuration,slowMultiplier));
    }

    private IEnumerator ChillEffectCo(float duration,float slowMultiplier)
    {
        entity.SlowDownEntity(duration, slowMultiplier);
        currentEffect = ElementType.Ice;
        entity_VFX.PlayOnStatusVFX(duration, ElementType.Ice);

        yield return new WaitForSeconds(duration);

        currentEffect = ElementType.None;
    }

    public bool CanBeApplied(ElementType element)
    {
        if (element == ElementType.Lightning && currentEffect == ElementType.Lightning) return true;
       
        return currentEffect == ElementType.None;
    }
}
