using System.Collections;
using UnityEngine;
using Unity.AI.Navigation;
using UnityEngine.AI;

public class DoorController : Unlockable
{
    [Header("Door Settings")]
    public float openAngle = 80.0f;
    public float closeAngle = 0.0f;
    public float animationSpeed = 1.5f;

    [Header("Audio Clips")]
    public AudioClip doorOpenSound;
    public AudioClip doorCloseSound;

    private bool isOpen = false;
    private bool isAnimating = false;
    private Quaternion targetRotation;
    private AudioSource audioSource;
    private interactableObj myInteractableObj;
    private NavMeshObstacle navMeshObstacle;

    void Start()
    {
        targetRotation = transform.localRotation;
        audioSource = GetComponent<AudioSource>();
        myInteractableObj = GetComponent<interactableObj>();
        navMeshObstacle = GetComponent<NavMeshObstacle>();

        if (audioSource == null)
        {
            Debug.LogError("Missing AudioSource component on the door.");
        }
    }

    void Update()
    {
        HandleDoorInteraction();
    }

    public override void Unlock()
    {
        base.Unlock();
        Debug.Log("Door unlocked!");
    }

    private void HandleDoorInteraction()
    {
        if (myInteractableObj.IsPlayerInRange && Input.GetKeyDown(KeyCode.E) && !isAnimating && !isLocked)
        {
            isOpen = !isOpen;
            targetRotation = Quaternion.Euler(0, isOpen ? openAngle : closeAngle, 0);
            PlayAudio(isOpen ? doorOpenSound : doorCloseSound);
            navMeshObstacle.carving = !isOpen;
            StartCoroutine(AnimateDoor());
        }
    }

    private void PlayAudio(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    private IEnumerator AnimateDoor()
    {
        isAnimating = true;
        while (Quaternion.Angle(transform.localRotation, targetRotation) > 0.1f)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * animationSpeed);
            yield return null;
        }
        transform.localRotation = targetRotation;
        isAnimating = false;
    }
}
