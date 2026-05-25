using UnityEngine;

public class UI : MonoBehaviour
{
    public static UI instance;

    [SerializeField] private GameObject[] uiElements;
    public bool alternativeInput { get; private set; }

    #region UI_Elements
    private PlayerInputSet input;

    public UI_SkillToolTip skillToolTip { get; private set; }
    public UI_ItemToolTip itemToolTip { get; private set; }
    public UI_StatToolTip statToolTip { get; private set; }

    public UI_SkillTree skillTreeUI { get; private set; }
    public UI_Inventory inventoryUI { get; private set; }

    public UI_ConfirmationDialog confirmationDialogUI { get; private set; }

    public UI_Options optionsUI { get; private set; }

    public UI_DeathScreen deathScreenUI { get; private set; }

    public UI_FadeScreen fadeScreenUI { get; private set; }
    #endregion

    private bool skillTreeEnabled;
    private bool inventoryEnabled;

    private void Awake()//组件被禁用Awake仍会执行，但是如果是整个 GameObject被禁用，则不会执行Awake
    {
        instance = this;

        skillToolTip = GetComponentInChildren<UI_SkillToolTip>();
        itemToolTip = GetComponentInChildren<UI_ItemToolTip>();
        statToolTip = GetComponentInChildren<UI_StatToolTip>();

        skillTreeUI = GetComponentInChildren<UI_SkillTree>(true);//设置为true，则即使组件被关闭了，也能找到该组件
        inventoryUI = GetComponentInChildren<UI_Inventory>(true);
        optionsUI = GetComponentInChildren<UI_Options>(true);
        deathScreenUI = GetComponentInChildren<UI_DeathScreen>(true);
        confirmationDialogUI = GetComponentInChildren<UI_ConfirmationDialog>(true);
        fadeScreenUI = GetComponentInChildren<UI_FadeScreen>(true);
        skillTreeUI.InitializeUI_Tree();
    }

    public void OpenDeathScreenUI()
    {
        SwitchTo(deathScreenUI.gameObject);
        input.Disable();
    }

    private void SwitchTo(GameObject objectToSwicth)
    {
        foreach (var element in uiElements)
        {
            element.gameObject.SetActive(false);
        }

        objectToSwicth.SetActive(true);
    }


    public void OpenOptionsUI()
    {
        HideAllTips();
        StopPlayerControls(true);

        SwitchTo(optionsUI.gameObject);

        skillTreeEnabled = false;
        inventoryEnabled = false;
    }

    public void SwitchToInGameUI()
    {
        foreach (var element in uiElements)
        {
            element.gameObject.SetActive(false);
        }

        HideAllTips();
        skillTreeEnabled = false;
        inventoryEnabled = false;
        StopPlayerControls(false);
    }

    public void SetupControlsUI(PlayerInputSet inputSet)
    {
        input = inputSet;

        input.UI.ToggleSkillTreeUI.performed += ctx => ToggleSkillTreeUI();
        input.UI.ToggleCharacterUI.performed += ctx => ToggleInventoryUI();

        input.UI.AlternativeInput.performed += ctx => alternativeInput = true;
        input.UI.AlternativeInput.canceled += ctx => alternativeInput = false;

        input.UI.ToggleOptionsUI.performed += ctx =>
        {
            foreach (var element in uiElements)
            {
                if (element.activeSelf)
                {
                    Time.timeScale = 1;
                    SwitchToInGameUI();
                    return;
                }
            }

            Time.timeScale = 0;
            OpenOptionsUI();
        };
    }

    private void StopPlayerControlsIfNeeded()
    {
        foreach (var element in uiElements)
        {
            if (element.activeSelf)
            {
                StopPlayerControls(true);
                return;
            }
        }
        StopPlayerControls(false);
    }

    private void StopPlayerControls(bool stopControls)
    {
        if (stopControls)
        {
            input.Player.Disable();
        }
        else
        {
            input.Player.Enable();
        }
    }

    private void HideAllTips()
    {
        skillToolTip.ShowToolTip(false, null);
        statToolTip.ShowToolTip(false, null);
        itemToolTip.ShowToolTip(false, null);
    }

    private void SetAllTips()
    {
        skillToolTip.transform.SetAsLastSibling();
        statToolTip.transform.SetAsLastSibling();
        itemToolTip.transform.SetAsLastSibling();
    }

    public void ToggleSkillTreeUI()
    {
        skillTreeUI.transform.SetAsLastSibling();
        SetAllTips();
        fadeScreenUI.transform.SetAsLastSibling();

        skillTreeEnabled = !skillTreeEnabled;
        skillTreeUI.gameObject.SetActive(skillTreeEnabled);

        HideAllTips();

        StopPlayerControlsIfNeeded();
    }

    public void ToggleInventoryUI()
    {
        if (confirmationDialogUI.gameObject.activeSelf)
        {
            return;
        }
        inventoryUI.transform.SetAsLastSibling();
        SetAllTips();
        fadeScreenUI.transform.SetAsLastSibling();

        inventoryEnabled = !inventoryEnabled;
        inventoryUI.gameObject.SetActive(inventoryEnabled);
        HideAllTips();

        StopPlayerControlsIfNeeded();
    }

    
}
