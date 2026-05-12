using UnityEngine;

public class UI_ToolTip : MonoBehaviour
{
    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void ShowToolTip(bool show,RectTransform targetTransform)
    {
        if (!show)
        {
            rect.position = new Vector2(9999, 9999);
            return;
        }

        UpdatePosition(targetTransform);
    }

    public void UpdatePosition(RectTransform targetRect)
    {
        rect.position = targetRect.position;
    }
}
