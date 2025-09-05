using UnityEngine;

public class GlobalAudioTrigger : MonoBehaviour, IPlayerIsInRangeTrigger
{
    // Public Fields

    public bool playerInRange;
    public bool IsPlayerInRange => playerInRange;
    public bool canLoop = false;
    public bool canCut = false;
    public float fadeDuration;

    // Private Fields
    [SerializeField] private AudioClip musicClip;
    private AudioSource audioSource;
    private bool hasTriggered = false;

    private void Awake()
    {
        // Get Component
        audioSource = gameObject.GetComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the entering object is the player, and music hasn't triggered before
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            PlayMusic();
        }
    }

    // Play Music
    private void PlayMusic()
    {
        if (musicClip != null)
        {
            audioSource.clip = musicClip;
            audioSource.loop = canLoop;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Music clip is not assigned");
        }
    }

    // IPlayerIsInRangeTrigger
    public void OnPlayerEnter()
    {
        playerInRange = true;

        if (!hasTriggered)
        {
            hasTriggered = true;
            PlayMusic();
        }
    }

    public void OnPlayerExit()
    {
        playerInRange = false;

        if(canCut)
        {
            AudioFadeOut.StartFadeOutAudioCoroutine(this, audioSource, fadeDuration);
        }
    }
}
