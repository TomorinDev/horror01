using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class selectionManager : MonoBehaviour
{
    // Public Fields
    public static selectionManager instance { get; set; }
    public bool onTarget;

    public GameObject interaction_info_UI;
    public GameObject interaction_pop_UI;

    // Maximum interaction distance
    public float maxInteractionDistance = 10.0f;

    private Text interaction_info_txt;
    private Text interaction_pop_txt;
    private Camera mainCamera;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        onTarget = false;

        // Get Component
        interaction_info_txt = interaction_info_UI.GetComponent<Text>();
        interaction_pop_txt = interaction_pop_UI.GetComponent<Text>();
        mainCamera = Camera.main;
    }

    // Method to show pickup message
    public void ShowPickUpMessage(string itemName)
    {
        Debug.Log("You picked up: " + itemName);
        interaction_pop_txt.text = "You picked up: " + itemName;
        ShowPopUp();
    }

    // Pop Up
    private void ShowPopUp()
    {
        interaction_pop_UI.SetActive(true); // Show the popup UI
        StartCoroutine(HidePickUpMessage()); // Hide After Time Pass
    }

    // Hide the pickup message after 2 seconds
    private IEnumerator HidePickUpMessage()
    {
        yield return new WaitForSeconds(2f);
        interaction_pop_UI.SetActive(false); // Hide the popup UI
    }

    // Update is called once per frame
    void Update()
    {
        CheckForInteraction();
    }

    // Check For Interaction
    private void CheckForInteraction()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Ray Hit Interactable Object
        if (Physics.Raycast(ray, out hit))
        {
            interactableObj interactableObject = hit.transform.GetComponent<interactableObj>();

            // Object Must Be Interactable and Player Is In Range
            if (interactableObject != null && interactableObject.playerInRange)
            {
                HandleInteraction(interactableObject, hit.transform.position);
            }
            else
            {
                ResetInteraction();
            }
        }
        else
        {
            ResetInteraction();
        }
    }

    // Handle Interaction
    private void HandleInteraction(interactableObj interactableObject, Vector3 hitPosition)
    {
        onTarget = true;
        float distance = Vector3.Distance(mainCamera.transform.position, hitPosition);

        // Check Camera and Object Range
        if (distance <= maxInteractionDistance)
        {
            UpdateInteractionUI(interactableObject);
        }
        else
        {
            interaction_info_UI.SetActive(false); // Hide if too far
        }
    }

    // Update UI
    private void UpdateInteractionUI(interactableObj interactableObject)
    {
        // Set Text
        interaction_info_txt.text = $"{interactableObject.getItemName()}\n{interactableObject.getItemDescription()}";
        interaction_info_UI.SetActive(true); // Show UI Text
    }

    // Reset interaction
    private void ResetInteraction()
    {
        onTarget = false;
        interaction_info_UI.SetActive(false); // Hide UI Text
    }
}
