
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    private Player player;

    [SerializeField] private RectTransform healthRect;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;

    private void Start()
    {
        player = FindFirstObjectByType<Player>();
        player.entity_Health.OnHealthUpdate += UpdateHealthBar;
        UpdateHealthBar();
    }


    private void UpdateHealthBar()
    {
        float curHp = Mathf.RoundToInt(player.entity_Health.GetCurHp());
        float maxHp = player.entity_Health.GetMaxHp();
        float sizeDifference = Mathf.Abs(maxHp - healthRect.sizeDelta.x);

        if (sizeDifference > 0.1f)
            healthRect.sizeDelta = new Vector2(maxHp, healthRect.sizeDelta.y);

        healthText.text = curHp + "/" + maxHp;
        healthSlider.value = player.entity_Health.GetHealthPercent();
    }

}
