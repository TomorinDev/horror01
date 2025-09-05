using UnityEngine;

public class GenericTrigger: MonoBehaviour
{
    [Header("Interaction Settings")]
    public MonoBehaviour scriptToActivate; // The script to activate on another GameObject



    private interactableObj myInteractableObj;
    void Start()
    {
        myInteractableObj = GetComponent<interactableObj>();
    }

    void Update()
    {
        // If the player presses 'E'
        if (myInteractableObj.IsPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ActivateObjectAndScript();
        }
        
    }

    void ActivateObjectAndScript()
    {
        // Enable the script if it is not already enabled
        if (scriptToActivate != null && !scriptToActivate.enabled)
        {
            scriptToActivate.enabled = true;
        }
    }
}
