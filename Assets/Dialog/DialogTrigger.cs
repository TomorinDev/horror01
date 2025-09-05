using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    public string[] customDialogs; 

    private bool isPlayerNearby = false;

    private void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            DialogManager dialogManager = FindObjectOfType<DialogManager>();
            if (!dialogManager.gameObject.activeSelf) return;

            dialogManager.StartDialog(customDialogs); 
        }
    }

    private void OnTriggerEnter(Collider other)
    
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }
}
