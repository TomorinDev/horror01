using UnityEngine;
using UnityEngine.UI; // 注意需要导入UI命名空间
using UnityEngine.SceneManagement;


public class TitleScreenDialogManager : MonoBehaviour
{
    public TitleScreenDialogSystem dialogSystem; // 对话显示器
    public GameObject dialogRoot; // 对话 UI 的根节点
    public Image backgroundImage; // 背景图片的 Image 组件
    public Sprite[] backgroundSprites; // 三张背景图片

    private string[] dialogs; // 当前对话内容数组
    private int currentDialogIndex = 0; // 当前对话索引
    private bool isDialogActive = false;

    // 对应背景切换的对话索引
    public int[] backgroundSwitchIndices = new int[2] { 2, 5 }; // 在第 2 段和第 5 段切换背景

    private void Start()
    {
        dialogRoot.SetActive(false); // 初始隐藏对话UI
    }

    private void Update()
    {
        if (isDialogActive && Input.GetMouseButtonDown(0))
        {
            HandleMouseClick();
        }
    }

    public void StartDialog(string[] newDialogs)
    {
        // 激活 dialogRoot
        if (!dialogRoot.activeSelf)
        {
            dialogRoot.SetActive(true);
        }

        dialogs = newDialogs;
        currentDialogIndex = 0;
        isDialogActive = true;

        UpdateBackground(); // 初始背景
        dialogSystem.StartTyping(dialogs[currentDialogIndex]); // 开始打印第一段对话
    }

    private void HandleMouseClick()
    {
        if (dialogSystem.IsTyping())
        {
            // 如果正在打印，立即显示完整内容
            dialogSystem.DisplayFullText(dialogs[currentDialogIndex]);
        }
        else
        {
            // 显示下一段或结束对话
            if (currentDialogIndex < dialogs.Length - 1)
            {
                currentDialogIndex++;
                UpdateBackground(); // 根据索引更新背景
                dialogSystem.StartTyping(dialogs[currentDialogIndex]);
            }
            else
            {
                EndDialog(); // 当前对话结束
            }
        }
    }

    private void UpdateBackground()
    {
        // 检查是否需要切换背景
        for (int i = 0; i < backgroundSwitchIndices.Length; i++)
        {
            if (currentDialogIndex == backgroundSwitchIndices[i])
            {
                backgroundImage.sprite = backgroundSprites[i + 1]; // 切换到对应的背景图片
                return;
            }
        }

        // 默认使用第一张背景
        if (currentDialogIndex == 0)
        {
            backgroundImage.sprite = backgroundSprites[0];
        }
    }

    public void EndDialog()
    {
        isDialogActive = false;
        dialogRoot.SetActive(false); // 隐藏对话UI
        Debug.Log("Dialog finished.");
        SceneManager.LoadScene("Level1");
    }
}
