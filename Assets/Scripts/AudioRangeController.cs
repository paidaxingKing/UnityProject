using UnityEngine;

public class AudioRangeController : MonoBehaviour//动态类型的音效大小随距离变化，常用于脚步声，燃烧声等会持续播放的音乐
{
    private AudioSource source;
    private Transform player;

    private float maxVolume;
    [SerializeField] private float minDistanceToHear = 12;
    [SerializeField] private bool showGizmos;


    private void Start()
    {
        player = Player.instance.transform;
       source = GetComponent<AudioSource>();

        maxVolume = source.volume;
    }

    private void Update()//每一帧动态计算扬声器到玩家的距离，根据距离调整音效大小
    {
        if (player == null) return;

        float distance = Vector2.Distance(player.position,transform.position);
        float ratio = Mathf.Clamp01(1 - (distance / minDistanceToHear));

        source.volume = Mathf.Lerp(0, maxVolume, ratio * ratio);
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos) return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, minDistanceToHear);
    }
}
