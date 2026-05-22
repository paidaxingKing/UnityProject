using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;

public class UI_SkillToolTip :  UI_ToolTip
{
    private UI_SkillTree skillTree;
    private UI ui;

    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillDescription;
    [SerializeField] private TextMeshProUGUI skillCoolDown;
    [SerializeField] private TextMeshProUGUI skillRequirements;

    [Space]
    [SerializeField] private string meetConditionColorHex;
    [SerializeField] private string notMeetConditionColorHex;
    [SerializeField] private string importantInfoColorHex;
    [SerializeField] private Color exampleColor;
    [SerializeField] private string lockedSkillText = "你已经选了另一条路径，这个技能现已锁定";

    private Coroutine textEffectCo;

    protected override void Awake()
    {
        base.Awake();
        ui = GetComponentInParent<UI>();
        skillTree = ui.GetComponentInChildren<UI_SkillTree>(true);
    }

    public override void ShowToolTip(bool show, RectTransform targetTransform)
    {
        base.ShowToolTip(show, targetTransform);


    }

    public void ShowToolTip(bool show ,RectTransform targetTransform,UI_TreeNode node)
    {
        base.ShowToolTip(show, targetTransform);

        if (!show) return;

        skillName.text = node.skillData.skillName;
        skillDescription.text = node.skillData.description;
        skillCoolDown.text = "Cooldown: " + node.skillData.upgradeData.cooldown + " s.";

        string requirementText = GetRequirements(node.skillData.cost, node.needNodes, node.conflictNodes);
        string conflictText = $"<color={importantInfoColorHex}> {lockedSkillText} </color>";

        skillRequirements.text = node.isLocked ? conflictText : requirementText;
    }

    private string GetRequirements(int skillCost, UI_TreeNode[] needNodes, UI_TreeNode[] conflictNodes)
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("Requirements:");

        string costColor = skillTree.HasEnoughSkillPoints(skillCost) ? meetConditionColorHex : notMeetConditionColorHex;

        sb.AppendLine($"<color={costColor}>- {skillCost} skill point(s) </color>");
        //<color=>{costColor}>xxxxxx</color>为字符串设置颜色,costColor是十六进制设置

        foreach (var node in needNodes)
        {
            string nodeColor = node.isUnlocked ? meetConditionColorHex : notMeetConditionColorHex;
            sb.AppendLine($"<color={nodeColor}>- {node.skillData.skillName} </color>");
        }

        if (conflictNodes.Length > 0)
        {
            sb.AppendLine();//空行
            sb.AppendLine($"<color={importantInfoColorHex}>Conflict with: </color>");

            foreach (var node in conflictNodes)
            {
                sb.AppendLine($"<color={importantInfoColorHex}>- {node.skillData.skillName} </color>");
            }
            
        }
        return sb.ToString();
    }

    public void StopLockedSkillEffect()
    {
        if (textEffectCo != null)
        {
            StopCoroutine(textEffectCo);
        }
    }

    public void LockedSkillEffect()
    {
        StopLockedSkillEffect();
        textEffectCo = StartCoroutine(TextBlinkEffectCo(skillRequirements,0.15f,3));
    }

    private IEnumerator TextBlinkEffectCo(TextMeshProUGUI text, float interval,int blinkCount)
    {
        for (int i = 0; i < blinkCount; i++)
        {
            text.text = GetColoredText(notMeetConditionColorHex, lockedSkillText);
            yield return new WaitForSeconds(interval);

            text.text = GetColoredText(importantInfoColorHex, lockedSkillText);
            yield return new WaitForSeconds(interval);
        }
    }
}
