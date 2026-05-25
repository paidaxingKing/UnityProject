using UnityEngine;
using UnityEngine.Audio;

public class Player_SFX : Entity_SFX
{
    [Header("Player SFX Names")]
    [SerializeField] private string dash;
    [SerializeField] private string jump;
    [SerializeField] private string land;
    [SerializeField] private string flyingSword;

    public void PlayDash()
    {
        AudioManager.instance.PlaySFX(dash, audioSource, soundDistance);
    }

    public void PlayJump()
    {
        AudioManager.instance.PlaySFX(jump, audioSource, soundDistance);
    }

    public void PlayLand()
    {
        AudioManager.instance.PlaySFX(land, audioSource, soundDistance);
    }

    public void PlayFlyingSword()
    {
        AudioManager.instance.PlaySFX(flyingSword, audioSource, soundDistance);
    }
}
