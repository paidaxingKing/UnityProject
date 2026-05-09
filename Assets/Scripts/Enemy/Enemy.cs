using System.Collections;
using UnityEngine;

public class Enemy : Entity
{
    public Enemy_IdleState idleState;
    public Enemy_MoveState moveState;
    public Enemy_AttackState attackState;
    public Enemy_BattleState battleState;
    public Enemy_DeadState deadState;
    public Enemy_StunnedState stunnedState;

    [Header("Battle Details")]
    public float battleMoveSpeed = 3;
    public float attackDistance = 2;
    public float battleTimeDuration = 5;
    public float minRetreatDistance = 1;
    public Vector2 retreatVelocity;

    [Header("Stunned State Details")]
    public float stunnedDuration = 0.1f;
    public Vector2 stunnedVelocity = new Vector2(7, 7);
    [SerializeField] protected bool canBeStunned = false;

    [Header("Movement Details")]
    public float moveSpeed;
    public float idleTime;
    [Range(0, 2)]
    public float moveAnimSpeedMultiplier;

    [Header("Player Detection")]
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private Transform playerCheck;
    [SerializeField] private float playerCheckDistance;
    public Transform player { get; private set; }

    protected override IEnumerator SlowDownEntityCo(float duration, float slowMultiplier)
    {
        float originalMoveSpeed = moveSpeed;
        float originalBattleMoveSpeed = battleMoveSpeed;
        float originalAnimSpeed = anim.speed;

        float speedMultiplier = 1 - slowMultiplier;

        moveSpeed *= speedMultiplier;
        battleMoveSpeed *= speedMultiplier;
        anim.speed *= speedMultiplier;

        yield return new WaitForSeconds(duration);

        moveSpeed = originalMoveSpeed;
        battleMoveSpeed = originalBattleMoveSpeed;
        anim.speed = originalAnimSpeed;
    }

    public void EnableCounterWindow(bool enable)
    {
        canBeStunned = enable;
    }


    protected override void Update()
    {
        base.Update();
    }

    public override void EntityDeath()
    {
        base.EntityDeath();

        stateMachine.ChangeState(deadState);
    }

    private void HandlePlayerDeath()
    {
        stateMachine.ChangeState(idleState);
    }

    public Transform GetPlayerTransform()
    {
        if (player == null)
        {
            player = PlayerDetection().transform;
        }
        return player;
    }

    public void TryEnterBattleState(Transform player)
    {
        if (stateMachine.currentState == battleState || stateMachine.currentState == attackState)
        {
            return;
        }
        this.player = player;
        stateMachine.ChangeState(battleState);
    }

    public RaycastHit2D PlayerDetection()
    {
        RaycastHit2D hit = Physics2D.Raycast(playerCheck.position, Vector2.right * facingDir, playerCheckDistance, whatIsPlayer | whatIsGround);

        if (hit.collider == null || hit.collider.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            return default;
        }
        return hit;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.red;
        Gizmos.DrawLine(playerCheck.position, playerCheck.position + new Vector3(playerCheckDistance * facingDir, 0));
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(playerCheck.position, playerCheck.position + new Vector3(attackDistance * facingDir, 0));
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(playerCheck.position, playerCheck.position + new Vector3(minRetreatDistance * facingDir, 0));
    }

    private void OnEnable()
    {
        Player.OnPlayerDeath += HandlePlayerDeath;
    }

    private void OnDisable()
    {
        Player.OnPlayerDeath -= HandlePlayerDeath;
    }

}
