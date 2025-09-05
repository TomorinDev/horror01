using UnityEngine;

public class EnemySoundTrigger : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip stunClip;
    public AudioClip attackClip;
    public AudioClip chaseClip;
    public AudioClip walkClip;

    private EnemyAI enemyAI;
    private Animator animator;
    private string currentState = "";

    void Start()
    {
        // Get Components
        animator = GetComponent<Animator>();
        enemyAI = GetComponent<EnemyAI>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Check EnemyAI State
        if (enemyAI != null)
        {
            CheckEnemyAIstate();
        }
    }

    // Check EnemyAI State
    private void CheckEnemyAIstate()
    {
        if (animator.GetBool("isStunned"))
        {
            PlaySound(stunClip, "Stunning", true);
        }
        else if (animator.GetBool("isAttacking"))
        {
            PlaySound(attackClip, "Attacking", true);
        }
        else if (enemyAI.isChasing && currentState != "Chasing")
        {
            PlaySound(chaseClip, "Chasing", false);
        }
        else if (enemyAI.isWalking && currentState != "Walking")
        {
            PlaySound(walkClip, "Walking", false);
        }
        else if (!enemyAI.isAttacking && !enemyAI.isChasing && !enemyAI.isWalking && currentState != "Idle")
        {
            // Stop Audio
            audioSource.Stop();
            currentState = "Idle";
        }
    }

    // Play Sound
    private void PlaySound(AudioClip clip, string state, bool uninterruptible)
    {
        if (clip == null)
        {
            Debug.LogWarning("Audio " + state + " is Not Assigned");
            return;
        }

        // Currently play uninterruptible sound
        if (uninterruptible && audioSource.isPlaying && audioSource.clip == clip)
        {
            return;
        }

        HandlePlaySound(clip, state);
    }

    // Handle Play Sound
    private void HandlePlaySound(AudioClip clip, string state)
    {
        if (audioSource.clip != clip || !audioSource.isPlaying)
        {
            audioSource.clip = clip;
            audioSource.Play();
            currentState = state;
        }
    }
}
