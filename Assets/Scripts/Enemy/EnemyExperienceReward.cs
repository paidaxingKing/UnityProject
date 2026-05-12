using UnityEngine;

public class EnemyExperienceReward : MonoBehaviour
{
    [SerializeField] private int experience = 25;
    private bool rewarded;

    public void GiveReward()
    {
        if (rewarded) return;

        PlayerLevel playerLevel = FindFirstObjectByType<PlayerLevel>();
        if (playerLevel == null) return;

        rewarded = true;
        playerLevel.GainExperience(experience);
    }
}
