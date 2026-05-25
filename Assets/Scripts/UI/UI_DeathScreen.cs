using UnityEngine;

public class UI_DeathScreen : MonoBehaviour
{

    public void Restart()
    {
        GameManager.instance.RestartScene();
    }

    public void GoToTrainingGround()
    {
        GameManager.instance.ChangeScene("Level_0", RespawnType.Enter);
    }


    public void GoToMainMenu()
    {
        GameManager.instance.ChangeScene("MainMenu", RespawnType.None);
    }
}
