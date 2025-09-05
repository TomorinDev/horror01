using UnityEngine;

public class ItemFlashlight : MonoBehaviour, IUsable
{
    private Light flashlightLight; // Reference to the flashlight's light component
    public bool isOn = false; // Flashlight state
    private InventoryController playerInventory; // Reference to the player's inventory controller



    [Header("Audio Settings")]
    private AudioSource audioSource; // Flashlight audio source
    public AudioClip turnOnSound; // Sound effect for turning on the flashlight
    public AudioClip turnOffSound; // Sound effect for turning off the flashlight



    [Header("Battery Consumption Settings")]
    public float consumptionRate = 1f; // Battery consumption rate (units per second)


    void Start()
    {
        // Get Components
        InitialGetComponents();

        // Find the InventoryController in the scene
        playerInventory = FindObjectOfType<InventoryController>();
    }




    void Update()
    {
        if (isOn)
        {
            // Consume battery power at the specified rate
            playerInventory.ConsumeBatteryPower(consumptionRate * Time.deltaTime);

            // If no battery is available, turn off the flashlight
            if (!playerInventory.HasBattery())
            {
                ToggleFlashlight(false);
            }
        }
    }




    // Get Components 
    private void InitialGetComponents()
    {
        // Find the Light component on the flashlight
        flashlightLight = GetComponent<Light>();
        if (flashlightLight != null)
        {
            flashlightLight.enabled = false; // Set the initial state to OFF
        }

        // Find or add an AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }




    // Called when the flashlight is used
    public void Use()
    {
        ToggleFlashlight(!isOn);
    }


    // Toggles the flashlight on or off
    public void ToggleFlashlight(bool turnOn)
    {
        if (turnOn)
        {
            // Check if there is a valid battery available
            InventorySlot batterySlot = playerInventory.GetFirstAvailableBatterySlot();
            if (batterySlot == null)
            {
                return;
            }
        }

        // Update flashlight state
        isOn = turnOn;

        // Enable or disable the Light component
        if (flashlightLight != null)
        {
            flashlightLight.enabled = isOn;
        }
        // Play the corresponding sound effect
        PlaySound(isOn ? turnOnSound : turnOffSound);
    }




    // Plays a sound effect
    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
