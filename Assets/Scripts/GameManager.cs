using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

//自定义场景切换的类，因为要实现场景切换的过渡和位置设置
public class GameManager : MonoBehaviour,ISaveable
{
    public static GameManager instance;
    private Vector3 lastPlayerPosition;
    private string lastScenePlayed;
    private bool dataLoaded;


    //public void SetLastPlayerPosition(Vector3 position)
    //{
    //    lastPlayerPosition = position;
    //}
    

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        lastScenePlayed = "Level_0";//默认在初始地
        DontDestroyOnLoad(gameObject);//当切换场景时，该gameobject不会被销毁，即GameManager是全局的只存在一个实例
    }

    public void SetDefaultScene()
    {
        lastScenePlayed = "Level_0";
    }

    public void ContinuePlay()
    {
        Debug.Log(Time.time + " " + lastScenePlayed);
        ChangeScene(lastScenePlayed,RespawnType.None);
    }

    public void RestartScene()
    {
        SaveManager.instance.SaveGame();

        string senceName = SceneManager.GetActiveScene().name;
        ChangeScene(senceName, RespawnType.None);
    }

    public void ChangeScene(string sceneName,RespawnType respawnType)
    {

        SaveManager.instance.SaveGame();
        Time.timeScale = 1f;//因为显示选项UI时， Time.timeScale被设为0
                            //而下述协程中存在协程等待的情况，因此在时间暂停时，协程会一直等待
        StartCoroutine(ChangeSceneCO(sceneName,respawnType));//用协程实现的原因，是为了实现专场间的过渡效果
    }

    private UI_FadeScreen FindFadeScreenUI()
    {
        if (UI.instance != null)
        {
            return UI.instance.fadeScreenUI;
        }
        else
            return FindFirstObjectByType<UI_FadeScreen>();
    }

    private IEnumerator ChangeSceneCO(string sceneName, RespawnType respawnType)
    {
        UI_FadeScreen fadeScreen = FindFadeScreenUI();
        // Fade Effect
        fadeScreen.DoFadeOut();//透明变黑
        yield return fadeScreen.fadeEffectCO;//表示等该协程执行结束才执行下列代码

        SceneManager.LoadScene(sceneName);//Unity自带类，实现场景切换的功能
        //切换场景实际上会把先前场景的游戏物件都销毁，然后在新场景创建新的游戏物件，所以会有新的实例
        //所以不能在跨场景活跃的实例中固定引用会被销毁的实例
        //单例模式也是，单例是在游戏加载时，单例变量引用unity自动创建的实例(monobehavior脚本文件会被unity自动创建实例)

        dataLoaded = false;
        yield return null;

        while (dataLoaded == false)//等待数据都加载完之后，才开始淡入进入场景
        {
            yield return null;
        }

        fadeScreen = FindFadeScreenUI();//新场景下，之前的实例被销毁了
        fadeScreen.DoFadeIn();//黑变透明

        Player player = Player.instance;
        if (player == null) yield break;


        Debug.Log(SaveManager.instance.GetGameData() == null);
        Vector3 position = GetNewPlayerPosition(respawnType);
        //此时gamemanager已经转移到新场景了，所以是在新场景里面找位置，而不是之前的场景里找位置

        if (position != Vector3.zero)
        {
            player.TeleportPlayer(position);
        }
    }

    private Vector3 GetNewPlayerPosition(RespawnType type)
    {
        if (type == RespawnType.None)
        {
            var data = SaveManager.instance.GetGameData();
            //获得所有检查点
            var checkpoints = FindObjectsByType<Object_Checkpoint>(FindObjectsSortMode.None);
            var unlockedCheckpoints = checkpoints
                .Where(cp => data.unLockedCheckpoints.TryGetValue(cp.GetCheckpointID(), out bool unlocked) && unlocked)
                .Select(cp => cp.GetPosition())
                .ToList();
            //遍历场景中的每一个检查点，根据保存数据中的字典，判断检查点是否被激活
            //最后获取到每个检查点的重生位置，存放在列表中

            var enterWaypoints = FindObjectsByType<Object_Waypoint>(FindObjectsSortMode.None)
                .Where(wp => wp.GetRespawnType() == RespawnType.Enter)
                .Select(wp => wp.GetPosition())
                .ToList();

            var selectedPositions = unlockedCheckpoints.Concat(enterWaypoints).ToList();//concat：连接两个列表

            if (selectedPositions.Count == 0)
            {
                return Vector3.zero;
            }

            return selectedPositions.OrderBy(position => Vector3.Distance(position, lastPlayerPosition)).First();
            //默认升序排序，玩家死亡后传送到距离其最近的检查点或者关卡入口
        }

        return GetWaypointPosition(type);
    }

    private Vector3 GetWaypointPosition(RespawnType respawnType)
    {
        var waypoints = FindObjectsByType<Object_Waypoint>(FindObjectsSortMode.None);

        foreach (var point in waypoints)
        {
            if (point.GetRespawnType() == respawnType)
            {
                return point.GetPosition();
            }
        }
        return Vector3.zero;
    }

    public void LoadData(GameData data)
    {
        lastScenePlayed = data.lastScenePlayed;
        lastPlayerPosition = data.lastPlayerPosition;
        dataLoaded = true;//数据加载完了
    }

    public void SaveData(ref GameData data)
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "MainMenu") return;

        data.lastPlayerPosition = Player.instance.transform.position;
        data.lastScenePlayed = currentScene;
        dataLoaded = false;
    }

}
