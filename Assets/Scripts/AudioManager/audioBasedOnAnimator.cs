using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioBasedOnAnimator : MonoBehaviour
{
    // Public Fields
    public AudioClip walkingClip;
    public AudioClip runningClip;
    public AudioClip jumpingClip;
    public AudioClip poweringClip;

    // Private Fields
    private Animator animator;
    private AudioSource audioSource;

    void Start()
    {
        // Get Component
        animator = gameObject.GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on " + gameObject.name);
        }

        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found on " + gameObject.name);
        }
    }

    void Update()
    {
        // Check Animator Bools
        bool isWalking = animator.GetBool("isWalking");
        bool isRunning = animator.GetBool("isRunning");
        bool isPowering = animator.GetBool("isPowering");
        bool isJumping = animator.GetBool("isJumping");

        // Play Audio based on the animator state
        if (isJumping)
        {
            turnOnAudio(jumpingClip, false, 0.05f); // Jumping Sound, No Loop
        }
        else if (isWalking)
        {
            turnOnAudio(walkingClip, true, 0.05f); // Walking Sound, Loop
        }
        else if (isRunning)
        {
            turnOnAudio(runningClip, true, 0.05f); // LRunning Sound, Loop
        }
        else if (isPowering)
        {
            turnOnAudio(poweringClip, false, 1.0f); // Powering sound, No Loop
        }

        // Stop playing
        if (!isWalking && !isRunning && !isJumping && !isPowering)
        {
            turnOffAudio();
        }
    }


    // Turn on Audio
    private void turnOnAudio(AudioClip clip, bool loop, float volume)
    {
        if (clip != null)
        {
            // Assign the clip and enable AudioSource
            audioSource.clip = clip;

            // Set looping based on the state
            audioSource.loop = loop;
            audioSource.enabled = true;

            // Set Volume
            audioSource.volume = volume;

            // When Can Loop
            if (loop)
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }
        }
    }

    // Turn off Audio
    private void turnOffAudio()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        audioSource.enabled = false; // Disable the AudioSource
    }
}
