using UnityEngine;
using UnityEngine.UI;

public class Entity_Health : MonoBehaviour,IDamageable
{
    private Entity_VFX entityVFX;
    private Entity entity;
    private Slider healthBar;
    private Entity_Stats entityStats;

    [SerializeField] protected float currentHp;
    [SerializeField] protected bool isDead = false;

    [Header("On Damage Knockback")]
    [SerializeField] private Vector2 knockbackPower = new Vector2(1.5f, 2.5f);
    [SerializeField] private Vector2 heavyKnockbackPower = new Vector2(7,7);
    [SerializeField] private float knockbackDuration = 0.2f;
    [SerializeField] private float heavyKnockbackDuration = 0.5f;
    [Header("On Heavy Damage")]
    [SerializeField] private float heavyDamageThreshold = .3f;

    protected virtual void Awake()
    {
        entityVFX = GetComponent<Entity_VFX>();
        entity = GetComponent<Entity>();
        healthBar = GetComponentInChildren<Slider>();
        entityStats = GetComponent<Entity_Stats>();

        currentHp = entityStats.GetMaxHp();
        UpdateHealthBar();
    }

    public virtual bool BeDamaged(float damage,float elementalDamage,ElementType elementType, Transform damageCauser)
    {
        if (isDead) return false;

        if (AttackEvaded())
        {
            return false;
        }

        Entity_Stats attackerStats = damageCauser.GetComponent<Entity_Stats>();//获得攻击者的属性
        float armorReduction = attackerStats.GetArmorReduction();//根据攻击者的属性计算护甲穿透

        float mitigation = entityStats.GetArmorMitigation(armorReduction);//根据护甲穿透和自身护甲值计算护甲减免
        float finalPhysicalDamage = damage * (1 - mitigation);

        float elementalResistance = entityStats.GetElementalResistance(elementType);//根据元素类型和自身属性计算元素抗性
        float finalElementalDamage = elementalDamage * (1 - elementalResistance);

        TakeKnockback(damageCauser, finalPhysicalDamage);
        ReduceHp(finalPhysicalDamage + finalElementalDamage);

        return true;
    }

    private void TakeKnockback(Transform damageCauser, float finalPhysicalDamage)
    {
        Vector2 knockback = CalculateKnockback(finalPhysicalDamage, damageCauser);//根据伤害和攻击者位置计算击退效果
        float duration = CalculateKnockbackDuration(finalPhysicalDamage);
        entity.ReceiveKnockback(knockback, duration);
    }

    private bool AttackEvaded()
    {
        return Random.Range(0,100) < entityStats.GetEvasion();
    }

    protected void ReduceHp(float damage)
    {
        entityVFX.PlayOnDamageVFX();
        currentHp -= damage;
        UpdateHealthBar();
        if (currentHp <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBar == null) return;
        healthBar.value = currentHp / entityStats.GetMaxHp();
    }

    private void Die()
    {
        isDead = true;

        entity.EntityDeath();
    }

    private float CalculateKnockbackDuration(float damage)
    {
        return IsHeavyDamage(damage) ? heavyKnockbackDuration : knockbackDuration;
    }

    private Vector2 CalculateKnockback(float damage,Transform damageCauser)
    {
        int direction = damageCauser.position.x > transform.position.x ? -1 : 1;

        Vector2 knockback = IsHeavyDamage(damage)? heavyKnockbackPower : knockbackPower;

        knockback.x = knockback.x * direction;

        return knockback;
    }

    private bool IsHeavyDamage(float damage)
    {
        return damage >= entityStats.GetMaxHp() * heavyDamageThreshold;
    }
}
