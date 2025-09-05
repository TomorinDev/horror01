using System.Collections.Generic;
using UnityEngine;

public class ShadowmanTriggerHandler : MonoBehaviour, IPlayerIsInRangeTrigger
{
    [Header("Triggers Activate & Deactivate Settings")]
    public List<GameObject> ShadowmanBehaviourTriggers; // Behaviour Trigger List
    public int NumberToDisable = 2; // Number to Disable
    

    // Proximity Trigger
    public bool playerInRange;
    public bool IsPlayerInRange => playerInRange;




    // Handle Shadowman Triggers
    private void HandleShadowmanTriggers()
    {
        // Get Amount of Triggers
        int total = ShadowmanBehaviourTriggers.Count;

        // Disable All
        if (NumberToDisable >= total)
        {
            Debug.LogWarning("NumberToDisable is greater than or equal to the total number of elements. Disabling all elements.");
            SetBehaviourTriggersState(false);
            return;
        }

        // Indices for Random
        List<int> indices = new List<int>();
        for (int i = 0; i < total; i++)
        {
            indices.Add(i);
        }

        // Shuffle
        Shuffle(indices);

        // Toggle Triggers Active
        ToggleTriggersActive(total, indices);
    }


    // Toggle Triggers Active: Random Activate & Deactivate
    private void ToggleTriggersActive(int total, List<int> indices)
    {
        // Disable Random
        for (int i = 0; i < NumberToDisable; i++)
        {
            Debug.Log("Deactivate Trigger");
            ShadowmanBehaviourTriggers[indices[i]].SetActive(false);
        }

        // Enable Rest
        for (int i = NumberToDisable; i < total; i++)
        {
            Debug.Log("Activate Trigger");
            ShadowmanBehaviourTriggers[indices[i]].SetActive(true);
        }
    }




    // Shuffle
    private void Shuffle(List<int> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            int temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }





    // Set Behaviour State
    private void SetBehaviourTriggersState(bool state)
    {
        foreach (var behaviourTriggers in ShadowmanBehaviourTriggers)
        {
            behaviourTriggers.SetActive(state);
        }
    }




    // IPlayerIsInRangeTrigger
    public void OnPlayerEnter()
    {
        playerInRange = true;
        HandleShadowmanTriggers();
    }

    public void OnPlayerExit()
    {
        playerInRange = false;
    }
}
