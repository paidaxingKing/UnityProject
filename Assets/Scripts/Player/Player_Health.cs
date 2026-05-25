using UnityEngine;

public class Player_Health : Entity_Health
{
    private Player player;

    protected override void Awake()
    {
        base.Awake();
        player = GetComponentInParent<Player>();
    }

    private void Update()
    {
        
    }

    protected override void Die()
    {
        base.Die();
        player.ui.OpenDeathScreenUI();
        //GameManager.instance.SetLastPlayerPosition(transform.position);
        //GameManager.instance.RestartScene();
    }
}
