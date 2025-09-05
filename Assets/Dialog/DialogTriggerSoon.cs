using UnityEngine;

public class DialogTriggerSoon : MonoBehaviour
{
    [Header("Dialog Settings")]
    public string[] dialogLines; // 对话内容
    public DialogManager dialogManager; // 引用DialogManager

    private bool isTriggered = false; // 是否已经触发

    void Start()
    {
        if (!isTriggered)
        {
            TriggerDialog();
            isTriggered = true; // 确保只触发一次
        }
    }

    private void TriggerDialog()
    {
        if (dialogManager != null && dialogLines.Length > 0)
        {
            dialogManager.StartDialog(dialogLines);
        }
        else
        {
            Debug.LogWarning("DialogManager is not assigned or dialogLines are empty.");
        }
    }
}
