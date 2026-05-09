using UnityEngine;

public class Player_JumpAttackState : PlayerState
{

    private bool touchGround;
    public Player_JumpAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        touchGround = false;

        player.SetVelocity(player.jumpAttackVelocity.x * player.facingDir, player.jumpAttackVelocity.y);
    }

    public override void Update()
    {
        base.Update();
        //如果不加touchGround，会导致当玩家在地面的攻击动画播放结束前，会反复执行anim.SetTrigger("jumpAttackTrigger")
        //而此时动画状态机不会经过含有jumpAttackTrigger的过渡条件，会导致jumpAttackTrigger一直处于触发状态，无法重置
        if (player.groundDetected && !touchGround)
        {          
            touchGround = true;
            anim.SetTrigger("jumpAttackTrigger");
            player.SetVelocity(0, rb.linearVelocity.y);
        }

        if (triggerCalled && player.groundDetected)
        {
           stateMachine.ChangeState(player.idleState);
        }
    }
}
