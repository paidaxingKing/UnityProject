using UnityEngine;

public class Skill_Base : MonoBehaviour
{
    public Player_SkillManager skillManager {  get; private set; }
    public Player player {  get; private set; }

    public DamageScaleData damageScaleData;

    [Header("General details")]
    [SerializeField] protected SkillType skillType;//基础技能类型
    [SerializeField] protected SkillUpgradeType upgradeType;//技能升级之后的衍生类型
    [SerializeField] protected float cooldown;

    

    private float lastTimeUsed;

    protected virtual void Awake()
    {
        lastTimeUsed = lastTimeUsed - cooldown;
        player = GetComponentInParent<Player>();
        skillManager = GetComponentInParent<Player_SkillManager>();
    }

    public virtual void TryUseSkill()
    {

    }

    public void SetSkillUpgrade(Skill_DataSO skillData)
    {
        UpgradeData upgradeData = skillData.upgradeData;
        upgradeType = upgradeData.upgradeType;
        cooldown = upgradeData.cooldown;
        damageScaleData = upgradeData.damageScale;
    }

    protected bool IsUnlocked(SkillUpgradeType type)
    {
        return upgradeType == type;
    }

    public bool CanUseSkill()
    {
        if (upgradeType == SkillUpgradeType.None) return false;

        if (OnCooldown()) return false;

        //mana check
        //unlock check

        return true;
    }

    public SkillUpgradeType GetUpgradeType()
    {
        return upgradeType;
    }

    public SkillType GetBaseType()
    {
        return skillType;
    }

    protected bool OnCooldown() => Time.time < lastTimeUsed + cooldown;
    public void SetSkillOnCooldown() => lastTimeUsed = Time.time;

    public void ResetCooldownBy(float cooldownReduction) => lastTimeUsed -= cooldownReduction;
    public void ResetCooldown() => lastTimeUsed = Time.time - cooldown;
}
