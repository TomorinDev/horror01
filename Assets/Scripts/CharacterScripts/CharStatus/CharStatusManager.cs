using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharStatusManager : MonoBehaviour
{
    // Health
    [Header("Health Settings")]
    public Slider healthBar; // Health Bar UI
    public float maxHealth = 100f;
    public float currentHealth;




    // Stamina
    [Header("Stamina Settings")]
    public Slider staminaBar; // Stamina Bar UI
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaDrainRate = 10f;
    public float minStaminaForRun = 10f;

    // Recovery
    public float staminaRecoveryRate = 5f;
    public float staminaRecoveryDelay = 2f; // Delay
    private float staminaRecoveryTimer = 0f; // Timer to track the delay




    // Sanity
    [Header("Sanity Settings")]
    public Slider sanityBar; // Sanity Bar UI
    public float minSanity = 0f;
    public float maxSanity = 100f;
    public float currentSanity;

    // Recovery Delay
    public float sanityRecoveryRate = 5f;
    public float sanityRecoveryDelay = 6f; // Delay
    public float sanityRecoveryTimer = 0f; // Timer to track the delay




    // Restart Scene Time
    [Header("Restart Scene Settings")]
    public float RestartSceneTime = 0f;


    private Animator animator;
    private StatusPPEManager effectsManager;




    // Getter
    public float GetSanity()
    {
        return currentSanity;
    }




    void Start()
    {
        // Get Component
        animator = GetComponent<Animator>();
        effectsManager = GetComponent<StatusPPEManager>();

        // Check If Set Up Bars
        if (healthBar == null || staminaBar == null)
        {
            Debug.LogWarning("One or more UI elements are not assigned");
        }

        // Initialize Health
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;

        // Initialize Stamina
        currentStamina = maxStamina;
        staminaBar.maxValue = maxStamina;
        staminaBar.value = currentStamina;

        // Initialize Sanity
        currentSanity = maxSanity;
        sanityBar.maxValue = maxSanity;
        sanityBar.value = currentSanity;
    }




    void Update()
    {
        // Update Post Processing Effect
        if (effectsManager != null)
        {
            effectsManager.UpdatePPE(currentStamina, maxStamina, currentHealth, maxHealth, currentSanity, maxSanity);
        }
    }




    // Drain Health
    public void TakeDamage(float damageAmount)
    {
        // If alive, can decrease health
        if (!animator.GetBool("isKilled"))
        {
            currentHealth -= damageAmount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            UpdateHealthBar();
        }

        // Die
        if (currentHealth <= 0)
        {
            animator.SetBool("isKilled", true);
            Debug.Log("Character is dead.");

            // Restart Scene after 2 seconds
            Invoke("RestartScene", RestartSceneTime);
        }
    }


    // Heal Health
    public void Heal(float healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();
    }

    // Update Health Bar
    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }
    }




    // Drain Stamina
    public void DrainStamina(float deltaTime)
    {
        currentStamina -= staminaDrainRate * deltaTime;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        staminaRecoveryTimer = 0f;
        UpdateStaminaBar();
    }

    // Recover Stamina
    public void RecoverStamina(float deltaTime)
    {
        // Start recovering after the delay
        if (staminaRecoveryTimer >= staminaRecoveryDelay)
        {
            currentStamina += staminaRecoveryRate * deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        }
        else
        {
            staminaRecoveryTimer += deltaTime; // Increment the timer
        }
        UpdateStaminaBar();
    }

    // Update Stamina Bar
    private void UpdateStaminaBar()
    {
        if (staminaBar != null)
        {
            staminaBar.value = currentStamina;
        }
    }

    // Check if Stamina still available for running
    public bool CanRun()
    {
        return currentStamina > minStaminaForRun;
    }




    // Drain Sanity
    public void DrainSanity(float drainAmount)
    {
        currentSanity -= drainAmount;
        currentSanity = Mathf.Clamp(currentSanity, minSanity, maxSanity);
        UpdateSanityBar();

        // Check if sanity is completely drained
        if (currentSanity <= 0)
        {
            Debug.Log("Character has lost all sanity.");
        }
    }

    // Recover Sanity
    public void RecoverSanity(float deltaTime)
    {
        // Start recovering after the delay
        if (sanityRecoveryTimer >= sanityRecoveryDelay)
        {
            currentSanity += sanityRecoveryRate * deltaTime;
            currentSanity = Mathf.Clamp(currentSanity, 0, maxSanity);
        }
        else
        {
            sanityRecoveryTimer += deltaTime; // Increment the timer
        }
        UpdateSanityBar();
    }

    // Update Sanity Bar
    private void UpdateSanityBar()
    {
        if (sanityBar != null)
        {
            sanityBar.value = currentSanity;
        }
    }




    // Die: Restart the scene
    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }
}