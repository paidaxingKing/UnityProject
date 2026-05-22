using UnityEngine;

[CreateAssetMenu(menuName = "RPG SetUp/Item Data/Item effect/Heal on doing physical damage", fileName = "Item effect data - Heal on doing physical damage")]
public class ItemEffect_HealOnDoingDamage : ItemEffect_DataSO
{
    [SerializeField] private float percentHealedOnAttack = 0.05f;

    private void HealOnDoingDamage(float damage)
    {
        player.entity_Health.AddHp(damage * percentHealedOnAttack);
    }

    public override void ExecuteEffect()
    {
        base.ExecuteEffect();
    }

    public override void Subscribe(Player player)
    {
        base.Subscribe(player);
        player.combat.OnDoingPhysicalDamage += HealOnDoingDamage;//事件的参数要和订阅该事件的方法的参数相匹配
    }

    public override void Unsubscribe()
    {
        base.Unsubscribe();
        player.combat.OnDoingPhysicalDamage -= HealOnDoingDamage;
    }
}
