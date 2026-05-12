using UnityEngine;

[CreateAssetMenu(menuName = "RPG SetUp/Skill Data", fileName = "Skill Data - ")]
public class Skill_DataSO : ScriptableObject
{
    public int cost;

    [Header("Skill Description")]
    public string skillName;
    [TextArea]
    public string description;

    public Sprite icon;
}
