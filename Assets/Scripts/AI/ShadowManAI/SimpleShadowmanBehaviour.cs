using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleShadowmanBehaviour : MonoBehaviour, IPlayerIsInRangeTrigger
{
    [Header("General Settings")]
    public List<Light> lights; // Lights
    public List<GameObject> devices; // Device
    public Transform playerTransform; // Player
    [Tooltip("The action interval time in seconds. Define how fast to trigger the Enemy Random Action.")]
    public float actionInterval = 10.0f; // Action Interval

    private float timer;


    [Header("Attack Settings")]
    [Tooltip("Enemy Object Should Be Shadowman Hand")]
    public GameObject enemyObject; // Enemy GameObject
    public float attackDamage = 20f; // Damage per attack
    public float attackInterval = 2f; // Interval between attacks
    private float attackTimer;


    [Header("Player Status")]
    public CharStatusManager charStatusManager;


    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip audioClip_TurnOffLight;
    public AudioClip audioClip_TurnOnDevice;
    public AudioClip audioClip_AttackPlayer;


    [Header("Animator Get Script")]
    public ShadowmanAnimator shadowmanAnimator; // Enemy Animator Script


    // Proximity Trigger
    public bool playerInRange;
    public bool IsPlayerInRange => playerInRange;


    // Node: Just Keep in here, if we use that later or not
    /*
    public ProactiveActionNode(List<GameObject> deviceList, float actionInterval)
    {
        devices = deviceList;
        interval = actionInterval;
        timer = interval;
    }

    public override NodeState Evaluate()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            PerformRandomAction();
            timer = interval;
            return NodeState.Success;
        }

        return NodeState.Running;
    }*/

    void Start()
    {
        InitialGetComponents();

        // Set Default Timer
        timer = actionInterval;

        // Start Set False
        if (enemyObject != null)
        {
            enemyObject.SetActive(false);
        }
    }


    void Update()
    {
        TriggerBehaviour();
    }




    // Get Components
    private void InitialGetComponents()
    {
        // Audio Source
        audioSource = GetComponent<AudioSource>();
    }




    // Trigger Behaviour
    private void TriggerBehaviour()
    {
        // Check Player Enter Box Collider Range
        if (playerInRange)
        {
            timer -= Time.deltaTime;

            // Time Decrease To Zero
            if (timer <= 0)
            {
                // Perform Random Action
                PerformRandomAction();
                timer = actionInterval; // Reset Timer
            }
        }
    }




    // Perform Random Action
    private void PerformRandomAction()
    {
        int action = Random.Range(0, 3);

        switch (action)
        {
            case 0:
                TurnOffRandomLight();
                break;
            case 1:
                TurnOnRandomDevice();
                break;
            case 2:
                AttackPlayer();
                break;
        }
    }




    // Turn Off Random Light
    private void TurnOffRandomLight()
    {
        List<Light> availableLights = new List<Light>();

        // Find light in lights List, Add as lights that can be trigger for controlling
        foreach (var light in lights)
        {
            if (light.enabled)
                availableLights.Add(light);
        }

        if (availableLights.Count > 0)
        {
            HandleTurnOffRandomLight(availableLights);
        }
    }

    // Handle Turn Off Random Light
    private void HandleTurnOffRandomLight(List<Light> availableLights)
    {
        Debug.Log("Monster turns off a random light.");

        // Choose one random light to Turn Off
        Light selectedLight = availableLights[Random.Range(0, availableLights.Count)];
        selectedLight.enabled = false;

        // Play Trigger Audio: Turn Off Light
        PlayTriggerAudio(audioClip_TurnOffLight);
    }




    // Turn On Random Device
    private void TurnOnRandomDevice()
    {
        Debug.Log("Monster turns on a random device.");

        List<GameObject> availableDevices = new List<GameObject>();

        // Find Devices
        foreach (var device in devices)
        {
            DevicePower devicePower = device.GetComponent<DevicePower>();
            if (devicePower != null && !devicePower.isOn)
            {
                availableDevices.Add(device);
            }
        }

        if (availableDevices.Count > 0)
        {
            HandleTurnOnRandomDevice(availableDevices);
        }
    }

    // Handle Turn On Random Device
    private void HandleTurnOnRandomDevice(List<GameObject> availableDevices)
    {
        // Turn On Random Device
        GameObject selectedDevice = availableDevices[Random.Range(0, availableDevices.Count)];
        selectedDevice.GetComponent<DevicePower>().TurnOn();

        // Play Trigger Audio: Turn On Device
        PlayTriggerAudio(audioClip_TurnOnDevice);
    }




    // Attack Player
    private void AttackPlayer()
    {
        if (playerTransform == null)
        {
            Debug.LogWarning("Player transform is not set. Cannot attack.");
            return;
        }

        Debug.Log("Monster attacks the player!");

        // Active Enenmy OBJ
        ActiveEnemyOBJ();

        // Animator
        if (shadowmanAnimator != null || enemyObject != null) // Current is Active enemyOBJ
        {
            shadowmanAnimator.StartAttack();
        }
        else
        {
            Debug.LogError("ShadowmanAnimator is not assigned or enemyOBJ is deactive.");
        }

        // Attack
        AttackPlayerOverTime();
    }




    // Attack Player Over Time
    private void AttackPlayerOverTime()
    {
        // Player Is In Range
        if (playerInRange)
        {
            attackTimer -= Time.deltaTime;

            // Attack while timer to zero
            if (attackTimer <= 0)
            {
                HandleAttackPlayerOverTime();
            }
        }
    }

    // Handle Attack Player Over Time
    private void HandleAttackPlayerOverTime()
    { 
        if (charStatusManager != null)
        {
            // Play Trigger Audio: Attack Player
            PlayTriggerAudio(audioClip_AttackPlayer);

            // Damage to player
            charStatusManager.TakeDamage(attackDamage);
        }
        attackTimer = attackInterval; // Reset attack timer
    }




    // Active Enenmy OBJ
    private void ActiveEnemyOBJ()
    {
        Debug.Log("Active Shadowman Hand");
        // Activate enemy object when attacking
        if (enemyObject != null)
        {
            enemyObject.SetActive(true);

            // Initialize Animator
            if (shadowmanAnimator == null)
            {
                shadowmanAnimator.InitialGetComponents();
            }
        }
    }

    // Deactive Enemy OBJ
    private void DeactiveEnemyOBJ()
    {
        Debug.Log("Deactive Shadowman Hand");
        // Deactivate enemy object when player exits range
        if (enemyObject != null)
        {
            enemyObject.SetActive(false);
        }
    }




    // Play Trigger Audio
    private void PlayTriggerAudio(AudioClip audioClip)
    {
        // Play the turn off sound
        if (audioSource != null && audioClip != null)
        {
            audioSource.PlayOneShot(audioClip);
        }
    }




    // IPlayerIsInRangeTrigger
    public void OnPlayerEnter()
    {
        playerInRange = true;
    }

    public void OnPlayerExit()
    {
        playerInRange = false;

        DeactiveEnemyOBJ();
    }
}