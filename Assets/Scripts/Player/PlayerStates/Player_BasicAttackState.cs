using UnityEngine;

public class Player_BasicAttackState : PlayerState
{
    private float attackVelocityTimer;//起到惯性的作用，当在奔跑时攻击，玩家会继续向前移动一小段距离
    private const int FirstComboIndex = 1;
    private int comboIndex = 1;
    private int comboMax = 3;

    private float lastTimeAttack;
    private bool comboAttackQueued; //当玩家在攻击动画的某个时刻按下攻击键，这个变量会被设置为true，表示玩家想要在当前攻击结束后立即进行下一次攻击
    private int attackDir;

    public Player_BasicAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        comboAttackQueued = false;

        ResetComboIndexIfNeeded();

        if (player.moveInput.x != 0)
        {
            attackDir = ((int)player.moveInput.x);
        }
        else
        {
            attackDir = player.facingDir;
        }
       
        anim.SetInteger("basicAttackIndex", comboIndex);

        ApplyAttackVelocity();

    }

    public override void Update()
    {
        base.Update();
        HandleAttackVelocity();

        if (input.Player.Attack.WasPressedThisFrame())
        {
            QueueNextAttack();
        }

        if (triggerCalled)
        {
            HandleStateExit();
        }
    }

    private void HandleStateExit()
    {
        if (comboAttackQueued)
        {
            anim.SetBool(animBoolName, false);
            player.EnterAttackStateWithDelay();
        }
        else
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        comboIndex++;
        lastTimeAttack = Time.time;
    }

    private void QueueNextAttack()
    {
        if (comboIndex < comboMax)
        {
            comboAttackQueued = true;
        }
    }

    private void ResetComboIndexIfNeeded()
    {
        //如果在comboResetTime时间内没有继续攻击，comboIndex就会重置为1
        //如果comboIndex超过comboMax，就会重置为1
        if (Time.time - lastTimeAttack > player.comboResetTime || comboIndex > comboMax)
        {
            comboIndex = FirstComboIndex;
        }
        
    }

    private void HandleAttackVelocity()
    {
        attackVelocityTimer -= Time.deltaTime;

        if (attackVelocityTimer < 0)
        {
            player.SetVelocity(0, rb.linearVelocity.y);
        }
    }

    private void ApplyAttackVelocity()
    {
        Vector2 attackVelocity = player.attackVelocity[comboIndex - 1];
        attackVelocityTimer = player.attackVelocityDuration;
        player.SetVelocity(attackVelocity.x * attackDir, attackVelocity.y);
    }
}
