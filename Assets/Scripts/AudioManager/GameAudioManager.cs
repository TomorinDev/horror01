using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudioManager : MonoBehaviour
{
    // Public Field
    public static GameAudioManager Instance;

    // Private Field
    private audioSystem audioSystem;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        audioSystem = gameObject.GetComponent<audioSystem>();
    }

    // Play Audio
    public void playAudio()
    {
        Debug.Log(audioSystem.playAudio());
    }
}