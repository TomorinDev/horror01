using UnityEngine;
using TMPro;
using System.Collections;

public class TitleScreenDialogSystem : MonoBehaviour
{
    public TMP_Text dialogText; // 显示对话的文本
    public float typingSpeed = 0.05f; // 打字速度

    private bool isTyping = false; // 当前是否正在打印文字
    private Coroutine typingCoroutine; // 打字协程的引用

    public void StartTyping(string dialog)
    {
        if (!gameObject.activeInHierarchy)
        {
            Debug.LogError("Dialog game object is inactive. Cannot start typing.");
            return;
        }

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine); // 停止当前协程（如果存在）
        }
        typingCoroutine = StartCoroutine(TypeDialog(dialog)); // 启动新的打字协程
    }

    public void DisplayFullText(string dialog)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine); // 停止当前协程
            typingCoroutine = null;
        }
        dialogText.text = dialog; // 直接设置为完整内容
        isTyping = false; // 标记打字完成
    }

    public bool IsTyping()
    {
        return isTyping;
    }

    private IEnumerator TypeDialog(string dialog)
    {
        isTyping = true; // 开始打字
        dialogText.text = ""; // 清空内容

        foreach (char letter in dialog.ToCharArray())
        {
            dialogText.text += letter; // 每次添加一个字符
            yield return new WaitForSeconds(typingSpeed); // 等待指定时间
        }

        isTyping = false; // 打字完成
    }
}
