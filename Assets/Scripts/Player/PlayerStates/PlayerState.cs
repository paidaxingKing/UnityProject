using Unity.VisualScripting;
using UnityEngine;

public abstract class PlayerState : EntityState
{
    protected Player player;
    protected PlayerInputSet input;
   
    public PlayerState(Player player,StateMachine stateMachine, string animBoolName) : base(stateMachine, animBoolName)
    {
        this.player = player;

        anim = player.anim;
        rb = player.rb;
        input = player.input;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base .Update();
        
        if (input.Player.Dash.WasPressedThisFrame())
        {
            stateMachine.ChangeState(player.dashState);
        }
    }

    public override void UpdateAnimationParameters()
    {
        base.UpdateAnimationParameters();
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

    private bool CanDash()
    {
        return true;
    }
}
