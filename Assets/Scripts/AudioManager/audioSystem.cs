using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioSystem : MonoBehaviour
{
    // Audio Clips Sources
    public AudioClip audioClip;
    private AudioSource audioSource;

    void Start()
    {
        // Get Component
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Set Clip
        audioSource.clip = audioClip;
    }

    // Playing Audio
    public string playAudio()
    {
        if (audioClip != null && !audioSource.isPlaying)
        {
            audioSource.Stop();
            audioSource.Play();
            return "Audio is now playing: " + audioClip.name;
        }
        else if (audioSource.isPlaying)
        {
            return "Audio is already playing.";
        }
        else
        {
            return "No audio clip assigned.";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
