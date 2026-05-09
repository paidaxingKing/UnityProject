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
    [SerializeField] private float defaultChillDuration = 3f;
    [SerializeField] private float defaultChillSlowMultiplier = 0.2f;

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
                float elementalDamage = entityStats.GetElementalDamege(out elementType);
                
                bool targetGotHit = damageableTarget.BeDamaged(entityStats.GetPhysicalDamage(out isCrit),elementalDamage,elementType, transform); 
                
                if (elementType != ElementType.None)
                {
                    ApplyStatusEffect(target.transform,elementType);
                }

                if (targetGotHit)
                {
                    vfx.UpdateOnHitColor(elementType);
                    vfx.CreateOnHitVFX(target.transform,isCrit);
                }
            }
        }
    }

    public void ApplyStatusEffect(Transform target,ElementType elementType)
    {
        Entity_StatusHandler statusHandler = target.GetComponent<Entity_StatusHandler>();
        if (statusHandler == null) return;

        if (elementType == ElementType.Ice && statusHandler.CanBeApplied(elementType))
        {
            statusHandler.ApplyChilledEffect(defaultChillDuration,defaultChillSlowMultiplier);
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
