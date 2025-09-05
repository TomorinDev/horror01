using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    public TextMeshProUGUI continueText;
    public GameObject gameTitle; // 添加游戏标题
    public GameObject menuGroup; // 菜单组对象

    private bool isInputReceived = false;

    void Start()
    {
        if (continueText != null)
        {
            continueText.text = "Press Any Button to Continue";
        }
        menuGroup.SetActive(false); // 初始化隐藏菜单
    }

    void Update()
    {
        if (!isInputReceived && Input.anyKeyDown)
        {
            isInputReceived = true;
            continueText.text = "Loading...";
            Invoke("ShowMenu", 0.5f); // 显示菜单
        }
    }

    void ShowMenu()
    {
        // 隐藏标题和文字
        gameTitle.SetActive(false);
        continueText.gameObject.SetActive(false);

        // 显示菜单
        menuGroup.SetActive(true);
    }

    public void StartGame()
    {
        Debug.Log("StartGame triggered");
        SceneManager.LoadScene("Level0"); // 加载 Level1
    }

    public void QuitGame()
    {
        Application.Quit(); // 退出游戏
    }
}
