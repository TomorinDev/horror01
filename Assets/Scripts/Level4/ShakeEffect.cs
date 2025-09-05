using UnityEngine;

public class ShakeEffectWithQuickBlackout : MonoBehaviour
{
    public Transform targetToShake; // The object to shake (e.g., Camera)
    public float shakeDuration = 0.5f; // Duration of the shake
    public float shakeMagnitude = 0.5f; // Intensity of the shake
    public GameObject objectToEnable; // The disabled object to enable
    public float disableDelay = 1f; // Delay before disabling this GameObject
    public Canvas blackoutCanvas; // Canvas with a full-screen black image
    public float blackoutDuration = 0.1f; // Duration of the blackout

    private Vector3 originalPosition; // Store the original position
    private bool isShaking = false;

    private void Start()
    {
        if (targetToShake != null)
        {
            originalPosition = targetToShake.localPosition;
        }

        if (objectToEnable != null)
        {
            objectToEnable.SetActive(false); // Ensure it's disabled initially
        }

        if (blackoutCanvas != null)
        {
            blackoutCanvas.gameObject.SetActive(false); // Make sure the canvas is initially hidden
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isShaking && targetToShake != null)
        {
            StartCoroutine(Shake());
        }

        if (objectToEnable != null)
        {
            objectToEnable.SetActive(true); // Enable the object
        }

        // Start a coroutine to disable this GameObject after the delay
        StartCoroutine(DisableAfterDelay());
    }

    private System.Collections.IEnumerator Shake()
    {
        isShaking = true;

        // Start the blackout effect
        if (blackoutCanvas != null)
        {
            StartCoroutine(QuickBlackout());
        }

        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            // Generate random shake offsets
            float offsetX = Random.Range(-1f, 1f) * shakeMagnitude;
            float offsetY = Random.Range(-1f, 1f) * shakeMagnitude;

            // Apply shake to the target position
            targetToShake.localPosition = originalPosition + new Vector3(offsetX, offsetY, 0f);

            elapsedTime += Time.deltaTime;

            yield return null; // Wait for the next frame
        }

        // Reset to the original position
        targetToShake.localPosition = originalPosition;
        isShaking = false;
    }

    private System.Collections.IEnumerator QuickBlackout()
    {
        // Activate blackout screen
        blackoutCanvas.gameObject.SetActive(true);

        // Wait for the blackout duration
        yield return new WaitForSeconds(blackoutDuration);

        // Deactivate blackout screen
        blackoutCanvas.gameObject.SetActive(false);
    }

    private System.Collections.IEnumerator DisableAfterDelay()
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(disableDelay);

        // Disable this GameObject
        gameObject.SetActive(false);
    }
}
