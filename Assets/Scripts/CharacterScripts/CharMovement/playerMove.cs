using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // Public Fields
    public CharacterController controller;
    public Transform groundCheck;
    public LayerMask groundMask;

    [Header("Movement")]
    public float walkSpeed = 2f;
    public float runSpeed = 4f;
    public float gravity = -19.62f;
    public float jumpHeight = 1f;
    public float mouseSensitivity = 50f;

    public float groundDistance = 0.4f;
    public float jumpCooldown = 0.7f;
    public float inputThreshold = 0.1f;

    public bool isRunning;
    public bool isMoving;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip walkClip;
    public AudioClip runClip;
    public float moveSound = 0.15f;

    [Header("Sound Sphere Settings")]
    public GameObject soundSpherePrefab;
    public float soundSphereMaxRadius = 1f;
    public float soundSphereExpansionSpeed = 30f;

    private GameObject currentSoundSphereInstance;  



    // Private Fields
    private Vector3 velocity;
    private Vector3 moveDirection;
    private float lastJumpTime = -1f;

    private bool isGrounded;
    private float currentSpeed;

    // Char Status
    private CharStatusManager statusManager;
    private StaminaManager staminaManager;

    public float getMouseSensitivityX()
    {
        return this.mouseSensitivity;
    }


    public void setMouseSensitivityX(float mouseSensitivityX)
    {
        this.mouseSensitivity = mouseSensitivityX;
    }




    private void Start()
    {
        // Get Components
        statusManager = GetComponent<CharStatusManager>();
        staminaManager = GetComponent<StaminaManager>();
        audioSource = GetComponent<AudioSource>();

        // If Audio Source Not Found
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found");
        }

        // Lock Cursor
        Cursor.lockState = CursorLockMode.Locked;
    }




    private void Update()
    {
        HandleGroundCheck();
        HandleMovement();
        HandleJump();
        ApplyGravity();
        HandleRotation();
        staminaManager.ManageStamina();
    }




    // Get Current Speed
    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }




    // Check Ground
    private void HandleGroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Reset downward velocity
        }
    }




    // Handle Movement
    private void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Run
        isRunning = Input.GetKey(KeyCode.LeftShift) && statusManager.CanRun();
        isMoving = Mathf.Abs(x) > inputThreshold || Mathf.Abs(z) > inputThreshold;

        // Set Speed
        float targetSpeed = isRunning ? runSpeed : walkSpeed;

        // Set speed to half when Moving Backwards
        if (z < 0)
        {
            targetSpeed *= 0.7f;
        }

        moveDirection = (transform.right * x + transform.forward * z).normalized;

        // Control Movement
        Vector3 movement = moveDirection * (isMoving ? targetSpeed : 0.0f);
        currentSpeed = movement.magnitude;
        controller.Move(movement * Time.deltaTime);

        // Play Move Sound
        HandlePlayMoveAudio(isMoving);
    }




    // Create Sound Sphere
    void CreateSoundSphere()
    {

        if (currentSoundSphereInstance == null)
        {
            // Create a new sound sphere instance
            currentSoundSphereInstance = Instantiate(soundSpherePrefab, transform.position, Quaternion.identity);

            // Configure the sound sphere
            SoundSphere soundSphereScript = currentSoundSphereInstance.GetComponent<SoundSphere>();
            soundSphereScript.maxRadius = isRunning ? soundSphereMaxRadius * 2 : soundSphereMaxRadius;
            soundSphereScript.expansionSpeed = soundSphereExpansionSpeed;

            // Register callback when sound sphere is destroyed
            soundSphereScript.OnDestroyed += () => currentSoundSphereInstance = null;
        }

    }




    // Handle Jump
    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && Time.time - lastJumpTime >= jumpCooldown)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            lastJumpTime = Time.time;
        }
    }

    // Handle Rotation
    private void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);
    }

    // Apply Gravity
    private void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }




    // Handle Play Move Audio
    private void HandlePlayMoveAudio(bool isMoving)
    {
        if (isRunning && (GetCurrentSpeed() > 0.01f))
        {
            turnOnAudio(runClip, true, moveSound); // Running Sound, Loop, Vol
            CreateSoundSphere();
        }
        else
        {
            if (isMoving)
            {
                turnOnAudio(walkClip, true, moveSound); // Walking Sound, Loop, Vol
                CreateSoundSphere();
            }
            else if (!isMoving)
            {
                turnOffAudio(); // Turn Off Audio
            }
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
