using System.Collections;
using UnityEngine;

public class Skill_Shard : Skill_Base
{
    private SkillObjective_Shard currentShard;
    private Entity_Health playerHealth;

    [SerializeField] private GameObject shardPrefab;
    [SerializeField] private float detonationTime = 2;

    [Header("Moving Shard Upgrade")]
    [SerializeField] private float shardSpeed = 20;

    [Header("MultiCast Shard Upgrade")]
    [SerializeField] private int maxCharges = 3;
    [SerializeField] private int currentCharges;
    [SerializeField] private bool isCharging;

    [Header("Teleport Shard Upgrade")]
    [SerializeField] private float shardExistDuration = 10;

    [Header("Health Rewind Shard Update")]
    [SerializeField] private float savedHealthPercent;

    protected override void Awake()
    {
        base.Awake();

        currentCharges = maxCharges;
        playerHealth = GetComponentInParent<Entity_Health>();
    }

    public override void TryUseSkill()
    {
        if (!CanUseSkill()) return;

        if (IsUnlocked(SkillUpgradeType.Shard))
        {
            HandleShardRegular();
        }

        if (IsUnlocked(SkillUpgradeType.Shard_MoveToEnemy))
        {
            HandleShardMoving();
        }

        if (IsUnlocked(SkillUpgradeType.Shard_MultiCast))
        {
            HandleShardMultiCast();
        }

        if (IsUnlocked(SkillUpgradeType.Shard_Teleport))
        {
            HandleShardTeleport();
        }

        if (IsUnlocked(SkillUpgradeType.Shard_TeleportAndHpRewind))
        {
            HandleShardHealthRewind();
        }
    }

    private void HandleShardHealthRewind()
    {
        if (currentShard == null)
        {
            CreateShard();
            savedHealthPercent = playerHealth.GetHealthPercent();
        }
        else
        {
            SwapPlayerAndShard();
            playerHealth.SetHealthToPercent(savedHealthPercent);
            SetSkillOnCooldown();
        }
    }

    private void HandleShardTeleport()
    {
        if (currentShard == null)
        {
            CreateShard();
        }
        else
        {
            SwapPlayerAndShard();
            SetSkillOnCooldown();
        }
    }

    private void SwapPlayerAndShard()
    {
        Vector3 shardPosition = currentShard.transform.position;
        Vector3 playerPosition = player.transform.position;

        currentShard.transform.position = playerPosition;
        player.TeleportPlayer(shardPosition);
        currentShard.Explode();
    }

    private void HandleShardMultiCast()
    {
        if (currentCharges <= 0) return;

        CreateShard();

        currentShard.MoveTowardsClosestTarget(shardSpeed);
        currentCharges--;

        if (!isCharging)
        {
            StartCoroutine(ShardRechargeCo());
        }

    }

    private IEnumerator ShardRechargeCo()
    {
        isCharging = true;

        while (currentCharges < maxCharges)
        {
            yield return new WaitForSeconds(cooldown);
            currentCharges++;
        }

        isCharging = false;
    }

    private void HandleShardMoving()//升级成跟踪属性
    {
        CreateShard();
        currentShard.MoveTowardsClosestTarget(shardSpeed);

        SetSkillOnCooldown();
    }

    private void HandleShardRegular()//默认等级
    {
        CreateShard();
        SetSkillOnCooldown();
    }

    public void CreateShard()
    {
        float detonationTime = GetDetonationTime();
            
        GameObject shard = Instantiate(shardPrefab,transform.position, Quaternion.identity);
        currentShard = shard.GetComponent<SkillObjective_Shard>();
        currentShard.SetupShard(this);

        if (IsUnlocked(SkillUpgradeType.Shard_Teleport) || IsUnlocked(SkillUpgradeType.Shard_TeleportAndHpRewind))
        {
            currentShard.OnExplode += ForceCoolDown;
        }
    }

    public void CreatRawShard()//创建基础shard
    {

        bool canMove = IsUnlocked(SkillUpgradeType.Shard_MoveToEnemy) || IsUnlocked(SkillUpgradeType.Shard_MultiCast);

        GameObject shard = Instantiate(shardPrefab, transform.position, Quaternion.identity);
        currentShard = shard.GetComponent<SkillObjective_Shard>();
        currentShard.SetupShard(this, detonationTime, canMove, shardSpeed);
    }

    public float GetDetonationTime()
    {
        if (IsUnlocked(SkillUpgradeType.Shard_Teleport) || IsUnlocked(SkillUpgradeType.Shard_TeleportAndHpRewind))
        {
            return shardExistDuration;
        }

        return detonationTime;
    }

    private  void ForceCoolDown()
    {
        if (!OnCooldown())
        {
            SetSkillOnCooldown();
            currentShard.OnExplode -= ForceCoolDown;
        }
    }
}
