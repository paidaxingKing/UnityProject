using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UI_TreeNode : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerDownHandler
{
    [SerializeField] private Skill_DataSO skillData;
    [SerializeField] private string skillName;
    [SerializeField] private Image skillIcon;
    [SerializeField] private string lockedColorHex = "#808080";
    private Color lastColor;
    public bool isUnlocked;
    public bool isLocked;

    //在Unity 编辑器的 Inspector 面板中修改了脚本的变量，或者脚本所在的物体被加载时，该方法会被自动调用。
    private void OnValidate()
    {
        if (skillData == null)
            return;

        skillName = skillData.skillName;
        skillIcon.sprite = skillData.icon;
        gameObject.name = "UI_TreeNode - " + skillName;
    }

    public void Awake()
    {
        UpdateIconColor(GetColorByHex(lockedColorHex));
    }

    private void UpdateIconColor(Color color)
    {
        if (skillIcon != null)
        {
            lastColor = skillIcon.color;
            skillIcon.color = color;
        }
    }

    private void Unlock()
    {
        isUnlocked = true;
        UpdateIconColor(Color.white);
    }

    private bool CanBeUnlocked()
    {
        if (isUnlocked || isLocked) return false;

        return true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (CanBeUnlocked())
        {
            Unlock();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)//当鼠标进入区域时，调用该方法
    {
        if (!isUnlocked)
        {
            UpdateIconColor(Color.white * 0.85f);
        }    
    }

    public void OnPointerExit(PointerEventData eventData)// 当鼠标离开该区域时，调用该方法
    {
        if (!isUnlocked)
        {
            UpdateIconColor(lastColor);
        }
        
    }

    private Color GetColorByHex(string hex)
    {
        Color color;
        ColorUtility.TryParseHtmlString(hex, out color);

        return color;
    }
}
