using UnityEngine;
using UnityEngine.UI;

public class UI_TreeConnection : MonoBehaviour
{
    [SerializeField] private RectTransform rotationPoint;
    [SerializeField] private RectTransform connectionLine;
    [SerializeField] private RectTransform childNodeConnectionPoint;

    public void DirectConnection(NodeDirectionType direction,float length,float offset)
    {
        bool shouldBeActive = direction != NodeDirectionType.None;
        float finalLength = shouldBeActive ? length : 0;
        float angle = GetDirectionAngle(direction);

        rotationPoint.localRotation = Quaternion.Euler(0, 0, angle + offset);
        connectionLine.sizeDelta = new Vector2(finalLength, connectionLine.sizeDelta.y);
    }

    public Image GetConnectionLineImage()
    {
        return connectionLine.GetComponent<Image>();
    }

    public Vector2 GetConnectionPoint(RectTransform rect)//获得该边子节点相对于父节点的父物件的相对坐标，因为所有技能点都在canvas同一层级下，需要把其他技能点的位置设定成该边子节点位置
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle
            (
                rect.parent as RectTransform,
                childNodeConnectionPoint.position,//子节点世界坐标
                null,
                out var localPosition
            );

        return localPosition;
    }

    private float GetDirectionAngle(NodeDirectionType type)
    {
        switch (type)
        {
            case NodeDirectionType.UpLeft: return 135f;
            case NodeDirectionType.Up: return 90f;
            case NodeDirectionType.UpRight: return 45f;
            case NodeDirectionType.DownRight: return -45f;
            case NodeDirectionType.Down: return -90f;
            case NodeDirectionType.DownLeft:return -135f;
            case NodeDirectionType.Left:return -180f;
            default: return 0f;
        }
    }
}

//只在个别两个类中使用，所以可以不另创文件,防止文件太多
public enum NodeDirectionType
{
    None,
    UpLeft,
    Up,
    UpRight,
    Right,
    Left,
    Down,
    DownLeft,
    DownRight       
}
