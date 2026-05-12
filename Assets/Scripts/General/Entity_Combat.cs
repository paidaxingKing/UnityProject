using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    private Entity_VFX vfx;
    public Entity_Stats entityStats;

    [Header("Target Detection")]
    [SerializeField] private Transform targetCheck;
    [SerializeField] private float targetCheckRadius = 1;
    [SerializeField] private LayerMask whatIsTarget;

    [Header("Status Effect Details")]
    [SerializeField] private float defaultDuration = 3f;
    [SerializeField] private float defaultChillSlowMultiplier = 0.2f;
    [SerializeField] private float defaultCharge = 0.25f;

    private void Awake()
    {
        vfx = GetComponent<Entity_VFX>();
        entityStats = GetComponent<Entity_Stats>();
    }

    public void PerformAttack()
    {
        foreach (Collider2D target in GetDetectedColliders())
        {
            IDamageable damageableTarget = target.GetComponent<IDamageable>();      

            if (damageableTarget != null)
            {
                bool isCrit;
                ElementType elementType;
                float elementalDamage = entityStats.GetElementalDamege(out elementType,0.6f);
                
                bool targetGotHit = damageableTarget.BeDamaged(entityStats.GetPhysicalDamage(out isCrit,1),elementalDamage,elementType, transform); 
                
                if (elementType != ElementType.None)
                {
                    ApplyStatusEffect(target.transform,elementType,0.4f);
                }

                if (targetGotHit)
                {
                    vfx.UpdateOnHitColor(elementType);
                    vfx.CreateOnHitVFX(target.transform,isCrit);
                }
            }
        }
    }

    public void ApplyStatusEffect(Transform target,ElementType elementType,float scaleFactor)
    {
        Entity_StatusHandler statusHandler = target.GetComponent<Entity_StatusHandler>();
        if (statusHandler == null) return;

        if (elementType == ElementType.Ice && statusHandler.CanBeApplied(elementType))
        {
            statusHandler.ApplyChillEffect(defaultDuration,defaultChillSlowMultiplier);
        }
        else if (elementType == ElementType.Fire  && statusHandler.CanBeApplied(elementType))
        {
            float fireDamage = entityStats.offensiveStats.fireDamage.GetValue() * scaleFactor;
            statusHandler.ApplyBurnEffect(defaultDuration, fireDamage);
        }
        else if (elementType == ElementType.Lightning && statusHandler.CanBeApplied(elementType))
        {
            float lightningDamage = entityStats.offensiveStats.lightningDamage.GetValue() * scaleFactor;
            statusHandler.ApplyElectrifyEffect(defaultDuration, lightningDamage,defaultCharge);
        }
    }

    protected void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(targetCheck.position, targetCheckRadius);
    }

    protected Collider2D[] GetDetectedColliders()
    {
        return Physics2D.OverlapCircleAll(targetCheck.position, targetCheckRadius, whatIsTarget);
    }
}
