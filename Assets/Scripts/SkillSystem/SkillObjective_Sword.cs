using UnityEngine;

public class SkillObjective_Sword : SkillObjective_Base
{
    [Header("Normal Sword")]
    public float initialSpeed = 5f;
    public float maxSpeed = 100f;
    public float acceleration = 200f;

    [Header("Stick Behavior")]
    public float stuckDuration = 20f;  // 插在地上的持续时间
    private float stuckTimer;          // 倒计时器

    private int groundLayer;
    private bool isStuck = false;
    private bool isReturning = false;
    private Rigidbody2D rb;

    private float currentSpeed;
    private Skill_Sword swordManager;

    private float returnSpeed = 50f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Setup(Skill_Sword skillManager, float maxSpeed, float accleration)
    {
        swordManager = skillManager;
        playerStats = swordManager.player.entity_Stats;
        damageScaleData = swordManager.damageScaleData;
        this.acceleration = accleration;
        this.maxSpeed = maxSpeed;
    }

    private void Start()
    {
        Player.instance.sfx.PlayFlyingSword();
        groundLayer = LayerMask.NameToLayer("Ground");
        currentSpeed = initialSpeed;

        Transform target = ClosestTarget();

        if (target != null)
        {
            Vector2 direction = target.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else
        {
            transform.rotation = Player.instance.transform.rotation;
        }
    }

    private void Update()
    {
        if (isReturning)
        {
            ReturnToPlayer();
            return;
        }

        // 核心修改 1：如果是插在地上状态，执行倒计时逻辑
        if (isStuck)
        {
            stuckTimer -= Time.deltaTime;
            if (stuckTimer <= 0)
            {
                // 20秒超时，通知管理器解锁，并销毁自己
                RetrieveSword();
            }
            return; // 插在地上就不往下执行飞行逻辑了
        }

        FlyStraight();
    }

    private void RetrieveSword()
    {
        swordManager.RetrieveSword();
        if (swordManager.ShouldUpdateStuckList())
        {
            swordManager.DeleteStuckSword(this);
        }

        Destroy(gameObject);
    }

    public void StartReturn()
    {
        isStuck = false;
        isReturning = true;

        // 解除父子关系（从墙壁上拔下来）
        transform.SetParent(null);

        // 确保它是一个纯触发器，防止在飞回途中被墙壁的物理碰撞卡住
        GetComponent<Collider2D>().isTrigger = true;
        rb.bodyType = RigidbodyType2D.Kinematic;
        Player.instance.sfx.PlayFlyingSword();
    }

    private void ReturnToPlayer()
    {
        Vector2 targetPos = Player.instance.transform.position;

        transform.position = Vector2.MoveTowards(transform.position, targetPos, returnSpeed * Time.deltaTime);

        Vector2 direction = (targetPos - (Vector2)transform.position).normalized;
        if (direction != Vector2.zero)
        {
            transform.right = direction;
        }

        // 检查是否已经飞回到玩家手中 (距离小于 1 个单位就算接触到了)
        if (Vector2.Distance(transform.position, targetPos) < 1f)
        {
            RetrieveSword();
        }
    }

    private void FlyStraight()
    {
        currentSpeed += acceleration * Time.deltaTime;
        currentSpeed = Mathf.Min(currentSpeed, maxSpeed);
        rb.linearVelocity = transform.right * currentSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 核心修改 2：如果已经插在地上了，只检测玩家是否来捡剑
        if (isStuck)
        {
            if (collision.GetComponent<Player>() != null)
            {
                // 玩家碰到了插在地上的剑 -> 捡起
                RetrieveSword();
            }
            return; // 已经插在地上，不再触发其他伤害或碰地逻辑
        }

        // 飞行过程中的伤害判定（正常执行）
        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null && collision.GetComponent<Player>() == null)
        {
            AttackData attackData = playerStats.GetAttackData(damageScaleData);
            float physicalDamage = attackData.physicalDamage;
            float elementalDamage = attackData.elementalDamage;
            ElementType elementType = attackData.elementType;

            damageable.BeDamaged(physicalDamage, elementalDamage, elementType, transform);

            Entity_StatusHandler statusHandler = collision.GetComponent<Entity_StatusHandler>();
            if (statusHandler != null && elementType != ElementType.None)
            {
                statusHandler.ApplyStatusEffect(elementType, attackData.effectData);
            }
        }

        // 撞击地面的判定（正常执行）
        if (collision.gameObject.layer == groundLayer)
        {
            StickToGround(collision.transform);
        }
    }

    private void StickToGround(Transform groundTransform)
    {
        isStuck = true;
        stuckTimer = stuckDuration; // 开启20秒倒计时

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.bodyType = RigidbodyType2D.Kinematic;

        GetComponent<Collider2D>().isTrigger = true;

        transform.SetParent(groundTransform);
        if (swordManager.ShouldUpdateStuckList())
        {
            swordManager.AddStuckSword(this);
        }
    }
}