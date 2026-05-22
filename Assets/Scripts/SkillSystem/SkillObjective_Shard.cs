using System;
using UnityEngine;

public class SkillObjective_Shard : SkillObjective_Base
{
    [SerializeField] private float acceleration = 40f;
    public event Action OnExplode;
    private Skill_Shard shardManager;

    [SerializeField] private GameObject vfxPrefab;

    private Transform target;
    private float maxSpeed;
    private float currentSpeed;

    private void Update()
    {
        if (target == null) return;

        currentSpeed = Mathf.MoveTowards(
            currentSpeed,
            maxSpeed,
            acceleration * Time.deltaTime
        );

        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            currentSpeed * Time.deltaTime
        );

    }

    public void MoveTowardsClosestTarget(float speed)
    {
        target = ClosestTarget();
        maxSpeed = speed;
        currentSpeed = 0f;
    }

    public void SetupShard(Skill_Shard shardManager,float detonationTime,bool canMove,float shardSpeed)
    {
        this.shardManager = shardManager;

        playerStats = shardManager.player.entity_Stats;
        damageScaleData = shardManager.damageScaleData;

        Invoke(nameof(Explode), detonationTime);

        if (canMove)
            MoveTowardsClosestTarget((float)shardSpeed);
    }

    public void SetupShard(Skill_Shard shardManager)//detonation爆炸
    {
        this.shardManager = shardManager;

        playerStats = shardManager.player.entity_Stats;
        damageScaleData = shardManager.damageScaleData;

        float detonationTime = shardManager.GetDetonationTime();

        Invoke(nameof(Explode), detonationTime);//延迟调用，用于定时调用方法，在detonationTime时间过后自动调用Explode方法，实现到时爆炸
    }

    //当检测到Trigger时自动调用该方法，只需要自己和对方其中一个勾选了Trigger即可
    private void OnTriggerEnter2D(Collider2D collision)      
    {
        if (collision.GetComponent<Enemy>() == null)
            return;

        Explode();
    }



    public void Explode()
    {
        DamageEnemiesInRadius(targetCheck, checkRadius);
        GameObject vfx =  Instantiate(vfxPrefab, transform.position, Quaternion.identity);
        SpriteRenderer sprite = vfx.GetComponentInChildren<SpriteRenderer>();
        sprite.color = shardManager.player.vfx.GetElementColor(elementType);

        OnExplode?.Invoke();
        Destroy(gameObject);
    }

    
}
