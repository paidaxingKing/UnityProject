using System.Linq;
using TMPro;
using UnityEngine;

public class UI_SkillTree : MonoBehaviour,ISaveable
{
    [SerializeField] private int skillPoints;
    [SerializeField] private TextMeshProUGUI skillPointsText;
    [SerializeField] private UI_TreeConnectionHandler[] parentNodes;
    public Player_SkillManager skillManager {  get; private set; }

    [SerializeField] private UI_TreeNode[] allTreeNodes;
    private void UpdateSkillPointsUI()
    {
        skillPointsText.text = skillPoints.ToString();
    }

    public void InitializeUI_Tree()
    {
        skillManager = FindAnyObjectByType<Player_SkillManager>();
        allTreeNodes = GetComponentsInChildren<UI_TreeNode>(true);
        foreach (var treeNode in allTreeNodes)
        {
            treeNode.InitializeUI_TreeNode();
        }
    }

    private void Start()
    {
        UpdateAllConnections();
        UpdateSkillPointsUI();
    }

    [ContextMenu("Reset Skill Tree")]
    public void RefundAllSkills()
    {
        UI_TreeNode[] skillNodes = GetComponentsInChildren<UI_TreeNode>();

        foreach (var node in skillNodes)
        {
            if (node.isUnlocked)
            {
                node.Refund();
            }           
        }
    }

    public void ReduceSkillPoint(int cost)
    {
        skillPoints -= cost;
        UpdateSkillPointsUI();
    }

    public void AddSkillPoint(int points)
    {
        skillPoints += points;
        UpdateSkillPointsUI();
    }

    public bool HasEnoughSkillPoints(int cost)
    {
        return skillPoints >= cost;
    }
    

    [ContextMenu("Update All Connections")]
    public void UpdateAllConnections()
    {
        foreach (var node in parentNodes)
        {
            node.UpdateAllConnections();
        }
    }

    public void LoadData(GameData data)
    {
        Debug.Log(data.skillPoints);
        skillPoints = data.skillPoints;

        foreach (var node in allTreeNodes)
        {
            string skillName = node.skillData.skillName;
            if (data.skillTreeUI.TryGetValue(skillName,out bool isUnlocked) && isUnlocked)
            {
                node.UnlockWithSaveData();
            }
        }

        foreach (var skill in skillManager.allSkills)
        {
            if (data.skillUpgrades.TryGetValue(skill.GetBaseType(),out SkillUpgradeType upgradeType))
            {           
                var upgradeNode = allTreeNodes.FirstOrDefault(node => node.skillData.upgradeData.upgradeType == upgradeType);
                if (upgradeNode != null)
                {
                    skill.SetSkillUpgrade(upgradeNode.skillData);
                }
               
            }
        }

    }

    public void SaveData(ref GameData data)
    {
        data.skillTreeUI.Clear();
        data.skillUpgrades.Clear();

        data.skillPoints = skillPoints;

        foreach (var node in allTreeNodes)
        {
            string skillName = node.skillData.skillName;
            data.skillTreeUI[skillName] = node.isUnlocked;
        }

        foreach (var skill in skillManager.allSkills)
        {
            data.skillUpgrades[skill.GetBaseType()] = skill.GetUpgradeType();
        }
    }
}
