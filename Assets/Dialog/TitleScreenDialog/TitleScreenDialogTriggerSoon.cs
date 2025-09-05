using UnityEngine;
using System.Collections;

public class TitleScreenDialogTriggerSoon : MonoBehaviour
{
    public string[] dialogLines; // 对话内容
    public TitleScreenDialogManager dialogManager; // 对话管理器引用

    private bool isTriggered = false;

    private void Start()
    {
        StartCoroutine(TriggerAfterInitialization());
    }

    private IEnumerator TriggerAfterInitialization()
    {
        yield return null; // 等待一帧，确保初始化完成
        TriggerDialogSoon();
    }

    private void TriggerDialogSoon()
    {
        if (!isTriggered)
        {
            if (dialogManager != null && dialogLines.Length > 0)
            {
                dialogManager.StartDialog(dialogLines); // 触发对话
                isTriggered = true;
            }
            else
            {
                Debug.LogWarning("DialogManager is not assigned or dialogLines are empty.");
            }
        }
    }
}
