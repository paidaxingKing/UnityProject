using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    private FileDataHandler dataHandler;
    private GameData gameData;
    private List<ISaveable> allSaveables;

    [SerializeField] private string fileName = "PDXGame.json";
    [SerializeField] private bool encrytData = true;


    private void Awake()
    {
        instance = this;
    }

    private IEnumerator Start()//unity引擎自动启动该协程
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath,fileName,encrytData);
        //Application.persistentDataPath是Unity 引擎提供的一个专门用来存放游戏存档、玩家设置等持久化数据的"官方安全目录"，自适应不同的平台
        allSaveables = FindISaveables();

        yield return new WaitForSeconds(0.01f);//等待其他组件都初始化完毕后再加载游戏数据，如果先加载数据再初始化，可能会导致数据被覆盖
        LoadGame();
    }

    public GameData GetGameData()
    {
        return gameData;
    }

    private void LoadGame()
    {
        gameData = dataHandler.LoadData();

        if (gameData == null)
        {
            Debug.Log("No saved data found,creating new save!");
            gameData = new GameData();
            return;
        }

        foreach (var saveable in allSaveables)
        {
            saveable.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        
        foreach (var saveable in allSaveables)
        {
            saveable.SaveData(ref gameData);
        }

        dataHandler.SaveData(gameData);
    }

    [ContextMenu("*** Delete save data ***")]
    public void DeleteSvaeData()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName,encrytData);
        dataHandler.Delete();
    }

    private void OnApplicationQuit()//关闭游戏时调用该方法
    {
        SaveGame();
    }

    private List<ISaveable> FindISaveables()
    {
        return FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None)//搜寻场景中所有的脚本组件,包括被禁用的组件，不需要排序
                .OfType<ISaveable>()//在所有脚本组件中过滤留下实现了ISaveable接口的组件
                .ToList();//最后打包成list返回
    }
}
