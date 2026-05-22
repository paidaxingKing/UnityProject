using UnityEngine;

public class Skill_Dash : Skill_Base
{

    public void OnStartEffect()
    {
        if (IsUnlocked(SkillUpgradeType.Dash_CloneOnStart) || IsUnlocked(SkillUpgradeType.Dash_CloneOnStartAndArrival))
        {
            CreateClone();
        }

        if (IsUnlocked(SkillUpgradeType.Dash_ShardOnStart) || IsUnlocked(SkillUpgradeType.Dash_ShardOnStartAndArrival))
        {
            CreateShard();
        }
    }

    public void OnEndEffect()
    {
        if (IsUnlocked(SkillUpgradeType.Dash_CloneOnStartAndArrival))
        {
            CreateClone();
        }

        if (IsUnlocked(SkillUpgradeType.Dash_ShardOnStartAndArrival))
        {
            CreateShard();
        }
    }

    private void CreateShard()
    {
        skillManager.shard.CreatRawShard();
    }

    private void CreateClone()
    {
        Debug.Log("Create clone");
    }
}
