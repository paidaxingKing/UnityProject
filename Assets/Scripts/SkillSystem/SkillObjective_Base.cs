using UnityEngine;

public class SkillObjective_Base : MonoBehaviour
{
    [SerializeField] protected LayerMask whatIsEnemy;
    [SerializeField] protected Transform targetCheck;
    [SerializeField] protected float checkRadius = 1;

    [SerializeField] protected float trackDistance = 20;

    //每个技能物体造成的伤害与角色的基础属性和该技能的伤害倍率有关
    protected Entity_Stats playerStats;
    protected DamageScaleData damageScaleData;
    protected ElementType elementType;

    protected Collider2D[] EnemiesAround(Transform transform,float radius)
    {
        return Physics2D.OverlapCircleAll(transform.position,radius,whatIsEnemy);
    }

    protected void DamageEnemiesInRadius(Transform transform, float radius)
    {
        foreach (var target in EnemiesAround(transform,radius))
        {
            IDamageable damageable = target.GetComponent<IDamageable>();

            if (damageable == null) continue;

            AttackData attackData = playerStats.GetAttackData(damageScaleData);
            Entity_StatusHandler statusHandler = target.GetComponent<Entity_StatusHandler>();

            float physicalDamage = attackData.physicalDamage;
            float elementalDamage = attackData.elementalDamage;
            ElementType elementType = attackData.elementType;

            damageable.BeDamaged(physicalDamage, elementalDamage, elementType, transform);

            if (elementType != ElementType.None)
            {
                target.GetComponent<Entity_StatusHandler>().ApplyStatusEffect(elementType, attackData.effectData);
            }
        }
    }

    protected Transform ClosestTarget()
    {
        Transform target = null;
        float closestDistance = Mathf.Infinity;

        foreach (var enemy in EnemiesAround(transform, trackDistance))
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);

            if (distance < closestDistance)
            {
                target = enemy.transform;
                closestDistance = distance;
            }

        }
        return target;

    }

    protected virtual void OnDrawGizmos()
    {

        if (targetCheck == null)
        {
            targetCheck = transform;
        }
        Gizmos.DrawWireSphere(targetCheck.position, checkRadius);


    }
 
}
