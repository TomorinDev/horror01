using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class interactionTrigger : MonoBehaviour
{
    // Public Fields
    public AudioSource audioSource; // Audio
    public AudioClip dialogueClip;

    public Image interactionBoard; // UI Board
    public Image interactionImage; // UI Image
    public Sprite imageSource; // Image

    public Text interactionNameText; // UI Text
    public Text interactionText; // UI Text
    public string interactionName; // Text
    public string message; // Text

    public float delayBeforeHide = 2f;  // Time in seconds before UI hides

    public bool playerIsInRange;

    // Private Fields
    private bool audioPlayed = false; // Check Audio is played

    private void Start()
    {
        // Disable the UI Image & Text
        interactionBoard.enabled = false;
        interactionImage.enabled = false;
        interactionNameText.enabled = false;
        interactionText.enabled = false;
    }

    // In Range
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Player In Range
            playerIsInRange = true;

            // Trigger the interaction if player enter
            TriggerInteraction();
        }
    }

    // Not In Range
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Player Not In Range
            playerIsInRange = false;

            // Hide the UI and stop the Audio
            StartCoroutine(EndInteraction());
        }
    }

    // Trigger Interaction
    void TriggerInteraction()
    {
        // Play Audio
        if (audioSource != null && dialogueClip != null && !audioPlayed)
        {
            audioSource.clip = dialogueClip;
            audioSource.Play();
            audioPlayed = true;  // Mark Audio to Played
        }

        // Set the Image Source
        if (interactionImage != null && imageSource != null)
        {
            interactionImage.sprite = imageSource;
        }

        // Show UI Image & Text
        interactionBoard.enabled = true;
        interactionImage.enabled = true;
        interactionNameText.enabled = true;
        interactionText.enabled = true;

        // Set Text Message
        interactionNameText.text = interactionName;
        interactionText.text = message; 
    }

    // End Interaction
    IEnumerator EndInteraction()
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delayBeforeHide);

        // Player Not In Range
        if (!playerIsInRange)
        {
            // Stop Audio & Hide UI
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            // Hide UI Image & Text
            interactionBoard.enabled = false;
            interactionImage.enabled = false;
            interactionNameText.enabled = false;
            interactionText.enabled = false;
        }
    }
}
