using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ConfirmationDialog : MonoBehaviour
{
    [SerializeField] private GameObject dialogPanel;
    [SerializeField] private Button confirmButton;  // 确认按钮
    [SerializeField] private Button cancelButton;   // 取消按钮
    [SerializeField] private TextMeshProUGUI confirmText;

    private event Action onConfirmAction;

    private void Awake()
    {
        // 绑定按钮事件
        confirmButton.onClick.AddListener(ClickConfirm);
        cancelButton.onClick.AddListener(CloseDialog);
        dialogPanel.SetActive(false); // 默认隐藏
    }

    // 核心：打开弹窗，并传入“确认时要做什么”
    public void Show(string message, Action onConfirm)
    {
        confirmText.text = message;
        onConfirmAction = onConfirm;
        dialogPanel.SetActive(true);
        dialogPanel.transform.SetAsLastSibling();
    }

    private void ClickConfirm()
    {
        onConfirmAction?.Invoke(); // 执行丢弃逻辑
        CloseDialog();
    }

    private void CloseDialog()
    {
        onConfirmAction = null;
        dialogPanel.SetActive(false);
    }
}
