using UnityEngine;
using UnityEngine.SceneManagement;

public class Object_Waypoint : MonoBehaviour
{
    [SerializeField] private string transferToScene;

    [Space]
    public RespawnType waypointType;
    [SerializeField] private RespawnType connectedWaypointType;
    [SerializeField] private bool canBeTrigger = true;

    private void OnValidate()
    {
        gameObject.name = "Object_Waypoint - " + waypointType.ToString() + " - " + transferToScene;

        if (waypointType == RespawnType.Enter)
        {
            connectedWaypointType = RespawnType.Exit;
        }
        else if (waypointType == RespawnType.Exit)
        {
            connectedWaypointType = RespawnType.Enter;
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)//条件触发传送，例如只有打完BOSS后才能触发离开场景的传送
    {
        if (!canBeTrigger) return;

        SaveManager.instance.SaveGame();

        SceneManager.LoadScene(transferToScene);
        //当进入一个游戏场景时，各个GameObject的Awake方法和Start等方法都会自动执行
        //会自动调用SaveManager的Start协程，加载之前保存的数据，玩家就会出现在新场景的对应传送点
    }

    private void OnTriggerExit2D(Collider2D collision)//相当于离开重生点后，该传送点即可随意传送
    {
        canBeTrigger = true;

    }


}
