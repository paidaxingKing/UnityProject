using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UI_TreeNode : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerDownHandler
{
    private UI ui;//用父物体作为媒介，获得处于同一层级下的UI_SkillToolTip，UI脚本作为中转站
    private RectTransform rectTransform;
    private UI_SkillTree skillTree;
    private UI_TreeConnectionHandler connectionHandler;

    [Header("Unlock Details")]
    public UI_TreeNode[] needNodes;
    public UI_TreeNode[] conflictNodes;
    public bool isUnlocked;
    public bool isLocked;

    [Header("Skill Details")]
    public Skill_DataSO skillData;
    [SerializeField] private string skillName;
    [SerializeField] private Image skillIcon;
    [SerializeField] private int skillCost;
    [SerializeField] private string lockedColorHex = "#808080";
    private Color highlightColor = Color.white * 0.85f;


    //在Unity 编辑器的 Inspector 面板中修改了脚本的变量，或者脚本所在的物体被加载时，该方法会被自动调用。
    private void OnValidate()
    {
        if (skillData == null)
            return;

        skillName = skillData.skillName;
        skillIcon.sprite = skillData.icon;
        skillCost = skillData.cost;
        gameObject.name = "UI_TreeNode - " + skillName;
    }

    public void InitializeUI_TreeNode()
    {
        ui = GetComponentInParent<UI>();
        UpdateIconColor(GetColorByHex(lockedColorHex));
        skillTree = GetComponentInParent<UI_SkillTree>(true);
        rectTransform = GetComponent<RectTransform>();
        connectionHandler = GetComponent<UI_TreeConnectionHandler>();
    }

    private void UpdateIconColor(Color color)
    {
        if (skillIcon != null)
        {           
            skillIcon.color = color;
        }
    }

    public void Refund()
    {
        isUnlocked = false;
        UpdateIconColor(GetColorByHex(lockedColorHex));

        skillTree.AddSkillPoint(skillCost);
        connectionHandler.UnlockConnectionImage(false);
        UnlockConflictNodes();
    }

    private void Unlock()
    {
        isUnlocked = true;
        skillTree.ReduceSkillPoint(skillData.cost);
        LockConflictNodes();

        UpdateIconColor(Color.white);
        connectionHandler.UnlockConnectionImage(true);

        skillTree.skillManager.GetSkillByType(skillData.skillType).SetSkillUpgrade(skillData);
    }

    public void UnlockWithSaveData()
    {
        isUnlocked = true;
        LockConflictNodes();
        UpdateIconColor(Color.white);
        connectionHandler.UnlockConnectionImage(true);
    }

    private void UnlockConflictNodes()
    {
        foreach (var node in conflictNodes)
        {
            node.isLocked = false;
        }
    }

    private void LockChildNodes()
    {
        foreach (var node in connectionHandler.GetChildNodes())
        {
            node.isLocked = true;
            node.LockChildNodes();
        }
    }

    private void LockConflictNodes()
    {
        foreach (var node in conflictNodes)
        {
            node.isLocked = true;
            node.LockChildNodes();
        }
    }

    private bool CanBeUnlocked()
    {
        if (isUnlocked || isLocked) return false;

        if (!skillTree.HasEnoughSkillPoints(skillData.cost)) return false;
        
        foreach(var node in needNodes)
        {
            if (!node.isUnlocked) return false;
        }

        foreach (var nodee in conflictNodes)
        {
            if (nodee.isUnlocked) return false;
        }

        return true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (CanBeUnlocked())
        {
            Unlock();
        }
        else if (isLocked)
        {
            ui.skillToolTip.LockedSkillEffect();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)//当鼠标进入区域时，调用该方法
    {
        ui.skillToolTip.ShowToolTip(true, rectTransform,this);
        if (isUnlocked || isLocked)
        {
            return;
        }
        highlightColor.a = 1;
        UpdateIconColor(highlightColor);
    }

    public void OnPointerExit(PointerEventData eventData)// 当鼠标离开该区域时，调用该方法
    {//当GameObject被禁用时，该方法便不会被调用
        ui.skillToolTip.ShowToolTip(false, rectTransform);
        ui.skillToolTip.StopLockedSkillEffect();

        if (isUnlocked || isLocked)
        {
            return;
        }      
        UpdateIconColor(GetColorByHex(lockedColorHex));             
    }

    //当GameObject被禁用时调用该方法
    private void OnDisable()
    {
        if  (isLocked || !isUnlocked)
        {
            UpdateIconColor(GetColorByHex(lockedColorHex));
        }    
    }

    private Color GetColorByHex(string hex)
    {
        Color color;
        ColorUtility.TryParseHtmlString(hex, out color);

        return color;
    }
}
