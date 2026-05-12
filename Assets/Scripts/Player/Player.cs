using System.Collections;
using UnityEngine;
using System;

public class Player : Entity
{
    public static event Action OnPlayerDeath;

    public PlayerInputSet input { get; private set; }
    public Vector2 moveInput { get; private set; }
    public Player_SkillManager skillManager { get; private set; }

    #region State Variables
    public Player_IdleState idleState { get; private set; }
    public Player_MoveState moveState { get; private set; }
    public Player_JumpState jumpState { get; private set; }
    public Player_FallState fallState { get; private set; }
    public Player_WallSlideState wallSlideState { get; private set; }
    public Player_WallJumpState wallJumpState { get; private set; }
    public Player_DashState dashState { get; private set; }
    public Player_BasicAttackState basicAttackState { get; private set; }
    public Player_JumpAttackState jumpAttackState { get; private set; }
    public Player_DeadState deadState { get; private set; } 
    public Player_CounterAttackState counterAttackState { get; private set; }
    public Player_HealState healState { get; private set; }
    #endregion

    [Header("Attack Details")]
    public Vector2[] attackVelocity;
    public Vector2 jumpAttackVelocity;
    public float attackVelocityDuration = 0.1f;
    public float comboResetTime = 0.8f;
    private Coroutine queuedAttackCo;


    [Header("Movement Details")]
    public float moveSpeed;
    public float jumpForce;
    public Vector2 wallJumpForce;

    [Range(0, 1)]
    public float inAirMoveSpeedMultiplier = 0.6f;
    [Range(0, 1)]
    public float wallSlideSlowMultiplier = 0.3f;
    [Space]
    public float dashDuration = 0.25f;
    public float dashSpeed = 20;

    protected override void Awake()
    {
        base.Awake();
        input = new PlayerInputSet();
        skillManager = GetComponent<Player_SkillManager>();

        idleState = new Player_IdleState(this, stateMachine, "idle");
        moveState = new Player_MoveState(this, stateMachine, "move");
        jumpState = new Player_JumpState(this, stateMachine, "jumpFall");
        fallState = new Player_FallState(this, stateMachine, "jumpFall");
        wallSlideState = new Player_WallSlideState(this, stateMachine, "wallSlide");
        wallJumpState = new Player_WallJumpState(this, stateMachine, "jumpFall");
        dashState = new Player_DashState(this, stateMachine, "dash");
        basicAttackState = new Player_BasicAttackState(this, stateMachine, "basicAttack");
        jumpAttackState = new Player_JumpAttackState(this, stateMachine, "jumpAttack");
        deadState = new Player_DeadState(this, stateMachine, "dead");
        counterAttackState = new Player_CounterAttackState(this, stateMachine, "counterAttack");
        healState = new Player_HealState(this, stateMachine, "heal");
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base .Update();
    }

    protected override IEnumerator SlowDownEntityCo(float duration, float slowMultiplier)
    {
        float originalMoveSpeed = moveSpeed;
        float originalJumpForce = jumpForce;
        float originalAnimSpeed = anim.speed;

        Vector2[] originalAttackVelocity = (Vector2[])attackVelocity.Clone();
        Vector2 originalJumpAttackVelocity = jumpAttackVelocity;
        Vector2 originalWallJumpForce = wallJumpForce;

        float speedMultiplier = 1 - slowMultiplier;

        moveSpeed *= speedMultiplier;
        jumpForce *= speedMultiplier;
        jumpAttackVelocity *= speedMultiplier;
        wallJumpForce *= speedMultiplier;
        anim.speed *= speedMultiplier;

        for (int i = 0; i < attackVelocity.Length; i++)
        {
            attackVelocity[i] *= speedMultiplier;
        }

        yield return new WaitForSeconds(duration);

        moveSpeed = originalMoveSpeed;
        jumpForce = originalJumpForce;
        jumpAttackVelocity = originalJumpAttackVelocity;
        wallJumpForce = originalWallJumpForce;
        anim.speed = originalAnimSpeed;

        for (int i = 0; i < attackVelocity.Length; i++)
        {
            attackVelocity[i] = originalAttackVelocity[i];
        }
    }

    public override void EntityDeath()
    {
        base.EntityDeath();

        OnPlayerDeath?.Invoke();
        stateMachine.ChangeState(deadState);
    }

    public void EnterAttackStateWithDelay()
    {
        if (queuedAttackCo != null)
        {
            StopCoroutine(queuedAttackCo);
        }
        queuedAttackCo = StartCoroutine(EnterAttackStateWithDelayCo());
    }

    private IEnumerator EnterAttackStateWithDelayCo()
    {
        yield return new WaitForEndOfFrame();
        stateMachine.ChangeState(basicAttackState);
    }

    private void OnEnable()
    {
        input.Enable();

        input.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.Player.Move.canceled += ctx => moveInput = Vector2.zero;
    }

    private void OnDisable()
    {
        input.Disable();
    }


}
