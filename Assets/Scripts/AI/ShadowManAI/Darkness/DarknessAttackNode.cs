using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;
using UnityEditor;
using System;

public class DarknessAttackNode : BehaviorTreeNode
{
    private DarknessDetection darknessDetection; // Darkness Detection
    private CharStatusManager charStatusManager; // Players Status
    // private RandomEyesEffect swirlingEyesEffect;
    private EyeEffectController eyeEffectController; // Eye Effect
    private HorrorWallEffectController horrorWallEffectController; // Horror Wall Effect
    private DarknessPPEController darknessPPEController; // Darkness Post Processing Effect
    private MonoBehaviour caller;

    private bool crescendoPlaying = false;
    private float attackStartTime;
    private float lastPlayTime = -6f; // Track the last time audio was played
    private float playTimeInterval = 6f; // Play Time Interval


    [Header("Sanity Settings")]
    public float sanityDrainRate = 10f;


    [Header("Attack Settings")]
    public float attackDuration = 5f; // Time before the player dies
    public float darknessDamage = 100f; // Full damage to kill player


    [Header("Audio Settings")]
    private AudioSource audioSource;
    private AudioClip audioClip; // Crescendo audio clip




    // Darkness Attack Node
    public DarknessAttackNode(
        DarknessDetection detection, 
        CharStatusManager statusManager, 
        AudioSource source, 
        AudioClip clip, 
        MonoBehaviour monoCaller, 
        EyeEffectController eyeEffect, 
        HorrorWallEffectController horrorWallEffectController, 
        DarknessPPEController darknessPPEController
        )
    {
        darknessDetection = detection;
        charStatusManager = statusManager;
        audioSource = source;
        audioClip = clip;
        caller = monoCaller; // This Thing, Use For MonoBehaviour
        eyeEffectController = eyeEffect;
        this.horrorWallEffectController = horrorWallEffectController;
        this.darknessPPEController = darknessPPEController;
    }




    public override NodeState Evaluate()
    {
        // If Player In Darkness
        if (darknessDetection.isInDarkness)
        {
            // Check if Sanity is zero
            if (charStatusManager.GetSanity() <= 0)
            {
                if (!crescendoPlaying)
                {
                    // Start crescendo and visual effects
                    crescendoPlaying = true;
                    attackStartTime = Time.time;
                    Debug.Log("Crescendo attack starts!");

                    // Eye Effect
                    // swirlingEyesEffect.Initialize(attackDuration);

                    // Start Play Audio
                    StartPlayAudio();
                }


                // Start Visual Effects
                HandleStartVisualEffects();


                // Horror Wall Effect Enable
                horrorWallEffectController.EnableHorrorWall();


                // Attack Kill
                if (Time.time - attackStartTime >= attackDuration)
                {
                    // Kill the player by dealing full damage
                    DarkAttack();

                    crescendoPlaying = false;

                    // Eye Effect Disable
                    eyeEffectController.DisableEye();
                    // swirlingEyesEffect.ClearEyes();

                    return NodeState.Success;
                }

                return NodeState.Running; // Attack is ongoing
            }
            else
            {
                // Drain Sanity
                charStatusManager.DrainSanity(Time.deltaTime * sanityDrainRate);
                return NodeState.Running; // Continue draining sanity
            }
        }
        else
        {
            // Player Escape
            HandlePlayerEscape();
            return NodeState.Failure;
        }
    }




    // Dark Attack
    private void DarkAttack()
    {
        Debug.Log("Player killed by darkness attack!");

        // Apply damage
        charStatusManager.TakeDamage(darknessDamage);
    }




    // Handle Start Visual Effect
    private void HandleStartVisualEffects()
    {
        // Eye Effects
        eyeEffectController.TriggerEyeEffect();

        // PPE
        darknessPPEController.StartPPE(attackStartTime, attackDuration);
    }




    // Handle Player Escape
    private void HandlePlayerEscape()
    {
        // Reset if the player escapes
        if (crescendoPlaying)
        {
            Debug.Log("Player escaped the darkness!");
            crescendoPlaying = false;

            // Reset Sanity Recover Timer
            charStatusManager.sanityRecoveryTimer = 0f;

            // Eye Effect Disable
            eyeEffectController.DisableEye();

            // Horror Wall Effect Disable
            horrorWallEffectController.DisableHorrorWall();

            // Reset PPE
            darknessPPEController.ResetPPE();

            // Stop Play Audio
            StopPlayAudio();
        }

        // Recover sanity over time
        charStatusManager.RecoverSanity(Time.deltaTime);
    }




    // Start Play Audio
    private void StartPlayAudio()
    {
        // Trigger crescendo audio
        if (audioClip != null && audioSource != null) // Time Exceed 
        {
            float timePass = Time.time - lastPlayTime;

            if (timePass >= playTimeInterval)
            {
                HandleStartPlayAudio();
            }
        }
    }

    private void HandleStartPlayAudio()
    {
        audioSource.clip = audioClip;
        audioSource.Play();
        lastPlayTime = Time.time;
    }


    // Stop Play Audio
    private void StopPlayAudio()
    {
        AudioFadeOut.StartFadeOutAudioCoroutine(caller, audioSource, 1f);
    }
}
