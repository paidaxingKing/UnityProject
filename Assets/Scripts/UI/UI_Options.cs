using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_Options : MonoBehaviour
{
    private Player player;

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private float miexerMutiplier = 25;

    [Header("BGM Volume Settings")]
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private string bgmParameter;

    [Header("SFX Volume Settings")]
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private string sfxParameter;

    private void Start()
    {
        player = FindFirstObjectByType<Player>();
    }

    public void BGMSliderValue(float value)//Slider组件调用该方法，传入value参数，区间是[0,1]
    {
        float newValue = Mathf.Log10(value) * miexerMutiplier;//Mathf.Log10()表示取以10为底的对数，用来表示分贝
        //参数被设为0表示声音没有衰减
        audioMixer.SetFloat(bgmParameter, newValue);
    }
    public void SFXSliderValue(float value)//Slider组件调用该方法，传入value参数，区间是[0,1]
    {
        float newValue = Mathf.Log10(value) * miexerMutiplier;//Mathf.Log10()表示取以10为底的对数，用来表示分贝
        //参数被设为0表示声音没有衰减
        audioMixer.SetFloat(sfxParameter, newValue);
    }

    public void GoMainMenuButton()
    {
        GameManager.instance.ChangeScene("MainMenu", RespawnType.None);
    }

    private void OnEnable()
    {
        sfxSlider.value = PlayerPrefs.GetFloat(sfxParameter,0.6f);
        bgmSlider.value = PlayerPrefs.GetFloat(bgmParameter, 0.6f);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(sfxParameter, sfxSlider.value);
        PlayerPrefs.SetFloat(bgmParameter, bgmSlider.value);
    }

    public void LoadUpVolume()
    {
        sfxSlider.value = PlayerPrefs.GetFloat(sfxParameter, 0.6f);
        bgmSlider.value = PlayerPrefs.GetFloat(bgmParameter, 0.6f);
    }

}
