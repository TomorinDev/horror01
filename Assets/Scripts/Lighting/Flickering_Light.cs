using UnityEngine;

public class FlickeringLightController : MonoBehaviour
{
    // Enum to define flicker modes
    public enum FlickerMode
    {
        Intensity,  // Flicker by changing intensity
        OnOff       // Flicker by toggling on/off
    }

    [Header("Flicker Settings")]
    [Tooltip("Select the flicker mode: Intensity or On/Off.")]
    public FlickerMode flickerMode = FlickerMode.Intensity;

    private Light lightSource;

    [Header("Intensity Flicker Settings")]
    [Tooltip("Minimum intensity of the light.")]
    public float minIntensity = 0.5f;

    [Tooltip("Maximum intensity of the light.")]
    public float maxIntensity = 2.0f;

    [Tooltip("Speed of the intensity flicker effect.")]
    public float intensityFlickerSpeed = 0.1f;

    [Tooltip("Randomness factor to make the intensity flicker more natural.")]
    public float intensityRandomness = 0.5f;

    [Header("On/Off Flicker Settings")]
    [Tooltip("Minimum time (in seconds) between on/off flickers.")]
    public float minOnOffTime = 0.05f;

    [Tooltip("Maximum time (in seconds) between on/off flickers.")]
    public float maxOnOffTime = 0.2f;

    private float targetIntensity;
    private float currentIntensity;
    private float intensityTimer;
    private float onOffTimer;

    void Start()
    {
        // Get the Light component
        lightSource = GetComponent<Light>();
        if (lightSource == null)
        {
            Debug.LogError("FlickeringLightController script requires a Light component on the same GameObject.");
            enabled = false;
            return;
        }

        // Initialize timers and intensities
        targetIntensity = lightSource.intensity;
        currentIntensity = targetIntensity;
        ResetOnOffTimer();
    }

    void Update()
    {
        // Update based on the selected flicker mode
        switch (flickerMode)
        {
            case FlickerMode.Intensity:
                IntensityFlicker();
                break;
            case FlickerMode.OnOff:
                OnOffFlicker();
                break;
        }
    }

    private void IntensityFlicker()
    {
        if (intensityTimer <= 0f)
        {
            // Set a new random target intensity
            targetIntensity = Random.Range(minIntensity, maxIntensity);
            intensityTimer = Random.Range(intensityFlickerSpeed - intensityRandomness, intensityFlickerSpeed + intensityRandomness);
        }

        // Smoothly transition to the target intensity
        currentIntensity = Mathf.Lerp(currentIntensity, targetIntensity, Time.deltaTime * 10f);
        lightSource.intensity = currentIntensity;

        // Decrease the timer
        intensityTimer -= Time.deltaTime;
    }

    private void OnOffFlicker()
    {
        // Count down the flicker timer
        onOffTimer -= Time.deltaTime;

        if (onOffTimer <= 0f)
        {
            // Toggle the light's on/off state
            lightSource.enabled = !lightSource.enabled;

            // Reset the timer
            ResetOnOffTimer();
        }
    }

    private void ResetOnOffTimer()
    {
        onOffTimer = Random.Range(minOnOffTime, maxOnOffTime);
    }
}
