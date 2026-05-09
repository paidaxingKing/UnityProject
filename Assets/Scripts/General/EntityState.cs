using UnityEngine;
using UnityEngine.Windows;

public class EntityState
{
    protected string animBoolName;
    protected Animator anim;
    protected Rigidbody2D rb;
    protected StateMachine stateMachine;

    protected float stateTimer;
    protected bool triggerCalled;

    public EntityState(StateMachine stateMachine, string animBoolName)
    {
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        anim.SetBool(animBoolName, true);
        triggerCalled = false;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
        UpdateAnimationParameters();
    }

    public virtual void Exit()
    {
        anim.SetBool(animBoolName, false);
    }

    public void AnimationTrigger()
    {
        triggerCalled = true;
    }

    public virtual void UpdateAnimationParameters()
    {

    }

}
