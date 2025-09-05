using UnityEngine;

public class DialogManager : MonoBehaviour
{
    public DialogSystem dialogSystem;
    public GameObject dialogRoot;

    private bool isDialogActive = false;

    private void Start()
    {
        dialogRoot.SetActive(false);
    }

    private void Update()
    {
        if (isDialogActive && Input.GetMouseButtonDown(0))
        {
            dialogSystem.HandleMouseClick();
        }
    }

    public void StartDialog(string[] dialogs)
    {
        dialogRoot.SetActive(true); 
        isDialogActive = true;
        dialogSystem.StartDialog(dialogs); 
    }

    public void EndDialog()
    {
        dialogRoot.SetActive(false); 
        isDialogActive = false;
    }
}
