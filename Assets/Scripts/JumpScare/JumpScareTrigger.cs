using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class JumpScareTrigger : MonoBehaviour
{
    [Header("Image Settings")]
    public GameObject jumpScareImageObject;
    public Sprite jumpScareSprite;

    [Header("Audio Settings")]
    public AudioClip scareSound;

    [Header("Global Volume Settings")]
    public Volume globalVolume;
    public float vignetteIntensity = 1f;

    public float waitTime = 5f;
    public float imageDisplayTime = 3f; 

    private AudioSource audioSource;
    private bool hasTriggered = false;
    private Vignette vignette;
    private Image jumpScareImage;

    private void Awake()
    {
        // Get Component
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
        audioSource.loop = false;

        // Get the Vignette effect
        if (globalVolume != null && globalVolume.profile.TryGet(out Vignette v))
        {
            vignette = v;
        }
        else
        {
            Debug.LogError("No Vignette found in the Global Volume.");
        }

        // Initialize the jump scare image
        if (jumpScareImageObject != null)
        {
            jumpScareImage = jumpScareImageObject.GetComponent<Image>();
            if (jumpScareImage != null)
            {
                jumpScareImageObject.SetActive(false); // Start inactive
                if (jumpScareSprite != null)
                {
                    jumpScareImage.sprite = jumpScareSprite; // Set the sprite
                }
                else
                {
                    Debug.LogWarning("JumpScareSprite is not assigned");
                }
            }
            else
            {
                Debug.LogError("No Image component found on JumpScareImageObject.");
            }
        }
        else
        {
            Debug.LogError("JumpScareImageObject is not assigned.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            StartCoroutine(HandleJumpScare());
        }
    }

    private IEnumerator HandleJumpScare()
    {
        yield return new WaitForSeconds(waitTime);

        if (scareSound != null)
        {
            audioSource.clip = scareSound;
            audioSource.Play();
        }

        if (jumpScareImageObject != null)
        {
            jumpScareImageObject.SetActive(true);
        }

        if (vignette != null)
        {
            vignette.intensity.Override(vignetteIntensity);
        }

        yield return new WaitForSeconds(imageDisplayTime);

        if (jumpScareImageObject != null)
        {
            jumpScareImageObject.SetActive(false);
        }

        if (vignette != null)
        {
            vignette.intensity.Override(0.3f);
        }
    }
}
