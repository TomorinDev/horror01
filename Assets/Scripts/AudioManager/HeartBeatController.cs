using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class HeartbeatController : MonoBehaviour
{
    [Header("Initial Settings")]
    public Transform player;
    public Transform target;
    public AudioSource heartbeatAudio;

    [Header("Distance")]
    public float maxDistance = 10f;

    [Header("Play Interval")]
    public float minInterval = 0.2f; // How Fast Can Play
    public float maxInterval = 1.5f; // How Slow Can Play

    private float timer = 0f;
    private float currentInterval;
    private bool isAudioPlaying = false; // Track if audio is currently playing

    void Update()
    {
        PlayAudio();
    }

    // Play Audio
    private void PlayAudio()
    {
        // Calcualate Distance Between Player and Target
        float distance = Vector3.Distance(player.position, target.position);

        // Debug
        // Debug.Log("Distance: " + distance + ", Current Interval: " + currentInterval);

        HandleSetPlayInterval(distance);
        HandlePlayAudio();
    }

    // Handle Set Play Interval
    private void HandleSetPlayInterval(float distance)
    {
        // Set play interval depends on the distance
        currentInterval = Mathf.Lerp(minInterval, maxInterval, distance / maxDistance);
        currentInterval = Mathf.Clamp(currentInterval, minInterval, maxInterval);
    }

    // Handle Play Audio
    private void HandlePlayAudio()
    {
        // Play Audio
        timer += Time.deltaTime;

        // Check Can Play Audio: The time since the last time the sound effect was played
        bool canPlayAudio = timer >= currentInterval && !isAudioPlaying;

        // Only play audio if enough time has passed and no audio is currently playing
        if (canPlayAudio)
        {
            StartCoroutine(PlayHeartbeatSound());
            timer = 0f;
        }
    }

    // Coroutine to handle playing the audio
    private IEnumerator PlayHeartbeatSound()
    {
        isAudioPlaying = true; // Set Play Audio True

        heartbeatAudio.PlayOneShot(heartbeatAudio.clip);

        // Wait for the clip to finish playing
        yield return new WaitForSeconds(heartbeatAudio.clip.length);

        isAudioPlaying = false; // Set Play Audio False 
    }
}
