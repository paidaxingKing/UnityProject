using UnityEngine;

public class UI_ToolTip : MonoBehaviour
{
    private RectTransform rect;

    [SerializeField] private Vector2 offset = new Vector2(300,20);

    protected virtual void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public virtual void ShowToolTip(bool show,RectTransform targetTransform)
    {
        if (!show)
        {
            rect.position = new Vector2(9999, 9999);
            return;
        }

        UpdatePosition(targetTransform);
    }

    protected string GetColoredText(string color, string text)
    {
        return $"<color={color}>{text}</color>";
    }

    public void UpdatePosition(RectTransform targetRect)//保证提示栏不会超出屏幕外
    {
        float screenCenterX = Screen.width / 2;
        float screenTop = Screen.height;
        float screenBottom = 0;

        Vector2 targetPosition = targetRect.position;

        
        targetPosition.x = targetPosition.x > screenCenterX ? targetPosition.x - offset.x : targetPosition.x + offset.x;

        float verticalHalf = rect.sizeDelta.y / 2f;
        float topY = targetPosition.y + verticalHalf;
        float bottomY = targetPosition.y - verticalHalf;

        if (topY > screenTop)
        {
            targetPosition.y = targetPosition.y - verticalHalf - offset.y;
        }
        if (bottomY < screenBottom)
        {
            targetPosition.y = targetPosition.y + verticalHalf + offset.y;
        }

        rect.position = targetPosition;
    }
}
