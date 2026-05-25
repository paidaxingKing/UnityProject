using UnityEngine;

public class Object_Checkpoint : MonoBehaviour,ISaveable
{
    [SerializeField] private string checkpointId;
    [SerializeField] private Transform respawnPoint;

    public bool isActive { get; private set; }

    private Animator anim;
    private AudioSource fireAudioSource;

    private void Awake()
    {  
        anim = GetComponentInChildren<Animator>();
        fireAudioSource = GetComponent<AudioSource>();
    }

    private void OnValidate()
    {
        Debug.Log("ID:" + checkpointId);
#if UNITY_EDITOR//只在编译器环境下执行，根据逻辑可以判断出这类分配ID的方法只用执行一次即可，无需在打包后在游戏中再次执行
        if (string.IsNullOrEmpty(checkpointId))
        {
            checkpointId = System.Guid.NewGuid().ToString();
        }
#endif
    }

    public string GetCheckpointID()
    {
        return checkpointId;    
    }

    public Vector3 GetPosition()
    {
        return respawnPoint == null ? transform.position : respawnPoint.position;
    }

    public void ActivateCheckpoint(bool active)
    {
        isActive = active;
        anim.SetBool("isActive", active);
        if (active && !fireAudioSource.isPlaying)
        {
            fireAudioSource.Play();
        }
        else if (!active)
        {
            fireAudioSource.Stop();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //把当前检查点的位置保存在游戏数据中,注意不是调用SaveGame方法，SaveGame方法是把最后把游戏数据存放在本地文件中
        ActivateCheckpoint(true);//激活这次触碰到的检查点，关闭其他检查点
    }

    public void LoadData(GameData data)
    {
        Debug.Log(checkpointId);
        bool active = data.unLockedCheckpoints.TryGetValue(checkpointId, out active);
        ActivateCheckpoint(active);
    }
       

    public void SaveData(ref GameData data)
    {
        Debug.Log(checkpointId);
        if (!isActive)
        {
            return;
        }

        if (!data.unLockedCheckpoints.ContainsKey(checkpointId))
        {
            data.unLockedCheckpoints.Add(checkpointId,true);

        }
    }
}
