using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFadeOut : MonoBehaviour
{
    private static Coroutine fadeOutCoroutine;

    // Start Fade Out
    public static void StartFadeOutAudioCoroutine(MonoBehaviour caller, AudioSource audioSource, float duration)
    {
        // Stop if there is coroutine
        if (fadeOutCoroutine != null)
        {
            caller.StopCoroutine(fadeOutCoroutine);
            fadeOutCoroutine = null;
        }

        // Start Coroutine
        fadeOutCoroutine = caller.StartCoroutine(FadeOutAudio(audioSource, duration));
    }


    // Fade Out Audio
    private static IEnumerator FadeOutAudio(AudioSource audioSource, float duration)
    {
        float startVolume = audioSource.volume;
        duration = 1f; // Fade out duration in seconds
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / duration);
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume; // Reset volume to original value
    }
}
