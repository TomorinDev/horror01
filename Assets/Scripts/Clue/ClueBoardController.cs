using UnityEngine;

public class ClueBoardController : MonoBehaviour
{
    // Singleton instance
    public static ClueBoardController Instance { get; private set; }
    public int clueCollected = 0;

    [Header("Clue Board")]
    public GameObject clueBoard; // Reference to the Clue Board GameObject

    public ClueSlotController[] clueSlots;      

    private bool isClueBoardVisible = false; // Default state is hidden

    // Get Clue Collected
    public int GetClueCollected()
    {
        return clueCollected;
    }


    private void Awake()
    {
        // Ensure there is only one instance
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Multiple instances of ClueBoardController detected! Destroying duplicate.");
            Destroy(gameObject);
        }
    }


    void Start()
    {
        // Ensure the Clue Board starts in the hidden state
        if (clueBoard != null)
        {
            clueBoard.SetActive(false);

            // Simulate pressing "C" twice
            Invoke(nameof(SimulateClueBoardToggle), 0.1f); // Simulate opening
            Invoke(nameof(SimulateClueBoardToggle), 0.2f); // Simulate closing
        }
    }


    private void SimulateClueBoardToggle()
    {
        ToggleClueBoard();
    }


    void Update()
    {
        // Toggle Clue Board visibility when pressing the "C" key
        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleClueBoard();
        }
    }

    private void ToggleClueBoard()
    {
        if (clueBoard != null)
        {
            isClueBoardVisible = !isClueBoardVisible;
            clueBoard.SetActive(isClueBoardVisible); // Show or hide the board
        }
        else
        {
            Debug.LogWarning("Clue Board reference is not assigned!");
        }
    }

    public void UpdateClueSlot(int clueID)
    {
        // Ensure the clueID is within range and update the slot
        if (clueSlots != null && clueID >= 0 && clueID < clueSlots.Length)
        {
            // Add Clue Count
            clueCollected++;
            clueSlots[clueID].RevealClue();
        }
        else
        {
            Debug.LogWarning($"Invalid clue ID: {clueID} or clueSlots not assigned.");
        }
    }

}
