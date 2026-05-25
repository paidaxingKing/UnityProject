using System;
using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    public event Action<float> OnDoingPhysicalDamage;

    private Entity_SFX sfx;
    private Entity_VFX vfx;
    public Entity_Stats entityStats;

    public DamageScaleData basicAttackScale;

    [Header("Target Detection")]
    [SerializeField] private Transform targetCheck;
    [SerializeField] private float targetCheckRadius = 1;
    [SerializeField] private LayerMask whatIsTarget;


    private void Awake()
    {
        vfx = GetComponent<Entity_VFX>();
        sfx = GetComponent<Entity_SFX>();
        entityStats = GetComponent<Entity_Stats>();
    }

    public void PerformAttack()
    {
        bool targetGotHit = false;

        foreach (Collider2D target in GetDetectedColliders())
        {
            IDamageable damageableTarget = target.GetComponent<IDamageable>();      

            if (damageableTarget != null)
            {

                Entity_StatusHandler statusHandler = target.GetComponent<Entity_StatusHandler>(); 
                AttackData attackData = entityStats.GetAttackData(basicAttackScale);

                float physicalDamage = attackData.physicalDamage;
                float elementalDamage = attackData.elementalDamage;
                ElementType elementType = attackData.elementType;
                bool isCrit = attackData.isCrit;
                
                targetGotHit = damageableTarget.BeDamaged(physicalDamage, elementalDamage,elementType, transform); 
                
                if (elementType != ElementType.None)
                {
                    statusHandler?.ApplyStatusEffect(elementType, attackData.effectData);

                }

                if (targetGotHit)
                {
                    OnDoingPhysicalDamage?.Invoke(physicalDamage);
                    vfx.CreateOnHitVFX(target.transform,isCrit,elementType);
                    sfx?.PlayAttackHit();
                }
            }
        }
        if (!targetGotHit)
        {
            sfx?.PlayAttackMiss();
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
