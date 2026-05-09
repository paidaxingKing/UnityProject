using UnityEngine;

public class Player_CounterAttackState : PlayerState
{
    private Player_Combat combat;
    private bool isCounterAttacking;
    public Player_CounterAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        combat = player.GetComponent<Player_Combat>();
    }

    public override void Enter()
    {
        base.Enter();
        anim.SetBool("counterAttackPerformed", false);
        stateTimer = combat.GetCounterDuration();
        isCounterAttacking = combat.CounterAttack();
        anim.SetBool("counterAttackPerformed", isCounterAttacking);
    }

    public override void Update()
    {
        base.Update();
        rb.linearVelocity = Vector2.zero;
        if (triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }

        if (stateTimer < 0 && !isCounterAttacking)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
