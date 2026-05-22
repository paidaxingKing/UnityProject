using System;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG SetUp/Skill Data", fileName = "Skill Data - ")]
public class Skill_DataSO : ScriptableObject
{
   
    [Header("Skill Description")]
    public string skillName;
    [TextArea]
    public string description;
    public Sprite icon;

    [Header("Unlock & Upgrade")]
    public int cost;
    public SkillType skillType;
    public UpgradeData upgradeData;


}

[Serializable]
public class UpgradeData
{
    public SkillUpgradeType upgradeType;
    public float cooldown;
    public DamageScaleData damageScale;
}
