using UnityEngine;

[CreateAssetMenu(menuName = "RPG SetUp/Item Data/Item effect/Ice Blast", fileName = "Item effect data - Ice blast on taking damage")]
public class ItemEffect_IceBlastOnTakingDamage : ItemEffect_DataSO
{
    [SerializeField] private ElementalEffectData effectData;
    [SerializeField] private float iceDamage;
    [SerializeField] private LayerMask whatIsEnemy;

    [SerializeField] private float healthPercentTrigger = 0.25f;
    [SerializeField] private float cooldown;

    private float lastTimeUsed = -999;

    [Header("VFX objects")]
    [SerializeField] private GameObject iceBlastVfx;
    [SerializeField] private GameObject onHitVfx;

    public override void ExecuteEffect()
    {
        bool noCooldwon = Time.time >= lastTimeUsed + cooldown;

        bool reachThreshold = player.entity_Health.GetHealthPercent() <= healthPercentTrigger;
        if (noCooldwon && reachThreshold)
        {
            player.vfx.CreatEffectOf(iceBlastVfx, player.transform);
            lastTimeUsed = Time.time;
            DamageEnemiesWithIce();
        }
    }

    private void DamageEnemiesWithIce()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(player.transform.position, 1.5f, whatIsEnemy);

        foreach (var target in enemies)
        {
            IDamageable damageable = target.GetComponent<IDamageable>();

            if (damageable == null) continue;

            bool targetGotHit = damageable.BeDamaged(0, iceDamage, ElementType.Ice, player.transform);
            Entity_StatusHandler statusHandler = target.GetComponent<Entity_StatusHandler>();
            statusHandler?.ApplyStatusEffect(ElementType.Ice, effectData);

            if (targetGotHit)
                player.vfx.CreatEffectOf(onHitVfx,target.transform);

        }
    }

    public override void Subscribe(Player player)
    {
        base.Subscribe(player);
        player.entity_Health.OnTakingDamage += ExecuteEffect;
    }

    public override void Unsubscribe()
    {
        base.Unsubscribe();
        player.entity_Health.OnTakingDamage -= ExecuteEffect;
        player = null;
    }
}
