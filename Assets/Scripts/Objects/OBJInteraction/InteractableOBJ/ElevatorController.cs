using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;
using Unity.VisualScripting;

public class ElevatorController : Unlockable
{
    [Header("Elevator Settings")]
    public string targetSceneName; // Name of the scene to load
    public float coolDownDuration = 300f;
    public AudioClip elevatorActivateSound;


    [Header("Shadowman Behaviour Triggers Settings")]
    [Tooltip("This Trigger will be fixed, when elevator start, it will used for survival related")]
    public List<GameObject> FixedShadowmanBehaviourTriggers;


    [Header("UI Settings")]
    public TextMeshProUGUI countdownText;


    private AudioSource audioSource;
    private interactableObj myInteractableObj;

    private PowerLevelManager powerManager;
    private bool isCountdownActive = false;




    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        myInteractableObj = GetComponent<interactableObj>();
        powerManager = FindObjectOfType<PowerLevelManager>();

        if (audioSource == null)
        {
            Debug.LogError("Missing AudioSource component on the elevator.");
        }

        // For Level 3
        if(targetSceneName == "Level3")
        {
            if (powerManager == null)
            {
                Debug.LogError("Missing PowerLevelManager in the scene.");
            }

            if (countdownText == null)
            {
                Debug.LogError("Countdown TextMeshProUGUI is not assigned.");
            }

            if (FixedShadowmanBehaviourTriggers == null || FixedShadowmanBehaviourTriggers.Count == 0)
            {
                Debug.LogWarning("No Shadowman Behaviour Triggers assigned.");
            }
        }
    }

    void Update()
    {
        HandleElevatorInteraction();
    }




    // Unlock
    public override void Unlock()
    {
        base.Unlock();
        Debug.Log("Elevator unlocked!");
    }




    // Handle Elevator Interaction
    private void HandleElevatorInteraction()
    {
        if (myInteractableObj.IsPlayerInRange && Input.GetKeyDown(KeyCode.E) && !isLocked && !isCountdownActive)
        {
            // Level 3
            if (targetSceneName == "Level3")
            {
                PlayAudio(elevatorActivateSound);
                Debug.Log("Elevator activated, starting countdown for Level3.");

                // Activate triggers
                ActivateShadowmanTriggers();

                // Start
                StartCoroutine(StartElevatorCoolDown());
            }
            else if (targetSceneName == "End")
            {
                // End Level
                PlayAudio(elevatorActivateSound);
                Debug.Log("Elevator activated, starting countdown for End Level.");

                // Check End Scene
                CheckClueAmount();
            }
            // Other Level
            else
            {
                Debug.Log($"Elevator activated, loading {targetSceneName} directly.");
                StartCoroutine(LoadTargetScene(targetSceneName));
            }
        }
        else if (myInteractableObj.IsPlayerInRange && Input.GetKeyDown(KeyCode.E) && isLocked)
        {
            Debug.Log("Elevator is locked!");
        }
    }




    // Check clue amount
    private void CheckClueAmount()
    {
        int clueCollected = ClueBoardController.Instance.GetClueCollected();

        // Bad
        if(clueCollected < 5)
        {
            StartCoroutine(LoadTargetScene("End1_Bad"));
        }
        // Mediumn
        else if (clueCollected >= 5 && clueCollected <= 9)
        {
            StartCoroutine(LoadTargetScene("End2_Medium"));
        }
        // Good
        else if (clueCollected >= 10)
        {
            StartCoroutine(LoadTargetScene("End3_Good"));
        }
        else
        {
            StartCoroutine(LoadTargetScene("End3_Good"));
        }
    }




    // Activate Shadowman Triggers
    private void ActivateShadowmanTriggers()
    {
        foreach (var trigger in FixedShadowmanBehaviourTriggers)
        {
            if (trigger != null)
            {
                // Activate each trigger
                trigger.SetActive(true);
                Debug.Log($"Activated Shadowman Trigger: {trigger.name}");
            }
        }
    }




    // Start Elevator
    private IEnumerator StartElevatorCoolDown()
    {
        isCountdownActive = true;
        float countdown = coolDownDuration;

        while (countdown > 0)
        {
            yield return new WaitForSeconds(1f);
            countdown -= 1f;

            // Update UI Text
            UpdateUItext(countdown);
        }

        // Countdown finished, check power level
        if (powerManager != null && powerManager.currentPower > 0)
        {
            Debug.Log("Power is sufficient. Loading the target scene.");
            StartCoroutine(LoadTargetScene(targetSceneName));
        }
        else
        {
            Debug.Log("Power is depleted. Elevator cannot be activated.");
        }

        isCountdownActive = false;
    }

    // Update UI Text
    private void UpdateUItext(float countdown)
    {
        if (countdownText != null)
        {
            countdownText.text = $"Time Remaining {Mathf.CeilToInt(countdown)} seconds";
        }
    }




    // Play Audio
    private void PlayAudio(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }




    // Load Target Scene
    private IEnumerator LoadTargetScene(string targetSceneName)
    {
        SceneManager.LoadScene(targetSceneName);
        yield break;
    }
}
