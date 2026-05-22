using UnityEngine;

public interface IDamageable
{
    public bool BeDamaged(float damage,float elementalDamage, ElementType elementType, Transform damageCauser);
}
