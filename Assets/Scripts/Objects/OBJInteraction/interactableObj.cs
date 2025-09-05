using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class interactableObj : MonoBehaviour, IPlayerIsInRangeTrigger
{
    [Header("Item Info Settings")]
    public string itemName; // Setting Up In Component Settings
    public string itemDescription;
    public string helpText;
    public Sprite itemImage; // 2D image


    [Header("Interaction Settings")]
    public bool playerInRange; // Just Can See That, haha, keep it public XD
    public bool isPickable;
    public IQuestProvider questProvider;
    public bool IsPlayerInRange => playerInRange;
    public bool canCheck = false;


    [Header("Audio Settings")]
    private AudioSource audioSource; // Existing AudioSource component
    public bool canPlaySound = false;
    private bool isSoundPlaying = false; // To track sound playback


    [Header("UI Settings")]
    private Image uiImage; // UI Image component
    private bool infoIsActive = false;



    [Header("Helper Settings")]
    public Text hintText;
    private bool isHintActive = false;






    public string getItemName()
    {
        return itemName;
    }

    public string getItemDescription()
    {
        return itemDescription;
    }

    public string getHelpText()
    {
        return helpText;
    }




    void Start()
    {
        // Get Components

        questProvider = GetComponent<IQuestProvider>();

        // Hint Text
        if(!string.IsNullOrEmpty(helpText))
        {
            // Check if set up hintText
            if (hintText != null)
            {
                hintText.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogError("Hint Text UI is not assigned");
            }
        }

        // Get existing AudioSource component
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (audioSource == null)
        {
            Debug.LogWarning("No AudioSource found on the GameObject");
        }

        // Find UI elements by name
        if(canCheck == true)
        {
            uiImage = GameObject.Find("DocumentImage").GetComponent<Image>();

            if (uiImage == null)
            {
                Debug.LogWarning("UI elements for item info are missing");
            }

            // Hide At First
            uiImage.enabled = false;
        }
    }




    void Update()
    {
        HandleInteraction();
 
    }




    // Handle Interaction
    private void HandleInteraction()
    {
        // Toggle Show Help Text
        ToggleShowHelpText();

        // Pressing E
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Play Interaction Sound
            PlayInteractionSound();

            // Is Pickable, Player In Range, Cursor Target, Object Pickable
            if (playerInRange && selectionManager.instance.onTarget && isPickable)
            {
                HandlePickableOBJ();
            }
        }

        // Active and Deactive info UI
        if (Input.GetKeyDown(KeyCode.E) && playerInRange && selectionManager.instance.onTarget && canCheck)
        {
            // Set Image, ItemName, Description
            if (uiImage != null)
            {
                setInfoUIElements();
            }

            // Toggle the info UI
            if (infoIsActive)
            {
                HideItemInfo();
            }
            else
            {
                ShowItemInfo();
            }
        }

        // Escape to Hide Directly
        if (Input.GetKeyDown(KeyCode.Escape) && infoIsActive)
        {
            HideItemInfo();
        }
    }




    // Toggle Show Help Text
    private void ToggleShowHelpText()
    {
        if (playerInRange && selectionManager.instance.onTarget)
        {
            showHint();
        }
        else
        {
            hideHint();
        }
    }

    // Show Hint
    private void showHint()
    {
        if (hintText != null && !string.IsNullOrEmpty(helpText))
        {
            hintText.text = helpText;
            hintText.gameObject.SetActive(true);
            isHintActive = true;
        }
    }

    // Hide Hint
    private void hideHint()
    {
        if (hintText != null)
        {
            hintText.gameObject.SetActive(false);
            isHintActive = false;
        }
    }

    // Set Info UI Elements
    private void setInfoUIElements()
    {
        uiImage.enabled = true;
        uiImage.sprite = itemImage;
    }

    // Show Item Info
    private void ShowItemInfo()
    {
        if (uiImage != null)
        {
            HandleToggleInfoUI(true);
            infoIsActive = true; // Mark UI as active
        }
        else
        {
            Debug.LogWarning("UI components for item info are missing");
        }
    }

    // Hide Item Info
    private void HideItemInfo()
    {
        if (uiImage != null)
        {
            HandleToggleInfoUI(false);
            infoIsActive = false; // Mark UI as inactive
        }
        else
        {
            Debug.LogWarning("UI components for item info are missing!");
        }
    }

    // Handle Toggle info UI
    private void HandleToggleInfoUI(bool status)
    {
        uiImage.gameObject.SetActive(status);
    }




    // Play Interaction Sound
    private void PlayInteractionSound()
    {
        if (canPlaySound)
        {
            if (!isSoundPlaying) // Check if the sound is currently playing
            {
                HandlePlayInteractionSound(); // Play the sound
            }
        }
    }

    // Handle Play Interaction Sound
    private void HandlePlayInteractionSound()
    {
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
            StartCoroutine(SoundPlayCoroutine());
        }
        else
        {
            Debug.LogWarning("AudioSource or AudioClip is missing!");
        }
    }

    // Play Sound
    private IEnumerator SoundPlayCoroutine()
    {
        isSoundPlaying = true;
        yield return new WaitForSeconds(audioSource.clip.length); // Wait for the sound to finish
        isSoundPlaying = false;
    }




    // Check if is First Aid Kit
    private void checkFirstAidKit()
    {
        // If OBJ is a FirstAidKit, heal the player
        FirstAidKit firstAidKit = GetComponent<FirstAidKit>();
        if (firstAidKit != null)
        {
            firstAidKit.HealPlayer(); // Heal the player
        }
    }




    // Handle Pickable OBJ
    private void HandlePickableOBJ()
    {
        // If OBJ is a FirstAidKit, heal the player
        checkFirstAidKit();

        // Show PickUpMessage
        selectionManager.instance.ShowPickUpMessage(itemName);

        // Object Create Quest
        if (questProvider != null && questProvider.CanCreateQuest())
        {
            questProvider.CreateQuest();
        }

        // Destroy Object
        Destroy(gameObject);
    }




    // IPlayerIsInRangeTrigger
    public void OnPlayerEnter()
    {
        playerInRange = true;
    }

    public void OnPlayerExit()
    {
        playerInRange = false;
    }
}
