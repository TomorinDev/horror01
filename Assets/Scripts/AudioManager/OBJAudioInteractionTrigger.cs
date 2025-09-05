using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBJAudioInteractionTrigger : MonoBehaviour
{
    // Public Fields
    public AudioClip backgroundMusicClip;
    public bool loopMusic = true;

    // Private Fields
    private AudioSource audioSource;
    private bool playerIsInRange = false; // Player Is In Range
    private bool musicPlayed = false;

    private void Start()
    {
        // Get Component
        audioSource = GetComponent<AudioSource>();

        if (audioSource != null && backgroundMusicClip != null)
        {
            audioSource.clip = backgroundMusicClip;
            audioSource.loop = loopMusic;

            // Preload Audio
            audioSource.Play();
            audioSource.Pause();
        }
    }




    // Mark Player Is In Range
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsInRange = true;

            // Play the background music
            PlayAudioSource();
        }
    }




    // Mark Player Is Not In Range
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsInRange = false;

            // Stop the background music
            StopAudioSource();
        }
    }




    // Play the background music
    void PlayAudioSource()
    {
        if (audioSource != null && backgroundMusicClip != null && !musicPlayed)
        {
            audioSource.UnPause();
            musicPlayed = true; // Is Played
        }
    }




    // Stop the background music
    void StopAudioSource()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Pause();
            musicPlayed = false; // Reset
        }
    }
}
