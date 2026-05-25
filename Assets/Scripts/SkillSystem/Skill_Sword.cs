
using System.Collections.Generic;
using UnityEngine;

public class Skill_Sword : Skill_Base
{
    [Header("Normal Sword")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration;

    // 核心新增：标记场上是否已经有活动的飞剑
    private bool hasStuckSword;

    [Header("MultiCast Sword Upgrade")]
    [SerializeField] private List<SkillObjective_Sword> stuckSwords;
    [SerializeField] private int maxSword;
    [SerializeField] private int currentSword;

    protected override void Awake()
    {
        base.Awake();
        stuckSwords = new List<SkillObjective_Sword>();
    }

    public override void TryUseSkill()
    {
        // 如果场上已经有一把剑了，直接返回，不允许发射

        if (!CanUseSkill()) return;

        if (IsUnlocked(SkillUpgradeType.Sword) && !hasStuckSword)
        {
            HandleSwordRegular();
        }

        if (IsUnlocked(SkillUpgradeType.Sword_MultiCast) || IsUnlocked(SkillUpgradeType.Sword_Retrieve))
        {
            HandleSwordMultiCast();
        }
    }

    public bool ShouldUpdateStuckList()
    {
        return IsUnlocked(SkillUpgradeType.Sword_MultiCast) || IsUnlocked(SkillUpgradeType.Sword_Retrieve);
    }

    public void DeleteStuckSword(SkillObjective_Sword sword)
    {
        stuckSwords.Remove(sword);
        currentSword--;
    }

    public void AddStuckSword(SkillObjective_Sword sword)
    {
        stuckSwords.Add(sword);
    }

    private void HandleReturnSword()
    {
        foreach (var sword in stuckSwords)
        {
            sword.StartReturn();
        }
    }

    private void HandleSwordMultiCast()
    {
        if (currentSword < maxSword)
        {
            CreateSword();
            currentSword++;
        }
        else if (IsUnlocked(SkillUpgradeType.Sword_Retrieve))
        {
            HandleReturnSword();
        }
    }

    private void HandleSwordRegular()
    {
        CreateSword();
        // 标记为已发射状态
        hasStuckSword = true;
        SetSkillOnCooldown();
    }

    public void CreateSword()
    {
        GameObject sword = Instantiate(swordPrefab, transform.position, Quaternion.identity);
        SkillObjective_Sword swordController = sword.GetComponent<SkillObjective_Sword>();
        swordController.Setup(this, maxSpeed, acceleration);
    }

    // 核心新增：飞剑销毁或被捡起时，调用此方法解锁
    public void RetrieveSword()
    {
        hasStuckSword = false;
        ResetCooldown();
    }

    public void FlySwordWithAttack()
    {
        if (IsUnlocked(SkillUpgradeType.Sword_WithAttack))
        {
            CreateSword();
        }
    }
}