using System;
using UnityEngine;

public enum PlayerStatType
{
    Strength,
    Agility,
    Vitality,
    Intelligence
}

public class PlayerLevel : MonoBehaviour
{
    public event Action OnLevelDataChanged;

    [SerializeField] private int level = 1;
    [SerializeField] private int currentExp;
    [SerializeField] private int expToNextLevel = 100;
    [SerializeField] private int statPoints;

    private Entity_Stats stats;

    public int Level => level;
    public int CurrentExp => currentExp;
    public int ExpToNextLevel => expToNextLevel;
    public int StatPoints => statPoints;

    private void Awake()
    {
        stats = GetComponent<Entity_Stats>();
    }

    public void GainExperience(int amount)
    {
        currentExp += amount;

        while (currentExp >= expToNextLevel)
        {
            currentExp -= expToNextLevel;
            LevelUp();
        }

        OnLevelDataChanged?.Invoke();
    }

    private void LevelUp()
    {
        level++;
        statPoints += 3;
        expToNextLevel = Mathf.RoundToInt(expToNextLevel * 1.25f);
    }

    public void AddStatPoint(PlayerStatType type)
    {
        if (statPoints <= 0) return;

        switch (type)
        {
            case PlayerStatType.Strength:
                stats.majorStats.strength.AddBonus(1);
                break;
            case PlayerStatType.Agility:
                stats.majorStats.agility.AddBonus(1);
                break;
            case PlayerStatType.Vitality:
                stats.majorStats.vitality.AddBonus(1);
                break;
            case PlayerStatType.Intelligence:
                stats.majorStats.intelligence.AddBonus(1);
                break;
        }

        statPoints--;
        OnLevelDataChanged?.Invoke();
    }
}
