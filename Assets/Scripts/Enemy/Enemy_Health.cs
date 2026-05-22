using UnityEngine;
using UnityEngine.UI;

public class Enemy_Health : Entity_Health
{
    private Enemy enemy;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
    }
    
    public override bool BeDamaged(float damage,float elementalDamage,ElementType elementType, Transform damageCauser)
    {
        bool gotHit = true;
        gotHit =  base.BeDamaged(damage,elementalDamage, elementType, damageCauser);
        if (damageCauser.GetComponent<Player>() != null)
        {
            enemy.TryEnterBattleState(damageCauser);
        }
          
        return gotHit;
    }
}
