using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Audio DataBase")]
public class Audio_DataBaseSO : ScriptableObject//音频数据库
{
    public List<AudioClipData> player;//方便开发者浏览有哪些音频
    public List<AudioClipData> uiAduio;

    [Header("Music Lists")]
    public List<AudioClipData> mainMenuMusic;
    public List<AudioClipData> levelMusic;

    private Dictionary<string, AudioClipData> clipCollection;//实际用字典更容易应用音频

    private void OnEnable()//每次进入播放模式或修改内容时会执行
    {
        clipCollection = new Dictionary<string, AudioClipData>();
        AddToCollection(player);
        AddToCollection(uiAduio);
        AddToCollection(mainMenuMusic);
        AddToCollection(levelMusic);
    }

    public AudioClipData GetAudioData(string name)
    {
        return clipCollection.TryGetValue(name, out var data) ? data : null;
    }


    private void AddToCollection(List<AudioClipData> toAdd)
    {
        foreach (var data in toAdd)
        {
            if (data != null && !clipCollection.ContainsKey(data.audioName))
            {
                clipCollection.Add(data.audioName, data);
            }
        }
    }


}

[System.Serializable]
public class AudioClipData
{
    public string audioName;
    public List<AudioClip> clips = new List<AudioClip>();//AuidoClip是unity自带类
    [Range(0f,1f)] public float maxVolume = 1f;

    public AudioClip GetRandomClip()//随机播放同类型音频下的切片乐段
    {
        if (clips == null || clips.Count == 0) return null;

        return clips[Random.Range(0,clips.Count)];
    }
}

