using UnityEngine;
using TMPro;
using System.Collections;

public class DialogSystem : MonoBehaviour
{
    public TMP_Text dialogText; // 显示对话的文本组件
    public float typingSpeed = 0.05f; // 打字速度

    private string[] dialogs; // 保存对话内容的数组
    private int currentDialogIndex = 0; // 当前对话的索引
    private bool isTyping = false; // 是否正在打印文字

    public void StartDialog(string[] newDialogs)
    {
        dialogs = newDialogs; // 初始化对话内容
        currentDialogIndex = 0;

        if (dialogs.Length > 0)
        {
            StartTyping(dialogs[currentDialogIndex]); // 开始打印第一段内容
        }
        else
        {
            Debug.LogWarning("Dialogs array is empty! Cannot start dialog.");
        }
    }

    public void HandleMouseClick()
    {
        if (isTyping)
        {
            // 如果正在打印文字，立即显示完整内容
            StopAllCoroutines();
            dialogText.text = dialogs[currentDialogIndex];
            isTyping = false;
        }
        else
        {
            // 如果当前段已显示完毕，进入下一段或结束对话
            if (currentDialogIndex < dialogs.Length - 1)
            {
                currentDialogIndex++;
                StartTyping(dialogs[currentDialogIndex]);
            }
            else
            {
                // 当前是最后一段内容，结束对话
                EndDialog();
            }
        }
    }

    private void StartTyping(string dialog)
    {
        StartCoroutine(TypeDialog(dialog));
    }

    private IEnumerator TypeDialog(string dialog)
    {
        isTyping = true;
        dialogText.text = ""; // 清空当前显示内容
        foreach (char letter in dialog.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false; // 打印完成
    }

    private void EndDialog()
    {
        Debug.Log("Dialog ended.");
        FindObjectOfType<DialogManager>().EndDialog(); // 通知 DialogManager 对话结束
    }
}
