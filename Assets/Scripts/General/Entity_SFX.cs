using UnityEngine;

public class Entity_SFX : MonoBehaviour
{
    protected AudioSource audioSource;//每个角色身上挂载一个扬声器用来播放音乐

    [Header("SFX Names")]
    [SerializeField] private string attackHit;
    [SerializeField] private string attackMiss;

    [SerializeField] protected float soundDistance;
    [SerializeField] private bool showGizmos;

    private void Awake()
    {
        audioSource = GetComponentInChildren<AudioSource>();
    }

    public void PlayAttackHit()
    {
        AudioManager.instance.PlaySFX(attackHit, audioSource,soundDistance);
    }

    public void PlayAttackMiss()
    {
        AudioManager.instance.PlaySFX(attackMiss, audioSource,soundDistance);
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos) return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, soundDistance);
    }
}
