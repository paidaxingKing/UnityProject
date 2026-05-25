using UnityEngine;

public class Player_SkillManager : MonoBehaviour
{
    public Skill_Dash dash { get; private set; }
    public Skill_Heal heal { get; private set; }
    public Skill_Shard shard { get; private set; }
    public Skill_Sword sword { get; private set; }

    public Skill_Base[] allSkills { get; private set; }

    private void Awake()
    {
        dash = GetComponentInChildren<Skill_Dash>();
        heal = GetComponentInChildren<Skill_Heal>();
        shard = GetComponentInChildren<Skill_Shard>();
        sword = GetComponentInChildren<Skill_Sword>();

        allSkills = GetComponentsInChildren<Skill_Base>();
    }

    public Skill_Base GetSkillByType(SkillType skillType)
    {
        switch (skillType)
        {
            case SkillType.Dash: return dash;
            case SkillType.TimeShard: return shard;
            case SkillType.Sword: return sword;

            default: return null;
        }
    }
}
