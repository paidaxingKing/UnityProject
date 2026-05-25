using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour//负责播放背景音乐和全局音效
{
    public static AudioManager instance;

    [SerializeField] private Audio_DataBaseSO audio_DataBase;
    [SerializeField] private AudioSource bgmSource;//音频源，即扬声器，为其分配AudioClip即可播放该音效
    [SerializeField] private AudioSource sfxSource;//用于UI点击之类的

    private Transform player;
    private AudioClip lastMusicPlayed;

    private string currentBgmGroupName;
    private Coroutine currentBgmCo;
    [SerializeField] private bool bgmShouldPlay;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (!bgmSource.isPlaying && bgmShouldPlay)
        {
            if (!string.IsNullOrEmpty(currentBgmGroupName))
            {
                NextBGM(currentBgmGroupName);
            }
            
        }

        if (bgmSource.isPlaying && !bgmShouldPlay)
        {
            StopBGM();
        }
    }

    public void StopBGM()
    {
        bgmShouldPlay = false;

        StartCoroutine(FadeVolumeCO(bgmSource, 0, 1));

        if (currentBgmCo != null)
        {
            StopCoroutine(currentBgmCo);//若当前正在执行切换音乐的协程，就停止切换音乐
        }
    }

    public void StartBGM(string musicGroup)
    {
        bgmShouldPlay = true;

        if (musicGroup == currentBgmGroupName)
        {
            return;
        }

        NextBGM(musicGroup);
    }

    public void NextBGM(string musicGroup)
    {
        bgmShouldPlay = true;
        currentBgmGroupName = musicGroup;

        if (currentBgmCo != null)
        {
            StopCoroutine(currentBgmCo);
        }

        currentBgmCo =  StartCoroutine(SwitchMusicCO(musicGroup));
    }

    private IEnumerator SwitchMusicCO(string musicGroup)
    {
        AudioClipData data = audio_DataBase.GetAudioData(musicGroup);

        if (data == null || data.clips.Count == 0)
        {
            yield break;
        }

        AudioClip nextMusic = data.GetRandomClip();

        if (data.clips.Count > 1)
        {
            while (nextMusic == lastMusicPlayed)
            {
                nextMusic = data.GetRandomClip();
            }
        }
        
        if (bgmSource.isPlaying)
        {
            yield return FadeVolumeCO(bgmSource, 0, 1f);
            //(注意：直接写 yield return FadeVolumeCO(); 在 Unity 中是完全可以的，效果一样且更省性能)
            //会开启这个协程，与StartCoroutine(FadeVolumeCO())一样
        }

        lastMusicPlayed = nextMusic;
        bgmSource.clip = nextMusic;//切换clip后扬声器会停止，需要再次打开
        bgmSource.volume = 0;
        bgmSource.Play();

        StartCoroutine(FadeVolumeCO(bgmSource,data.maxVolume, 1f));
    }

    private IEnumerator FadeVolumeCO(AudioSource source,float targetVolume,float duration)
    {
        float time = 0;
        float startVolume = source.volume;

        while (time < duration)
        {
            time += Time.deltaTime;

            source.volume = Mathf.Lerp(startVolume, targetVolume, time/duration);
            yield return null;
        }
        source.volume = targetVolume;
    }

    public void PlaySFX(string soundName,AudioSource sfxSource,float minDistanceToHearSound = 5)
    {
        if (player == null)
        {
            player = Player.instance.transform;
        }

        var data = audio_DataBase.GetAudioData(soundName);
        if (data == null)
        {
            Debug.Log("找不到音效: - " + soundName);
            return;
        }

        var clip = data.GetRandomClip();
        if (clip == null) return;

        float maxVolume = data.maxVolume;
        float distance = Vector2.Distance(sfxSource.transform.position, player.position);
        float ratio = Mathf.Clamp01(1 - distance / minDistanceToHearSound);

        sfxSource.pitch = Random.Range(0.95f, 1.1f);//pitch: 音高
        sfxSource.volume = Mathf.Lerp(0, maxVolume, ratio * ratio);
        sfxSource.PlayOneShot(clip);//播放一次
    }

    public void PlayGlobalSFX(string soundName)
    {
        var data = audio_DataBase.GetAudioData(soundName);
        if (data == null) return;

        var clip = data.GetRandomClip();
        if (clip == null) return;
        sfxSource.volume = data.maxVolume;
        sfxSource.PlayOneShot(clip);
    }

}
