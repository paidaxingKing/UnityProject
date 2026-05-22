using UnityEngine;

public class Object_Checkpoint : MonoBehaviour,ISaveable
{

    private Object_Checkpoint[] allCheckpoints;
    private Animator anim;
    private Player player;

    private void Awake()
    {
        //在整个项目中找所有检查点
        allCheckpoints = FindObjectsByType<Object_Checkpoint>(FindObjectsSortMode.None);//不对找到的对象进行排序
        anim = GetComponentInChildren<Animator>();
    }

    public void ActivateCheckpoint(bool active)
    {
        anim.SetBool("isActive", active);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (var checkpoint in allCheckpoints)
        {
            checkpoint.ActivateCheckpoint(false);
        }

        SaveManager.instance.GetGameData().savedCheckpoint = transform.position;
        //把当前检查点的位置保存在游戏数据中,注意不是调用SaveGame方法，SaveGame方法是把最后把游戏数据存放在本地文件中
        ActivateCheckpoint(true);//激活这次触碰到的检查点，关闭其他检查点
    }

    public void LoadData(GameData data)
    {
        bool active = data.savedCheckpoint == transform.position;
        ActivateCheckpoint(active);
        if (active)
        {
            Player.instance.TeleportPlayer(transform.position);
        }
    }
       

    public void SaveData(ref GameData data)
    {
        
    }
}
