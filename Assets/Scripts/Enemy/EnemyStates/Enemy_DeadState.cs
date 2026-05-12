using Unity.VisualScripting;
using UnityEngine;

public class Enemy_DeadState : EnemyState
{
    private Collider2D col;

    public Enemy_DeadState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        col = enemy.GetComponent<Collider2D>();
    }

    public override void Enter()
    {
        anim.enabled = false;//使动画定格在死亡帧
        col.enabled = false;//禁用碰撞体，使敌人不会被地面碰撞体阻挡
        enemy.GetComponent<EnemyExperienceReward>()?.GiveReward();

        rb.gravityScale = 12;//增加重力加速度，使敌人快速落地

        rb.linearVelocity = new Vector2(rb.linearVelocity.x,15);

        enemy.DestroyThis();
    }
}
