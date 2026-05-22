using System;
using UnityEngine;
using UnityEngine.UI;

public class Entity_Health : MonoBehaviour,IDamageable
{

    public event Action OnTakingDamage;

    public event Action OnHealthUpdate;

    private Entity_VFX entityVFX;
    private Entity entity;
    private Slider healthBar;
    private Entity_Stats entityStats;
    private Entity_DropManager dropManager;

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
        dropManager = GetComponent<Entity_DropManager>();

        currentHp = entityStats.GetMaxHp();
        UpdateHealthBar();
    }

    public float GetMaxHp()
    {
        return entityStats.GetMaxHp();
    }

    public float GetCurHp()
    {
        return Mathf.Max(0,currentHp);
    }

    public float GetHealthPercent()
    {
        return currentHp / entityStats.GetMaxHp();
    }

    public void SetHealthToPercent(float percent)
    {
        currentHp = entityStats.GetMaxHp() * Mathf.Clamp(percent,0,1);
        OnHealthUpdate?.Invoke();
    }

    public virtual bool BeDamaged(float damage,float elementalDamage,ElementType elementType, Transform damageCauser)
    {
        if (isDead) return false;

        if (AttackEvaded())
        {
            return false;
        }

        Entity_Stats attackerStats = damageCauser.GetComponent<Entity_Stats>();//获得攻击者的属性
        float armorReduction = attackerStats != null ? attackerStats.GetArmorReduction() : 0;//根据攻击者的属性计算护甲穿透

        float mitigation = entityStats.GetArmorMitigation(armorReduction);//根据护甲穿透和自身护甲值计算护甲减免
        float finalPhysicalDamage = damage * (1 - mitigation);

        float elementalResistance = entityStats.GetElementalResistance(elementType);//根据元素类型和自身属性计算元素抗性
        float finalElementalDamage = elementalDamage * (1 - elementalResistance);

        TakeKnockback(damageCauser, finalPhysicalDamage);
        ReduceHp(finalPhysicalDamage + finalElementalDamage);

        OnTakingDamage?.Invoke();

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
        return UnityEngine.Random.Range(0,100) < entityStats.GetEvasion();
    }

    public void AddHp(float health)
    {
        if (currentHp <= 0) return;

        currentHp = Mathf.Min(currentHp + health,entityStats.GetMaxHp());
        OnHealthUpdate?.Invoke();
        UpdateHealthBar();
    }

    public void ReduceHp(float damage)
    {
        entityVFX.PlayOnDamageVFX();
        currentHp -= damage;
        OnHealthUpdate?.Invoke();
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

    protected virtual void Die()
    {
        isDead = true;

        entity.EntityDeath();
        dropManager?.DropItems();
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
