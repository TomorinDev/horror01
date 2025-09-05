using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class questManager : MonoBehaviour
{
    public static questManager instance;

    // UI Text
    public Text questText;

    // UI Text displaying current quest list
    public Text questListText;

    // Display duration
    public float displayDuration = 3f;

    // Is displaying message
    private bool isDisplayingMessage = false;

    // List of quests
    public List<Quest> quests = new List<Quest>();

    // Target Zone
    public GameObject targetZonePrefab;

    // Arrow
    public GameObject arrowPrefab;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        questText.text = ""; // Initialize quest text to be hidden
        UpdateQuestListUI();
    }

    // Add a new quest to the list
    public void AddQuest(Quest newQuest, Vector3 targetPosition, Vector3 zoneSize)
    {
        quests.Add(newQuest);
        Debug.Log("New quest added: " + newQuest.questName);
        ShowQuestMessage("New Quest: " + newQuest.questName); // Quest Text Shown
        UpdateQuestListUI(); // Update UI

        // Create Target Zone
        if (targetZonePrefab != null)
        {
            CreateTargetZone(newQuest.questName, targetPosition, zoneSize);
        }
    }

    // Complete a quest
    public void CompleteQuest(Quest quest)
    {
        if (quests.Contains(quest) && !quest.isCompleted)
        {
            quest.isCompleted = true;
            ShowQuestMessage("Quest Completed: " + quest.questName);
            UpdateQuestListUI(); // Update UI
        }
    }

    // Create Target Zone
    private void CreateTargetZone(string questName, Vector3 position, Vector3 zoneSize)
    {
        GameObject targetZone = Instantiate(targetZonePrefab, position, Quaternion.identity);
        targetZone.transform.localScale = zoneSize;

        // Get Component
        TargetZone targetZoneScript = targetZone.GetComponent<TargetZone>();
        if (targetZoneScript != null)
        {
            targetZoneScript.questToComplete = questName; // Set Up Quest To Complete Name in Target Zone
        }

        // Create Arrow
        if (arrowPrefab != null)
        {
            // Set Arrow Position
            Vector3 arrowPosition = position + new Vector3(0, 15f, 0);
            GameObject arrow = Instantiate(arrowPrefab, arrowPosition, Quaternion.identity);

            // Set Arrow Parent
            arrow.transform.SetParent(targetZone.transform);

            // Set Arrow Scale
            arrow.transform.localScale = new Vector3(1f, 10f, 1f);

            // Rotate arrow -90 degrees for X axis
            // arrow.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);

            // Y axis infinite rotation, add a component
            arrow.AddComponent<ArrowRotation>();
        }
    }

    // Show quest status or objective updates
    public void ShowQuestMessage(string message)
    {
        if (!isDisplayingMessage)
        {
            StartCoroutine(DisplayQuestMessage(message));
        }
    }

    // Ddisplay quest messages
    private IEnumerator DisplayQuestMessage(string message)
    {
        isDisplayingMessage = true;

        // Set the quest text
        questText.text = message;

        // Wait for display duration
        yield return new WaitForSeconds(displayDuration);

        // Hide text after duration
        questText.text = "";
        isDisplayingMessage = false;
    }

    // Update the quest list UI
    private void UpdateQuestListUI()
    {
        // Initialize Quest List Text
        questListText.text = "Current Quest List:\n";

        // Each Quest
        foreach (Quest quest in quests)
        {
            // Show Progress
            string questStatus = quest.isCompleted ? " (Completed)" : " (In Progress)";

            // Not Complete will show description
            if(!quest.isCompleted)
            {
                questListText.text += quest.questName + ": " + questStatus + "\n" + "Description: " + quest.description + "\n"; // Quest List Text
            }
            else
            {
                questListText.text += quest.questName + ": " + questStatus + "\n\n"; // Quest List Text
            }
        }
    }
}

// Quest class to define each quest
[System.Serializable]
public class Quest
{
    public string questName; // Name of the quest
    public string description; // Description of the quest
    public bool isCompleted; // Status of the quest

    public Quest(string name, string desc)
    {
        questName = name;
        description = desc;
        isCompleted = false;
    }
}
