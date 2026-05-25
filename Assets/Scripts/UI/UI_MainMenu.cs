using UnityEngine;

public class UI_MainMenu : MonoBehaviour
{

    private void Start()
    {
        transform.root.GetComponentInChildren<UI_FadeScreen>().DoFadeIn();
        AudioManager.instance.StartBGM("player_mainMenu");
        transform.root.GetComponentInChildren<UI_Options>(true).LoadUpVolume();

    }

    public void PlayButton()
    {
        AudioManager.instance.PlayGlobalSFX("button_click");
        GameManager.instance.ContinuePlay();
    }

    public void PlayOptions()
    {
        AudioManager.instance.PlayGlobalSFX("button_click");
    }

    public void QuitButton()
    {
        AudioManager.instance.PlayGlobalSFX("button_click");
        Application.Quit();//退出应用
        //在 Unity 编辑器里时，Application.Quit() 不会关闭编辑器，也不会停止 Play 模式
        //在打包后的程序里生效,点击后退出应用程序
    }
}
