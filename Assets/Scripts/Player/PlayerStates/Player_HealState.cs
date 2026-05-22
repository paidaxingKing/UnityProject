using UnityEngine;

public class Player_HealState : PlayerState
{
    public Player_HealState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        player.entity_Health.AddHp(100);
        player.SetVelocity(0,0);
    }

    public override void Update()
    {
        base.Update();
        player.SetVelocity(0, 0);
       
        if (triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
