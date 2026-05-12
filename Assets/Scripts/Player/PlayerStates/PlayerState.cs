using Unity.VisualScripting;
using UnityEngine;

public abstract class PlayerState : EntityState
{
    protected Player player;
    protected PlayerInputSet input;
    protected Player_SkillManager skillManager;
   
    public PlayerState(Player player,StateMachine stateMachine, string animBoolName) : base(stateMachine, animBoolName)
    {
        this.player = player;

        anim = player.anim;
        rb = player.rb;
        input = player.input;
        skillManager = player.skillManager;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base .Update();
        
        if (input.Player.Dash.WasPressedThisFrame() && CanDash())
        {
            skillManager.dash.SetSkillOnCooldown();
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
        if (!skillManager.dash.CanUseSkill()) return false;
        return true;
    }
}
